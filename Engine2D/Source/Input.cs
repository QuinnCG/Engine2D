using Silk.NET.Input;

namespace Engine2D;

public static class Input
{
	private static readonly HashSet<Key> _keysPressedThisFrame = new();
	private static readonly HashSet<Key> _keysReleasedThisFrame = new();
	private static readonly HashSet<Key> _keysHeld = new();

	internal static void Initialize(IInputContext inputContext)
	{
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
	}

	internal static void ClearInputsThisFrame()
	{
		_keysPressedThisFrame.Clear();
		_keysReleasedThisFrame.Clear();
	}

	public static bool IsKeyDown(Key key)
	{
		return _keysPressedThisFrame.Contains(key);
	}

	public static bool IsKeyHeld(Key key)
	{
		return _keysHeld.Contains(key);
	}

	public static bool IsKeyUp(Key key)
	{
		return _keysReleasedThisFrame.Contains(key);
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

	public static float GetAxis(Key positive, Key negative)
	{
		if (_keysHeld.Contains(positive))
			return 1f;
		else if (_keysHeld.Contains(negative))
			return -1f;
		else
			return 0f;
	}
}
