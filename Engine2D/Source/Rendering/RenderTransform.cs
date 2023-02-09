using Engine2D.Math;

namespace Engine2D.Rendering;

internal struct RenderTransform
{
	public Vector2 Position;
	public float Rotation;
	public Vector2 Scale = Vector2.One;

	public RenderTransform()
	{
		Scale = Vector2.One;
	}
	public RenderTransform(Vector2 position)
	{
		Position = position;
	}
	public RenderTransform(Vector2 position, float rotation)
	{
		Position = position;
		Rotation = rotation;
	}
	public RenderTransform(Vector2 position, float rotation, Vector2 scale)
	{
		Position = position;
		Rotation = rotation;
		Scale = scale;
	}
}
