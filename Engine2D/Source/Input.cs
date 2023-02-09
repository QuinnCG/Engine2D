using Silk.NET.Input;

namespace Engine2D;

public static class Input
{
	internal static void Initialize(IInputContext inputContext)
	{
		foreach (var keyboard in inputContext.Keyboards)
		{
			keyboard.KeyDown += OnKeyDown;
			keyboard.KeyUp += OnKeyUp;
		}
	}

	public static bool IsKeyDown()
	{
		return false;
	}

	public static bool IsKeyHeld()
	{
		return false;
	}

	public static bool IsKeyUp()
	{
		return false;
	}

	private static void OnKeyDown(IKeyboard keyboard, Key key, int scancode)
	{

	}

	private static void OnKeyUp(IKeyboard keyboard, Key key, int scancode)
	{

	}
}
