using Silk.NET.OpenGL;

namespace Engine2D.Rendering.OpenGL;

public sealed class GLVertexArray : IDisposable
{
	public uint Handle { get; }

	private readonly GL _gl;
	private readonly GLBuffer<float> _vbo;
	private readonly GLBuffer<uint> _ibo;

	public GLVertexArray(GL gl, GLBuffer<float> vbo, GLBuffer<uint> ibo)
	{
		_gl = gl;
		_vbo = vbo;
		_ibo = ibo;

		Handle = _gl.GenVertexArray();
		Bind();

		_vbo.Bind();
		_ibo.Bind();

		unsafe
		{
			_gl.EnableVertexAttribArray(0);
			_gl.VertexAttribPointer(0, 2, GLEnum.Float, false, sizeof(float) * 4, null);

			_gl.EnableVertexAttribArray(1);
			_gl.VertexAttribPointer(1, 2, GLEnum.Float, false, sizeof(float) * 4, (void*)(sizeof(float) * 2));
		}
	}

	public void Bind()
	{
		_gl.BindVertexArray(Handle);
	}

	public void Dispose()
	{
		_gl.DeleteVertexArray(Handle);

		_vbo.Dispose();
		_ibo.Dispose();
	}
}
