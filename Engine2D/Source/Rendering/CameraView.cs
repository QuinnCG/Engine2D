using Silk.NET.Maths;

namespace Engine2D.Rendering;

[RequireComponent<Transform>]
public class CameraView : Component
{
	public float OrthographicScale { get; set; } = 5f;
	public Color ClearColor { get; set; } = Color.Black;

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
		var matrix = Matrix4X4.CreateScale(_transform.Scale.X, _transform.Scale.Y, 1f);
		matrix *= Matrix4X4.CreateTranslation(-_transform.Position.X, -_transform.Position.Y, 0f);
		matrix *= Matrix4X4.CreateRotationZ(-_transform.Rotation * (MathF.PI / 180f));

		return matrix *= Matrix4X4.CreateOrthographic(OrthographicScale * Application.Window.Ratio, OrthographicScale, 0.1f, 1f);
	}
}
