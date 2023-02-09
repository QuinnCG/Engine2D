namespace Engine2D.Math;

public struct Vector2Int
{
	public static Vector2Int Zero => new(0, 0);
	public static Vector2Int One => new(1, 1);

	public int X { get; set; }
	public int Y { get; set; }

	public float Magnitude => MathF.Sqrt(MathF.Pow(X, 2) + MathF.Pow(X, 2));
	public float MagnitudeSquared => MathF.Pow(X, 2) + MathF.Pow(X, 2);
	public Vector2Int Normalized => new Vector2Int(X, Y) / (int)Magnitude;

	public Vector2Int(int x, int y)
	{
		X = x;
		Y = y;
	}
	public Vector2Int(Vector2Int v)
	{
		X = v.X;
		Y = v.Y;
	}

	public static Vector2Int operator +(Vector2Int a, Vector2Int b) => new(a.X + b.X, a.Y + b.Y);
	public static Vector2Int operator -(Vector2Int a, Vector2Int b) => new(a.X - b.X, a.Y - b.Y);
	public static Vector2Int operator *(Vector2Int a, Vector2Int b) => new(a.X * b.X, a.Y * b.Y);
	public static Vector2Int operator /(Vector2Int a, Vector2Int b) => new(a.X / b.X, a.Y / b.Y);

	public static Vector2Int operator *(Vector2Int v, int f) => new(v.X * f, v.Y * f);
	public static Vector2Int operator /(Vector2Int v, int f) => new(v.X / f, v.Y / f);
}
