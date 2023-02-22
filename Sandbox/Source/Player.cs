using Engine2D;
using Engine2D.Math;
using Engine2D.Rendering;
using Engine2D.Resources;

namespace Sandbox;

class Player : Entity
{
	private readonly Transform _transform;
	private readonly float _speed;

	public Player(float speed = 5f)
	{
		_transform = AddComponent<Transform>();

		var sprite = new Sprite(new Texture(Resource.Load<TextureResource>("Stump.png")));
		AddComponent(new SpriteRenderer(sprite, new Color(1f, 0f, 0f, 0.5f)));

		_speed = speed;
	}

	protected override void OnUpdate(float delta)
	{
		Vector2 inputDir = new Vector2()
		{
			X = Input.GetAxis(Key.D, Key.A),
			Y = Input.GetAxis(Key.W, Key.S)
		}.Normalized;
		_transform.Position += _speed * delta * inputDir;

		_transform.Rotation = Time.Current * 10f;
	}
}
