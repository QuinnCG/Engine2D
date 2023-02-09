namespace Engine2D;

public struct Color
{
	public static Color White => new(1f, 1f);

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
}
