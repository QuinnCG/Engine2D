using Silk.NET.Maths;

namespace Engine2D.Rendering;

public class Camera : Entity
{
	public float OrthograpihcScale = 5f;

	internal static Camera Active { get; set; }

	private readonly Transform _transform;

	public Camera()
	{
		Active ??= this;
		_transform = AddComponent<Transform>();
	}

	public void SetActive()
	{
		Active = this;
	}

	internal Matrix4X4<float> GetMatrix()
	{
		var matrix = Matrix4X4.CreateTranslation(_transform.Position.X, _transform.Position.Y, 0f);
		return matrix *= Matrix4X4.CreateOrthographic(OrthograpihcScale * Application.Window.Ratio, OrthograpihcScale, 0.1f, 1f);
	}
}
