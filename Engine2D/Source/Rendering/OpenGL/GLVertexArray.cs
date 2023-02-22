using Silk.NET.OpenGL;

namespace Engine2D.Rendering.OpenGL;

internal sealed class GLVertexArray : IDisposable
{
	public uint Handle { get; }

	private readonly GL _gl;

	private readonly GLBuffer<float> _vbo;
	private readonly GLBuffer<uint> _ibo;

	public GLVertexArray(GL gl)
	{
		_gl = gl;
		Handle = _gl.GenVertexArray();

		_vbo = new GLBuffer<float>(gl, BufferTargetARB.ArrayBuffer);
		_ibo = new GLBuffer<uint>(gl, BufferTargetARB.ElementArrayBuffer);

		Bind();
	}
	public GLVertexArray(GL gl, GLBuffer<float> vbo, GLBuffer<uint> ibo)
	{
		_gl = gl;
		_vbo = vbo;
		_ibo = ibo;

		Handle = _gl.GenVertexArray();
		Bind();
	}

	public unsafe void Bind()
	{
		_gl.BindVertexArray(Handle);
		_vbo.Bind();
		_ibo.Bind();

		uint stride = (uint)(sizeof(float) * Vertex.FloatCount);

		_gl.EnableVertexAttribArray(0);
		_gl.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, stride, null);

		_gl.EnableVertexAttribArray(1);
		_gl.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, stride, (void*)(sizeof(float) * 2));

		_gl.EnableVertexAttribArray(2);
		_gl.VertexAttribPointer(2, 4, VertexAttribPointerType.Float, false, stride, (void*)(sizeof(float) * 4));
	}

	public void Update(Vertex[] vertices)
	{
		_vbo.Update(VerticesToFloats(vertices));
		Bind();
	}
	public void Update(uint[] indices)
	{
		_ibo.Update(indices);
		Bind();
	}
	public void Update(Vertex[] vertices, uint[] indices)
	{
		_vbo.Update(VerticesToFloats(vertices));
		_ibo.Update(indices);
		Bind();
	}

	public void Dispose()
	{
		_gl.DeleteVertexArray(Handle);

		_vbo.Dispose();
		_ibo.Dispose();
	}

	private static float[] VerticesToFloats(Vertex[] vertices)
	{
		var floats = new List<float>();
		foreach (var vertex in vertices)
		{
			floats.AddRange(vertex.GetFloatArray());
		}

		return floats.ToArray();
	}
}
