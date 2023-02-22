namespace Engine2D.Rendering;

[RequireComponent<Transform>]
public sealed class SpriteRenderer : Component
{
	public Sprite? Sprite { get; set; }
	public Color Color { get; set; } = Color.White;

	private Transform _transform;

	public SpriteRenderer() { }
	public SpriteRenderer(Sprite sprite)
	{
		Sprite = sprite;
	}
	public SpriteRenderer(Color color)
	{
		Color = color;
	}
	public SpriteRenderer(Sprite sprite, Color color)
	{
		Sprite = sprite;
		Color = color;
	}

	protected override void OnBegin()
	{
		_transform = GetComponent<Transform>();
	}

	protected override void OnUpdate(float delta) { }

	protected override void OnRender(float delta)
	{
		Renderer.Submit(new RenderObject()
		{
			Sprite = Sprite,
			Color = Color,
			Transform = new RenderTransform()
			{
				Position = _transform.Position,
				Rotation = _transform.Rotation,
				Scale = _transform.Scale
			}
		});
	}
}
