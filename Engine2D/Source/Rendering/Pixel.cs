namespace Engine2D.Rendering;

public struct Pixel
{
	public byte R;
	public byte G; 
	public byte B;
	public byte A;

	public Pixel(byte value)
	{
		R = G = B = value;
		A = 255;
	}
	public Pixel(byte value, byte alpha)
	{
		R = G = B = value;
		A = alpha;
	}
	public Pixel(byte r, byte g, byte b)
	{
		R = r; G = g; B = b;
		A = 255;
	}
	public Pixel(byte r, byte g, byte b, byte a)
	{
		R = r; G = g; B = b; A = a;
	}

	public override string ToString() => $"<{R}, {G}, {B}, {A}>";
}
