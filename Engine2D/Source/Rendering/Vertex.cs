using Engine2D.Math;

namespace Engine2D.Rendering;

public struct Vertex
{
	// Position + UV.
	internal static int FloatCount = 4;

	public Vector2 Position;
	public Vector2 UV;

	public Vertex(Vector2 position, Vector2 uv)
	{
		Position = position;
		UV = uv;
	}

	internal float[] GetFloatArray()
	{
		return new float[]
		{
			Position.X, Position.Y,
			UV.X, UV.Y
		};
	}

	public override string ToString() => $"<{Position}, {UV}>";
}
