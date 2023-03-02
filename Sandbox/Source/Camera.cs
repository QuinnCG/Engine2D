using Engine2D;
using Engine2D.Rendering;

namespace Sandbox;

class Camera : Entity
{
	private readonly Transform _transform;
	private readonly Transform _target;

	public Camera(Transform target)
	{
		_target = target;

		_transform = AddComponent<Transform>();
		AddComponent<CameraView>();
	}

    protected override void OnUpdate(float delta)
    {
		_transform.Position = _target.Position;
    }
}
