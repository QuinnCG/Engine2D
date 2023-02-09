namespace Engine2D;
public static class Time
{
	public static float Current { get; private set; }
	public static float Delta { get; private set; }

	internal static void Update(float time, float delta)
	{
		Current = time;
		Delta = delta;
	}
}
