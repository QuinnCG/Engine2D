using System.Diagnostics;
using Engine2D.Math;
using Silk.NET.Input;

namespace Engine2D;

public static class Input
{
	public static Vector2 MousePosition
	{
		get
		{
			Debug.Assert(_inputContext.Mice.Count > 0);
			var pos = _inputContext.Mice[0].Position;
			return new(pos.X, pos.Y);
		}
	}
	public static Vector2 MouseDelta { get; private set; }

	private static IInputContext _inputContext;
	private static Vector2 _lastMousePosition;

	private static readonly HashSet<Key> _keysHeld = new();
	private static readonly HashSet<Key> _keysPressedThisFrame = new();
	private static readonly HashSet<Key> _keysReleasedThisFrame = new();

	private static readonly HashSet<Button> _buttonHeld = new();
	private static readonly HashSet<Button> _buttonPressedThisFrame = new();
	private static readonly HashSet<Button> _buttonReleasedThisFrame = new();

	internal static void Initialize(IInputContext inputContext)
	{
		_inputContext = inputContext;

		_lastMousePosition = MousePosition;
		Application.OnUpdate += _ =>
		{
			MouseDelta = MousePosition - _lastMousePosition;
			_lastMousePosition = MousePosition;
		};

		foreach (var keyboard in inputContext.Keyboards)
		{
			keyboard.KeyDown += (_, key, _) =>
			{
				OnKeyDown((Key)key);
			};
			keyboard.KeyUp += (_, key, _) =>
			{
				OnKeyUp((Key)key);
			};
		}

		foreach (var mouse in inputContext.Mice)
		{
			mouse.MouseDown += (_, button) =>
			{
				OnButtonDown((Button)button);
			};
			mouse.MouseUp += (_, button) =>
			{
				OnButtonUp((Button)button);
			};
		}
	}

	public static bool IsDown(Key key)
	{
		return _keysPressedThisFrame.Contains(key);
	}
	public static bool IsDown(Button button)
	{
		return _buttonPressedThisFrame.Contains(button);
	}

	public static bool IsHeld(Key key)
	{
		return _keysHeld.Contains(key);
	}
	public static bool IsHeld(Button button)
	{
		return _buttonHeld.Contains(button);
	}

	public static bool IsUp(Key key)
	{
		return _keysReleasedThisFrame.Contains(key);
	}
	public static bool IsUp(Button button)
	{
		return _buttonReleasedThisFrame.Contains(button);
	}

	public static float GetAxis(Key positive, Key negative)
	{
		if (_keysHeld.Contains(positive))
			return 1f;
		else if (_keysHeld.Contains(negative))
			return -1f;
		else
			return 0f;
	}

	internal static void ClearInputsThisFrame()
	{
		_keysPressedThisFrame.Clear();
		_keysReleasedThisFrame.Clear();
	}

	private static void OnKeyDown(Key key)
	{
		_keysPressedThisFrame.Add(key);
		_keysHeld.Add(key);
	}

	private static void OnKeyUp(Key key)
	{
		_keysReleasedThisFrame.Add(key);
		_keysHeld.Remove(key);
	}

	private static void OnButtonDown(Button button)
	{
		_buttonHeld.Add(button);
		_buttonPressedThisFrame.Add(button);
	}

	private static void OnButtonUp(Button button)
	{
		_buttonHeld.Remove(button);
		_buttonReleasedThisFrame.Add(button);
	}
}
