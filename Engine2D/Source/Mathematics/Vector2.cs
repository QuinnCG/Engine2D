namespace Engine2D.Math;

public struct Vector2
{
	public static Vector2 Zero => new(0f, 0f);
	public static Vector2 One => new(1f, 1f);

    public static Vector2 Up => new(0f, 1f);
    public static Vector2 Down => new(0f, -1f);
    public static Vector2 Right => new(1f, 0f);
    public static Vector2 Left => new(-1f, 0f);

    public float X { get; set; }
	public float Y { get; set; }

	public float Magnitude => MathF.Sqrt((X * X) + (Y * Y));
	public float MagnitudeSquared => MathF.Pow(X, 2) + MathF.Pow(Y, 2);
	public Vector2 Normalized
	{
		get
		{
			if (MagnitudeSquared == 0f) return Zero;
			return new Vector2(X, Y) / Magnitude;
		}
	}

	public Vector2(float x, float y)
	{
		X = x;
		Y = y;
	}
	public Vector2(Vector2 v)
	{
		X = v.X;
		Y = v.Y;
	}

	public static bool operator ==(Vector2 a, Vector2 b) => a.X == b.X && a.Y == b.Y;
	public static bool operator !=(Vector2 a, Vector2 b) => !(a.X == b.X && a.Y == b.Y);

    public static Vector2 operator +(Vector2 a, Vector2 b) => new(a.X + b.X, a.Y + b.Y);
	public static Vector2 operator -(Vector2 a, Vector2 b) => new(a.X - b.X, a.Y - b.Y);
	public static Vector2 operator *(Vector2 a, Vector2 b) => new(a.X * b.X, a.Y * b.Y);
	public static Vector2 operator /(Vector2 a, Vector2 b) => new(a.X / b.X, a.Y / b.Y);

	public static Vector2 operator +(Vector2 a, Vector2Int b) => new(a.X + b.X, a.Y + b.Y);
	public static Vector2 operator -(Vector2 a, Vector2Int b) => new(a.X - b.X, a.Y - b.Y);
	public static Vector2 operator *(Vector2 a, Vector2Int b) => new(a.X * b.X, a.Y * b.Y);
	public static Vector2 operator /(Vector2 a, Vector2Int b) => new(a.X / b.X, a.Y / b.Y);

    public static Vector2 operator +(Vector2Int a, Vector2 b) => new(a.X + b.X, a.Y + b.Y);
    public static Vector2 operator -(Vector2Int a, Vector2 b) => new(a.X - b.X, a.Y - b.Y);
    public static Vector2 operator *(Vector2Int a, Vector2 b) => new(a.X * b.X, a.Y * b.Y);
    public static Vector2 operator /(Vector2Int a, Vector2 b) => new(a.X / b.X, a.Y / b.Y);

    public static Vector2 operator *(Vector2 v, float f) => new(v.X * f, v.Y * f);
	public static Vector2 operator *(float f, Vector2 v) => new(v.X * f, v.Y * f);
	public static Vector2 operator /(Vector2 v, float f) => new(v.X / f, v.Y / f);
	public static Vector2 operator /(float f, Vector2 v) => new(v.X / f, v.Y / f);

	public override bool Equals(object? obj) => obj is Vector2 v && v == this;
	public override int GetHashCode() => HashCode.Combine(X, Y);
    public override string ToString() => $"<{X}, {Y}>";
}
