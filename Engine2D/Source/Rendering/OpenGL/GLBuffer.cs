using System.Numerics;
using System.Runtime.InteropServices;
using Silk.NET.OpenGL;

namespace Engine2D.Rendering.OpenGL;

internal sealed class GLBuffer<T> : IDisposable where T : unmanaged, INumber<T>
{
	public uint Handle { get; private set; }

	private readonly GL _gl;
	private readonly BufferTargetARB _target;

	private int _size;

	public GLBuffer(GL gl, BufferTargetARB target, int initialSize = 2)
	{
		_gl = gl;
		_target = target;

		CreateDynamicBuffer(initialSize);
	}
	public GLBuffer(GL gl, BufferTargetARB target, T[] data)
	{
		_gl = gl;
		_target = target;

		Handle = _gl.GenBuffer();
		Bind();
		_gl.BufferData(target, new ReadOnlySpan<T>(data), BufferUsageARB.StaticDraw);
	}

	public void Bind()
	{
		_gl.BindBuffer(_target, Handle);
	}

	public void Update(T[] data)
	{
		int requiredSize = Marshal.SizeOf<T>() * data.Length;
		int newSize = _size;

		if (requiredSize > _size)
		{
			while (newSize < requiredSize)
			{
				newSize *= 2;
			}
		}
		else if (requiredSize < _size / 2)
		{
			while (newSize > requiredSize * 2)
			{
				newSize /= 2;
			}
		}

		newSize = System.Math.Max(newSize, 2);
		if (newSize != _size)
		{
			Dispose();
			CreateDynamicBuffer(newSize);
		}

		Bind();
		_gl.BufferSubData(_target, 0, new ReadOnlySpan<T>(data));
	}

	public void Dispose()
	{
		_gl.DeleteBuffer(Handle);
	}

	private unsafe void CreateDynamicBuffer(int size)
	{
		Handle = _gl.GenBuffer();
		Bind();

		_size = size;
		_gl.BufferData(_target, (nuint)size, null, BufferUsageARB.DynamicDraw);
	}
}
