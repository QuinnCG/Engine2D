using Engine2D;
using Engine2D.Math;
using Engine2D.InputSystem;
using Engine2D.Rendering;
using Engine2D.Resources;

namespace Sandbox;

class Player : Entity
{
	private readonly Transform _transform;
	private readonly float _speed;

	private readonly List<Entity> _entities = new();
	private readonly Random _random = new();

	private float _lastFaceDir = 1f;

	public Player(float speed = 3f)
	{
        _speed = speed;
        _transform = AddComponent<Transform>();

		AddComponent<Animator>();
        AddComponent<SpriteRenderer>();
    }

    protected override void OnBegin()
    {
        var texture = new Texture(Resource.Load<TextureResource>("Player.png"));

        var animator = GetComponent<Animator>();
		animator.Animation = new Animation(0.2f, texture, new Vector2(0.5f, 1f), new Vector2(0f, -0.25f));
    }

    protected override void OnUpdate(float delta)
	{
		Vector2 inputDir = new Vector2()
		{
			X = Input.GetAxis(Key.D, Key.A),
			Y = Input.GetAxis(Key.W, Key.S)
		}.Normalized;

		_transform.Position += _speed * delta * inputDir;
		if (inputDir.X != 0f && inputDir.X != _lastFaceDir)
		{
			GetComponent<SpriteRenderer>().FlipX = inputDir.X < 0f;
			_lastFaceDir = inputDir.X;
		}

		if (Input.IsHeld(Key.R))
		{
			_transform.Rotation += 50f * Time.Delta;
		}

		if (Input.IsDown(Key.Up))
		{
			var entity = new Entity();
			var transform = entity.AddComponent<Transform>();
			transform.Position = new Vector2((_random.NextSingle() * 4) - 2f, (_random.NextSingle() * 4) - 2f);

			entity.AddComponent(new SpriteRenderer(new Color(_random.NextSingle(), _random.NextSingle(), _random.NextSingle(), 1f)));

			_entities.Add(entity);
			World.DefaultWorld.AddEntity(entity);
		}
		else if (Input.IsDown(Key.Down) && _entities.Count > 0)
		{
			World.DefaultWorld.RemoveEntity(_entities[^1]);
			_entities.RemoveAt(_entities.Count - 1);
		}
	}
}
