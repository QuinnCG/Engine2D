namespace Engine2D;

public struct Color
{
	public static Color Transparent => new(0f, 0f);
	
	public static Color White => new(1f);
	public static Color Black => new(0f);

	public static Color Red => new(1f, 0f, 0f);
	public static Color Green => new(0f, 1f, 0f);
	public static Color Blue => new(0f, 0f, 1f);

	public static Color Cyan => new(0f, 1f, 1f);
	public static Color Yellow => new(1f, 1f, 0f);
	public static Color Magenta => new(1f, 0f, 1f);

	public float R { get; set; }
	public float G { get; set; }
	public float B { get; set; }
	public float A { get; set; }

	public Color()
	{
		R = G = B = A = 1f;
	}
	public Color(float value)
	{
		R = G = B = value;
		A = 1f;
	}
	public Color(float r, float g, float b)
	{
		R = r; G = g; B = b;
		A = 1f;
	}
	public Color(float r, float g, float b, float a)
	{
		R = r; G = g; B = b; A = a;
	}
	public Color(int r, int g, int b)
	{
		R = r / 255;
		G = g / 255;
		B = b / 255;
		A = 1f;
	}
	public Color(int r, int g, int b, float a)
	{
		R = r / 255;
		G = g / 255;
		B = b / 255;
		A = a;
	}
	public Color(float value, float alpha)
	{
		R = G = B = value;
		A = alpha;
	}

	public override string ToString() => $"<{R}, {G}, {B}, {A}>";
}
