using Engine2D.Rendering.OpenGL;
using Engine2D.Resources;

namespace Engine2D.Rendering;

public class Texture
{
	internal static Texture NoTexture { get; }

	public int Width { get; }
	public int Height { get; }

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
	}
	internal Texture(byte[] pixels, int width, int height, int channels, bool compress)
	{
		_glTexture = new GLTexture(Application.GL, pixels, width, height, channels, compress);
	}

	~Texture()
	{
		_glTexture.Dispose();
	}

	public void Bind()
	{
		_glTexture.Bind();
	}
}
