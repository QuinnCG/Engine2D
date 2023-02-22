using Silk.NET.OpenGL;

namespace Engine2D.Rendering.OpenGL;

internal sealed class GLTexture : IDisposable
{
	public uint Handle { get; }

	private readonly GL _gl;

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

		InternalFormat internalFormat;
		PixelFormat pixelFormat;

		switch (channels)
		{
			case 1:
				internalFormat = InternalFormat.R8;
				pixelFormat = PixelFormat.Red;
				break;
			case 2:
				internalFormat = InternalFormat.RG8;
				pixelFormat = PixelFormat.RG;
				break;
			case 3:
				internalFormat = InternalFormat.Rgb8;
				pixelFormat = PixelFormat.Rgb;
				break;
			case 4:
				internalFormat = InternalFormat.Rgba8;
				pixelFormat = PixelFormat.Rgba;
				break;
			default:
				throw new Exception($"Textures must have 1, 2, 3, or 4 color channels not {channels}.");
		}

		var data = new ReadOnlySpan<byte>(pixels);
		_gl.TexImage2D(TextureTarget.Texture2D, 0, internalFormat, (uint)width, (uint)height, 0, pixelFormat, PixelType.UnsignedByte, data);
		_gl.GenerateMipmap(TextureTarget.Texture2D);
	}

	public void Bind()
	{
		_gl.ActiveTexture(TextureUnit.Texture0);
		_gl.BindTexture(TextureTarget.Texture2D, Handle);
	}

	public void Dispose()
	{
		_gl.DeleteTexture(Handle);
	}
}
