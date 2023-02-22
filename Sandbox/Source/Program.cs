using Engine2D;
using Engine2D.Rendering;
using Engine2D.Resources;

namespace Sandbox;

class Program
{
	private static void Main()
	{
		Application.OnLoad += OnLoad;
		Application.OnUpdate += delta =>
		{
			if (Input.IsKeyDown(Key.Escape))
			{
				Application.Window.Close();
			}

			Console.WriteLine(1f / delta);
		};
		Application.Start("Sandbox");
	}

	private static void OnLoad()
	{
		var world = new World();

		var entity = new Entity();
		entity.AddComponent<Transform>();

		var texture = new Texture(Resource.Load<TextureResource>("Campfire.png"), true);
		var sprite = new Sprite(texture);
		entity.AddComponent(new SpriteRenderer(sprite));
		world.AddEntity(entity);

		var camera = new Entity();
		camera.AddComponent<Transform>();
		camera.AddComponent<CameraView>();
		camera.AddComponent<Hover>();
		world.AddEntity(camera);

		var player = new Player();
		world.AddEntity(player);

		world.Load();
	}
}
