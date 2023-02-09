using Engine2D;

namespace Sandbox;

public class Hover : Component
{
	private Transform _transform;

	protected override void OnBegin()
	{
		_transform = GetComponent<Transform>();
	}

	protected override void OnUpdate(float delta)
	{
		_transform.Position.Y = MathF.Sin(Time.Current);
	}
}
