using Engine2D.Math;

namespace Engine2D.Rendering;

public class Sprite
{
	public Texture Texture { get; }
	public int PixelsPerUnit { get; }
	public Vector2 Size { get; }
	public Vector2Int Offset { get; }

	public Sprite(Texture texture)
	{
		Texture = texture;
		PixelsPerUnit = texture.Height;
		Size = Vector2.One;
		Offset = Vector2Int.Zero;
	}
	public Sprite(Texture texture, int pixelsPerUnit)
	{
		Texture = texture;
		PixelsPerUnit = pixelsPerUnit;
		Size = Vector2.One;
		Offset = Vector2Int.Zero;
	}
	public Sprite(Texture texture, int pixelsPerUnit, Vector2 size)
	{
		Texture = texture;
		PixelsPerUnit = pixelsPerUnit;
		Size = size;
		Offset = Vector2Int.Zero;
	}
	public Sprite(Texture texture, int pixelsPerUnit, Vector2 size, Vector2Int offset)
	{
		Texture = texture;
		PixelsPerUnit = pixelsPerUnit;
		Size = size;
		Offset = offset;
	}
}
