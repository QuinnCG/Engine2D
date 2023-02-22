using Engine2D.Math;

namespace Engine2D.Rendering;

public struct Vertex
{
	// Position + UV + Color.
	internal static int FloatCount = 8;

	public Vector2 Position;
	public Vector2 UV;
	public Color Color;

	public Vertex()
	{
		Position = Vector2.Zero;
		UV = Vector2.Zero;
		Color = Color.White;
	}
	public Vertex(Vector2 position, Vector2 uv)
	{
		Position = position;
		UV = uv;
		Color = Color.White;
	}
	public Vertex(Vector2 position, Vector2 uv, Color color)
	{
		Position = position;
		UV = uv;
		Color = color;
	}

	internal float[] GetFloatArray()
	{
		return new float[]
		{
			Position.X, Position.Y,
			UV.X, UV.Y,
			Color.R, Color.G, Color.B, Color.A
		};
	}

	public override string ToString() => $"<{Position}, {UV}, {Color}>";
}
