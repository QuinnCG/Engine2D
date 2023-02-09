using Silk.NET.OpenGL;

namespace Engine2D.Rendering.OpenGL;

public sealed class GLShader : IDisposable
{
	public uint Handle { get; }

	private readonly GL _gl;

	public GLShader(GL gl, in string vertexSoure, in string fragmentSource)
	{
		_gl = gl;

		var vs = CreateShader(ShaderType.VertexShader, vertexSoure);
		var fs = CreateShader(ShaderType.FragmentShader, fragmentSource);

		Handle = _gl.CreateProgram();
		_gl.AttachShader(Handle, vs);
		_gl.AttachShader(Handle, fs);
		_gl.LinkProgram(Handle);

		_gl.DeleteShader(vs);
		_gl.DeleteShader(fs);
	}

	public void Bind()
	{
		_gl.UseProgram(Handle);
	}

	public void SetUniform(string name, float value)
	{
		_gl.Uniform1(GetUniformLocation(name), value);
	}
	public void SetUniform(string name, int value)
	{
		_gl.Uniform1(GetUniformLocation(name), value);
	}

	public void Dispose()
	{
		_gl.DeleteProgram(Handle);
	}

	private uint CreateShader(ShaderType type, in string source)
	{
		var shader = _gl.CreateShader(type);
		_gl.ShaderSource(shader, source);
		_gl.CompileShader(shader);

		return shader;
	}

	private int GetUniformLocation(string name)
	{
		return _gl.GetUniformLocation(Handle, name);
	}
}
