using Engine2D;
using Engine2D.Math;

namespace Sandbox;

class Hover : Component
{
	private Transform _transform;

	protected override void OnBegin()
	{
		_transform = GetComponent<Transform>();
	}

	protected override void OnUpdate(float delta)
	{
		_transform.Position = new Vector2(_transform.Position.X, MathF.Sin(Time.Current));
	}
}
