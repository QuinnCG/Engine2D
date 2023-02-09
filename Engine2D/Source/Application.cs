using Engine2D.Rendering;

namespace Engine2D;

public static class Application
{
	internal static Window Window { get; }

	static Application()
	{
		Window = new Window();

		Window.OnLoad += () => Renderer.Initialize(Window);
		Window.OnUpdate += delta =>
		{
			Time.Update(Window.Time, delta);
			foreach (var world in World.LoadedWorlds)
			{
				world.Update(delta);
			}
		};
		Window.OnRender += delta => Renderer.Draw();
		Window.OnClose += () => Renderer.Dispose();
	}

	public static void Run()
	{
		Window.Run();
	}
}
