using Engine2D.Rendering.OpenGL;
using Engine2D.Resources;

namespace Engine2D.Rendering;

public class Texture
{
	public int Width { get; }
	public int Height { get; }

	private readonly GLTexture _glTexture;

	public Texture(TextureResource resource, bool compress = false)
	{
		_glTexture = new GLTexture(Application.GL, resource.Pixels, resource.Width, resource.Height, resource.Channels, compress);

		Width = resource.Width;
		Height = resource.Height;
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
