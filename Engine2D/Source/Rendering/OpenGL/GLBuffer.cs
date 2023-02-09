using System.Numerics;
using Silk.NET.OpenGL;

namespace Engine2D.Rendering.OpenGL;

public sealed class GLBuffer<T> : IDisposable where T : unmanaged, INumber<T>
{
	public uint Handle { get; }

	private readonly GL _gl;
	private readonly BufferTargetARB _target;

	public GLBuffer(GL gl, BufferTargetARB target, T[] data)
	{
		_gl = gl;
		_target = target;

		Handle = _gl.GenBuffer();
		Bind();
		_gl.BufferData(_target, new ReadOnlySpan<T>(data), BufferUsageARB.StaticDraw);
	}

	public void Bind()
	{
		_gl.BindBuffer(_target, Handle);
	}

	public void Dispose()
	{
		_gl.DeleteBuffer(Handle);
	}
}
