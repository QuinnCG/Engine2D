using System.Numerics;

namespace Engine2D.Math;

public struct Vector2<T> where T : INumber<T>
{
	public static Vector2<T> Zero => new(T.Zero, T.Zero);
	public static Vector2<T> One => new(T.One, T.One);

    public static Vector2<T> Up => new(T.Zero, T.One);
    public static Vector2<T> Down => new(T.Zero, -T.One);
    public static Vector2<T> Right => new(T.One, T.Zero);
    public static Vector2<T> Left => new(-T.One, T.Zero);

    public T X { get; set; }
	public T Y { get; set; }

	public T MagnitudeSquared => (X * X) + (Y * Y);

	public Vector2(T x, T y)
	{
		X = x;
		Y = y;
	}
	public Vector2(Vector2<T> v)
	{
		X = v.X;
		Y = v.Y;
	}

    public static bool operator ==(Vector2<T> a, Vector2<T> b) => a.X == b.X && a.Y == b.Y;
    public static bool operator !=(Vector2<T> a, Vector2<T> b) => !(a.X == b.X && a.Y == b.Y);

    public static Vector2<T> operator +(Vector2<T> a, Vector2<T> b) => new(a.X + b.X, a.Y + b.Y);
	public static Vector2<T> operator -(Vector2<T> a, Vector2<T> b) => new(a.X - b.X, a.Y - b.Y);
	public static Vector2<T> operator *(Vector2<T> a, Vector2<T> b) => new(a.X * b.X, a.Y * b.Y);
	public static Vector2<T> operator /(Vector2<T> a, Vector2<T> b) => new(a.X / b.X, a.Y / b.Y);

	public static Vector2<T> operator *(Vector2<T> v, T f) => new(v.X * f, v.Y * f);
	public static Vector2<T> operator *(T f, Vector2<T> v) => new(v.X * f, v.Y * f);
	public static Vector2<T> operator /(Vector2<T> v, T f) => new(v.X / f, v.Y / f);
	public static Vector2<T> operator /(T f, Vector2<T> v) => new(v.X / f, v.Y / f);

    public override bool Equals(object? obj) => obj is Vector2<T> v && v == this;
    public override int GetHashCode() => HashCode.Combine(X, Y);
    public override string ToString() => $"<{X}, {Y}>";
}