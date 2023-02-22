using Engine2D.Math;

namespace Engine2D.Rendering;

[RequireComponent<Transform>]
public sealed class SpriteRenderer : Component
{
	private static readonly Vertex[] _quadVertices =
	{
		new Vertex(new Vector2(-0.5f, -0.5f), new Vector2(0f, 0f)),
		new Vertex(new Vector2(-0.5f,  0.5f), new Vector2(0f, 1f)),
		new Vertex(new Vector2( 0.5f,  0.5f), new Vector2(1f, 1f)),
		new Vertex(new Vector2( 0.5f, -0.5f), new Vector2(1f, 0f)),
	};
	private static readonly uint[] _quadIndices =
	{
		0, 1, 2,
		3, 0, 2
	};

	public Sprite? Sprite
	{
		get => _sprite;
		set
		{
			_sprite = value;
			_renderBatch = new(_sprite?.Texture);
		}
	}
	public Color Color { get; set; } = Color.White;

	private Sprite? _sprite;
	private RenderBatch _renderBatch;
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
		_renderBatch = new(Sprite?.Texture);
		_transform = GetComponent<Transform>();

		Application.OnRender += OnRender;
	}

	private void OnRender(float delta)
	{
		_renderBatch.Clear();
		_renderBatch.Submit(new RenderObject()
		{
			Sprite = Sprite,
			Color = Color,

			Vertices = _quadVertices,
			Indices = _quadIndices,

			Transform = new RenderTransform()
			{
				Position = _transform.Position,
				Rotation = _transform.Rotation,
				Scale = _transform.Scale
			}
		});
		Renderer.Submit(_renderBatch);
	}

	protected override void OnEnd()
	{
		_renderBatch.Dispose();
	}
}
