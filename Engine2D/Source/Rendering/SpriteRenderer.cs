using Engine2D.Math;

namespace Engine2D.Rendering;

[RequireComponent<Transform>]
public sealed class SpriteRenderer : Component
{
	public Sprite? Sprite { get; set; }
	public Color Color { get; set; } = Color.White;
	public RenderLayer RenderLayer { get; set; } = RenderLayer.Default;

	public bool FlipX { get; set; } = false;
	public bool FlipY { get; set; } = false;

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
	public SpriteRenderer(RenderLayer layer)
	{
		RenderLayer = layer;
	}
	public SpriteRenderer(Sprite sprite, Color color)
	{
		Sprite = sprite;
		Color = color;
	}
	public SpriteRenderer(Sprite sprite, Color color, RenderLayer layer)
	{
		Sprite = sprite;
		Color = color;
		RenderLayer = layer;
	}

	// TODO: Implement sprite masking.
	// It could reference a sprite mask component.
	// It could use the mask's sprite as well as its entity's transform,
	// and the mask mode to stencil buffer this sprite.

	public static Entity CreateSpriteEntity(Sprite sprite, Color color, RenderLayer layer)
	{
		var entity = new Entity();
		entity.AddComponent<Transform>();
		entity.AddComponent(new SpriteRenderer(sprite, color, layer));
		return entity;
	}
	public static Entity CreateSpriteEntity(Sprite sprite, Color color)
	{
		return CreateSpriteEntity(sprite, color, RenderLayer.Default);
	}
	public static Entity CreateSpriteEntity(Sprite sprite, RenderLayer layer)
	{
		return CreateSpriteEntity(sprite, Color.White, layer);
	}
	public static Entity CreateSpriteEntity(Sprite sprite)
	{
		return CreateSpriteEntity(sprite, Color.White, RenderLayer.Default);
	}

	protected override void OnBegin()
	{
		_transform = GetComponent<Transform>();
	}

	protected override void OnRender(float delta)
	{
		var orientation = new Vector2()
		{
			X = FlipX ? -1f : 1f,
			Y = FlipY ? -1f : 1f,
		};
		Renderer.Submit(new RenderObject()
		{
			Sprite = Sprite,
			Color = Color,
			Layer = RenderLayer,

			Transform = new RenderTransform()
			{
				Position = _transform.Position,
				Rotation = _transform.Rotation,
				Scale = _transform.Scale * orientation
			}
		});
	}
}
