using Silk.NET.OpenGL;

namespace Engine2D.Rendering.OpenGL;

internal sealed class GLTexture : IDisposable
{
	public uint Handle { get; }

	private readonly GL _gl;

	private readonly InternalFormat _internalFormat;
	private readonly PixelFormat _pixelFormat;

	private readonly int _width;
    private readonly int _height;
    private readonly int _channels;

	public GLTexture(GL gl, byte[] pixels, int width, int height, int channels, bool compress = false)
	{
		_gl = gl;

		Handle = _gl.GenTexture();
		Bind();

		var compression = compress ? (int)GLEnum.Linear : (int)GLEnum.Nearest;

		_gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, compression);
		_gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, compression);

		_gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)GLEnum.Repeat);
		_gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)GLEnum.Repeat);

		_width = width;
		_height = height;
		_channels = channels;

		switch (channels)
		{
			case 1:
				_internalFormat = InternalFormat.R8;
				_pixelFormat = PixelFormat.Red;
				break;
			case 2:
				_internalFormat = InternalFormat.RG8;
				_pixelFormat = PixelFormat.RG;
				break;
			case 3:
				_internalFormat = InternalFormat.Rgb8;
				_pixelFormat = PixelFormat.Rgb;
				break;
			case 4:
				_internalFormat = InternalFormat.Rgba8;
				_pixelFormat = PixelFormat.Rgba;
				break;
			default:
				throw new Exception($"Textures must have 1, 2, 3, or 4 color channels not {channels}.");
		}

		var data = new ReadOnlySpan<byte>(pixels);
		_gl.TexImage2D(TextureTarget.Texture2D, 0, _internalFormat, (uint)width, (uint)height, 0, _pixelFormat, PixelType.UnsignedByte, data);
		_gl.GenerateMipmap(TextureTarget.Texture2D);
	}

	public void Bind()
	{
		_gl.ActiveTexture(TextureUnit.Texture0);
		_gl.BindTexture(TextureTarget.Texture2D, Handle);
	}

	public byte[] GetPixels()
	{
		Bind();

		var pixels = new Span<byte>(new byte[_width * _height * _channels]);
		_gl.GetTexImage(TextureTarget.Texture2D, 0, _pixelFormat, PixelType.UnsignedByte, pixels);

		return pixels.ToArray();
	}

	public void SetPixels(byte[] pixels)
	{
		Bind();

		var span = new ReadOnlySpan<byte>(pixels);
		_gl.TexImage2D(TextureTarget.Texture2D, 0, _internalFormat, (uint)_width, (uint)_height, 0, _pixelFormat, PixelType.UnsignedByte, span);
	}
	public void SetPixels(byte[] pixels, int x, int y, int width, int height)
	{
		Bind();

		var span = new ReadOnlySpan<byte>(pixels);
		_gl.TexSubImage2D(TextureTarget.Texture2D, 0, x, y, (uint)width, (uint)height, _pixelFormat, PixelType.UnsignedByte, span);
	}

	public void Dispose()
	{
		_gl.DeleteTexture(Handle);
	}
}
