using System.Text;
using Engine2D.Rendering;
using Silk.NET.OpenGL;

namespace Engine2D;

public static class Application
{
	public static Window Window { get; private set; }

	public static event Action OnLoad;
	public static event Action<float>? OnUpdate;
	public static event Action<float>? OnRender;

	public static int FPS { get; private set; }

	internal static GL GL { get; private set; }

	private static int _lastDebugMsgID = -1;

	public static void Start(string title = "Engine2D", int width = 1200, int height = 1000, int antiAliasingSamples = 2)
	{
		Window = new Window(title, width, height, antiAliasingSamples);
		Window.OnLoad += Load;

		Window.OnUpdate += delta =>
		{
			FPS = (int)(1f / delta);
			OnUpdate?.Invoke(delta);

			Time.Update(Window.Time, delta);
			foreach (var world in World.ActiveWorlds)
			{
				world.Update(delta);
			}

			Input.ClearInputsThisFrame();
		};
		Window.OnRender += delta =>
		{
			OnRender?.Invoke(delta);
			foreach (var world in World.ActiveWorlds)
			{
				world.Render(delta);
			}

			Renderer.Draw();
		};
		Window.OnClose += () => Renderer.Dispose();

		Window.Run();
	}

	private static unsafe void Load()
	{
		GL = Window.CreateOpenGLContext();

#if DEBUG
		GL.Enable(EnableCap.DebugOutputSynchronous);
		GL.DebugMessageCallback((source, type, id, severity, length, message, userParam) =>
		{
			if (id == _lastDebugMsgID)
			{
				return;
			}

			_lastDebugMsgID = id;

			if (severity is GLEnum.DebugSeverityLow or GLEnum.DebugSeverityNotification or GLEnum.DontCare)
			{
				return;
			}

			var msg = Encoding.Default.GetString((byte*)message.ToPointer(), length);
			Console.Write("[OpenGL Error]: ");
			Console.WriteLine(msg);
		}, null);
#endif

		Renderer.Initialize(Window);
		Input.Initialize(Window.CreateInputContext());

		OnLoad?.Invoke();
	}
}
