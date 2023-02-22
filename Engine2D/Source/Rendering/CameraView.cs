using Silk.NET.Maths;

namespace Engine2D.Rendering;

[RequireComponent<Transform>]
public class CameraView : Component
{
	public float OrthograpihcScale = 5f;

	internal static CameraView Active { get; set; }

	private Transform _transform;

	public CameraView()
	{
		Active ??= this;
	}

	protected override void OnBegin()
	{
		_transform = GetComponent<Transform>();
	}

	public void SetActive()
	{
		Active = this;
	}

	internal Matrix4X4<float> GetMatrix()
	{
		var matrix = Matrix4X4.CreateTranslation(-_transform.Position.X, -_transform.Position.Y, 0f);
		return matrix *= Matrix4X4.CreateOrthographic(OrthograpihcScale * Application.Window.Ratio, OrthograpihcScale, 0.1f, 1f);
	}
}
