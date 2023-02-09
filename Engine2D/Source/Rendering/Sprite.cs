using Engine2D.Math;

namespace Engine2D.Rendering;

public class Sprite
{
	public Texture Texture { get; }
	public int PixelsPerUnit { get; }
	public Vector2Int Size { get; }
	public int Index;

	public Sprite(Texture texture, int pixelsPerUnit)
	{
		Texture = texture;
		PixelsPerUnit = pixelsPerUnit;
		Size = Vector2Int.One;
		Index = 0;
	}
	public Sprite(Texture texture, int pixelsPerUnit, Vector2Int size, int index = 0)
	{
		Texture = texture;
		PixelsPerUnit = pixelsPerUnit;
		Size = size;
		Index = index;
	}
}
