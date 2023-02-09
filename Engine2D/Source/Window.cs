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

	public string Title { get; }
	public int Width { get; private set; }
	public int Height { get; private set; }

	public float Ratio => (float)Width / Height;

	public float Time => (float)_window.Time;

	private readonly IWindow _window;

	public Window(string title = "Sandbox", int width = 1200, int height = 1000)
	{
		var options = WindowOptions.Default;
		options.Title = title;
		options.Size = new(width, height);

		Title = title;
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
