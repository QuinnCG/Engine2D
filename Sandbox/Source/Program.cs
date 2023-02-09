using Engine2D;
using Engine2D.Rendering;

namespace Sandbox;

class Program
{
	private static void Main()
	{
		var world = new World();

		var entity = new Entity();
		entity.AddComponent<Transform>();
		entity.AddComponent<SpriteRenderer>();

		var camera = new Camera();
		camera.AddComponent<Hover>();

		world.Load();
		Application.Run();
	}
}
