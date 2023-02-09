namespace Engine2D.Rendering;

[RequireComponent<Transform>]
public sealed class SpriteRenderer : Component
{
	private Transform _transform;

	public SpriteRenderer()
	{
		Application.Window.OnRender += OnRender;
	}

	protected override void OnBegin()
	{
		_transform = GetComponent<Transform>();
	}

	private void OnRender(float delta)
	{
		Renderer.Submit(new RenderObject()
		{
			Transform = new()
			{
				Position = _transform.Position,
				Rotation = _transform.Rotation,
				Scale = _transform.Scale
			}
		});
	}
}
