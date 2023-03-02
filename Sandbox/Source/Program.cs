using Engine2D;
using Engine2D.InputSystem;
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
			if (Input.IsDown(Key.Escape))
			{
				Application.Window.Close();
			}

			//Console.WriteLine(Application.FPS);
		};
		Application.Start("Sandbox");
	}

	private static void OnLoad()
	{
		var world = new World();

		var entity = new Entity();
		entity.AddComponent<Transform>().Scale *= 2f;
		var texture = new Texture(Resource.Load<TextureResource>("Campfire.png"), true);
		var sprite = new Sprite(texture);
		entity.AddComponent(new SpriteRenderer(sprite));
		world.AddEntity(entity);

        var player = new Player();
        world.AddEntity(player);

		var camera = new Camera(player.GetComponent<Transform>());
		world.AddEntity(camera);

		var bgLayer = new RenderLayer();
		RenderLayer.AddBefore(bgLayer, RenderLayer.Default);

        texture = new Texture(Resource.Load<TextureResource>("Purple.jpg"), true);
        sprite = new Sprite(texture, 256);
        world.AddEntity(SpriteRenderer.CreateSpriteEntity(sprite, bgLayer));

        world.Load();
	}
}
