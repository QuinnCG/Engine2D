using Engine2D.Rendering.OpenGL;
using Engine2D.Resources;

namespace Engine2D.Rendering;

public class Texture
{
	internal static Texture NoTexture { get; }

	public int Width { get; }
	public int Height { get; }
	public int Channels { get; }

	private readonly GLTexture _glTexture;

	static Texture()
	{
		var pixels = new byte[]
		{
			byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue
		};

		NoTexture = new(pixels, 1, 1, 4, false);
	}

	public Texture(TextureResource resource, bool compress = false)
	{
		_glTexture = new GLTexture(Application.GL, resource.Pixels, resource.Width, resource.Height, resource.Channels, compress);

		Width = resource.Width;
		Height = resource.Height;
		Channels = resource.Channels;
	}
	internal Texture(byte[] pixels, int width, int height, int channels, bool compress = false)
	{
		_glTexture = new GLTexture(Application.GL, pixels, width, height, channels, compress);

		Width = width;
		Height = height;
		Channels = channels;
	}

	~Texture()
	{
		_glTexture.Dispose();
	}

	public Pixel[] GetPixels()
	{
		var pixels = new Pixel[Width * Height];
		byte[] rawPixels = _glTexture.GetPixels();

        if (Channels == 1)
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    int index = (y * Width) + x;
                    pixels[index] = new Pixel()
                    {
                        R = rawPixels[index + 0]
                    };
                }
            }
        }
        else if (Channels == 2)
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    int index = (y * Width) + x;
                    pixels[index] = new Pixel()
                    {
                        R = rawPixels[index + 0],
                        G = rawPixels[index + 1]
                    };
                }
            }
        }
        else if (Channels == 3)
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    int index = (y * Width) + x;
                    pixels[index] = new Pixel()
                    {
                        R = rawPixels[index + 0],
                        G = rawPixels[index + 1],
                        B = rawPixels[index + 2]
                    };
                }
            }
        }
        else if (Channels == 4)
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    int index = (y * Width) + x;

                    pixels[index] = new Pixel()
                    {
                        R = rawPixels[index + 0],
                        G = rawPixels[index + 1],
                        B = rawPixels[index + 2],
                        A = rawPixels[index + 3]
                    };
                }
            }
        }

        return pixels;
	}

    public void SetPixels(Pixel[] pixels)
    {
        _glTexture.SetPixels(PixelsToBytes(pixels));
    }
    public void SetPixels(Pixel[] pixels, int x, int y, int width, int height)
    {
        _glTexture.SetPixels(PixelsToBytes(pixels), x, y, width, height);
    }

    internal void Bind()
	{
		_glTexture.Bind();
	}

    private byte[] PixelsToBytes(Pixel[] pixels)
    {
        var bytes = new byte[pixels.Length * Channels];

        if (Channels == 1)
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    int index = (y * Width) + x;
                    bytes[index] = pixels[index + 0].R;
                }
            }
        }
        else if (Channels == 2)
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    int index = (y * Width) + x;
                    bytes[index] = pixels[index + 0].R;
                    bytes[index] = pixels[index + 1].G;
                }
            }
        }
        else if (Channels == 3)
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    int index = (y * Width) + x;
                    bytes[index] = pixels[index + 0].R;
                    bytes[index] = pixels[index + 1].G;
                    bytes[index] = pixels[index + 2].B;
                }
            }
        }
        else if (Channels == 4)
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    int index = (y * Width) + x;
                    bytes[index] = pixels[index + 0].R;
                    bytes[index] = pixels[index + 1].G;
                    bytes[index] = pixels[index + 2].B;
                    bytes[index] = pixels[index + 3].A;
                }
            }
        }

        return bytes;
    }
}
