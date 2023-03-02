using Engine2D.Math;

namespace Engine2D.Rendering;

public class Sprite
{
	public Texture Texture { get; }

	public int Height { get; private set; }
	public int Width { get; private set; }

    public int PixelsPerUnit { get; set; }
    public Vector2 UVScale
	{
		get => _uvScale;
		set
		{
			_uvScale = value;
            Width = (int)(Texture.Width * value.X);
            Height = (int)(Texture.Height * value.Y);
        }
	}
	public Vector2Int UVIndex { get; set; }
    public Vector2 Pivot { get; set; }

	private Vector2 _uvScale;

    public Sprite(Texture texture)
	{
		Texture = texture;
		PixelsPerUnit = texture.Height;
		UVScale = Vector2.One;
		UVIndex = Vector2Int.Zero;

		Width = texture.Width;
		Height = texture.Height;
	}
	public Sprite(Texture texture, int pixelsPerUnit)
	{
		Texture = texture;
		PixelsPerUnit = pixelsPerUnit;
		UVScale = Vector2.One;
		UVIndex = Vector2Int.Zero;

		Width = texture.Width;
		Height = texture.Height;
	}
	public Sprite(Texture texture, Vector2 uvScale, Vector2Int uvIndex)
	{
		Texture = texture;
		PixelsPerUnit = texture.Height;
		UVScale = uvScale;
		UVIndex = uvIndex;

		Width = (int)(texture.Width * uvScale.X);
		Height = (int)(texture.Height * uvScale.Y);
	}
	public Sprite(Texture texture, int pixelsPerUnit, Vector2 uvScale, Vector2Int uvIndex)
	{
		Texture = texture;
		PixelsPerUnit = pixelsPerUnit;
		UVScale = uvScale;
		UVIndex = uvIndex;

		Width = (int)(texture.Width * uvScale.X);
		Height = (int)(texture.Height * uvScale.Y);
	}
    public Sprite(Texture texture, int pixelsPerUnit, Vector2 uvScale, Vector2Int uvIndex, Vector2 pivot)
    {
        Texture = texture;
        PixelsPerUnit = pixelsPerUnit;
        UVScale = uvScale;
        UVIndex = uvIndex;
		Pivot = pivot;

        Width = (int)(texture.Width * uvScale.X);
        Height = (int)(texture.Height * uvScale.Y);
    }

    public static Sprite[] CreateSpriteSheet(Texture texture, Vector2 uvScale, int pixelsPerUnit, Vector2 pivot)
	{
		List<Sprite> sprites = new();

		var spriteResolution = new Vector2Int()
		{
			X = (int)(texture.Width * uvScale.X),
			Y = (int)(texture.Height * uvScale.Y)
		};

		int xSprites = texture.Width / spriteResolution.X;
		int ySprites = texture.Height / spriteResolution.Y;

		for (int y = 0; y < ySprites; y++)
		{
			for (int x = 0; x < xSprites; x++)
			{
				sprites.Add(new Sprite(texture, pixelsPerUnit, uvScale, new Vector2Int(x, y), pivot));
            }
		}

		return sprites.ToArray();
	}
	public static Sprite[] CreateSpriteSheet(Texture texture, Vector2 uvScale)
	{
		return CreateSpriteSheet(texture, uvScale, texture.Height, Vector2.Zero);
	}
    public static Sprite[] CreateSpriteSheet(Texture texture, Vector2 uvScale, Vector2 pivot)
    {
        return CreateSpriteSheet(texture, uvScale, texture.Height, pivot);
    }
    public static Sprite[] CreateSpriteSheet(Texture texture, Vector2 uvScale, int pixelsPerUnit)
    {
        return CreateSpriteSheet(texture, uvScale, pixelsPerUnit, Vector2.Zero);
    }
}
