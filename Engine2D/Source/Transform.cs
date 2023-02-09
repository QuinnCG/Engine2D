using Engine2D.Math;

namespace Engine2D;

public sealed class Transform : Component
{
	public Vector2 Position;
	public float Rotation;
	public Vector2 Scale = Vector2.One;
}
