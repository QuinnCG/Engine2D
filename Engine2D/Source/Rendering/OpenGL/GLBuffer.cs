using System.Numerics;
using Silk.NET.OpenGL;

namespace Engine2D.Rendering.OpenGL;

internal sealed class GLBuffer<T> : IDisposable where T : unmanaged, INumber<T>
{
	public uint Handle { get; private set; }

	private readonly GL _gl;
	private readonly BufferTargetARB _target;

	public GLBuffer(GL gl, BufferTargetARB target, int size = 64)
	{
		_gl = gl;
		_target = target;

		CreateDynamicBuffer(size);
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

		_gl.BufferData(_target, (nuint)size, null, BufferUsageARB.DynamicDraw);
	}
}
