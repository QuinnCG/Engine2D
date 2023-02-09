using Engine2D.Math;

namespace Engine2D.Rendering;

internal struct Vertex
{
	// Position + UV.
	public static int FloatCount = 2 + 2;

	public Vector2 Position;
	public Vector2 UV;

	public Vertex()
	{
		Position = new();
		UV = new();
	}
}
