using Silk.NET.Input;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;

using SilkWindow = Silk.NET.Windowing.Window;

namespace Engine2D;

public class Window
{
	public event Action? OnLoad;
	public event Action<float>? OnUpdate;
	public event Action<float>? OnRender;
	public event Action? OnClose;
	public event Action<int, int>? OnResize;

	public string Title { get => _window.Title; set => _window.Title = value; }
	public int Width { get; private set; }
	public int Height { get; private set; }
	public bool VSync { get => _window.VSync; set => _window.VSync = value; }
	public WindowMode Mode
	{
		get => (WindowMode)_window.WindowState;
		set => _window.WindowState = value switch
		{
			WindowMode.Windowed => WindowState.Normal,
			WindowMode.Minimized => WindowState.Minimized,
			WindowMode.Maximized => WindowState.Maximized,
			WindowMode.Fullscreen => WindowState.Fullscreen,
			_ => throw new Exception()
		};
	}
	public int RefreshRate
	{
		get
		{
			var rate = _window.VideoMode.RefreshRate;
			if (rate == null) throw new NullReferenceException();
			return (int)rate;
		}
	}

	public float Ratio => (float)Width / Height;

	internal float Time => (float)_window.Time;

	private readonly IWindow _window;

	public Window(string title = "Engine2D", int width = 1200, int height = 1000, int antiAliasingSamples = 2)
	{
		var options = WindowOptions.Default;
		options.Title = title;
		options.Size = new(width, height);
		options.Samples = antiAliasingSamples;

		var api = GraphicsAPI.Default;
		api.Profile = ContextProfile.Core;
		api.Version = new APIVersion(4, 3);
#if DEBUG
		api.Flags |= ContextFlags.Debug;
#endif
		options.API = api;

		Width = width;
		Height = height;

		_window = SilkWindow.Create(options);

		_window.Load += () => OnLoad?.Invoke();
		_window.Update += delta => OnUpdate?.Invoke((float)delta);
		_window.Render += delta => OnRender?.Invoke((float)delta);
		_window.Closing += OnClose;

		_window.Resize += size =>
		{
			Width = size.X;
			Height = size.Y;

			OnResize?.Invoke(Width, Height);
		};
	}

	public void Run()
	{
		_window.Run();
	}

	public void Close()
	{
		_window.Close();
	}

	internal IInputContext CreateInputContext()
	{
		return _window.CreateInput();
	}

	internal GL CreateOpenGLContext()
	{
		return GL.GetApi(_window);
	}
}
