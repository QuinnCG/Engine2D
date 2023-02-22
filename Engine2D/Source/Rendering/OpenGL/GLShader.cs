using Silk.NET.Maths;
using Silk.NET.OpenGL;

namespace Engine2D.Rendering.OpenGL;

internal sealed class GLShader : IDisposable
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

		_gl.GetProgramInfoLog(Handle, out string info);
		if (!string.IsNullOrWhiteSpace(info))
		{
			Console.Write("[Shader linking error]: ");
			Console.WriteLine(info);
		}

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
	public void SetUniform(string name, bool value)
	{
		_gl.Uniform1(GetUniformLocation(name), value ? 1 : 0);
	}
	public void SetUniform(string name, float x, float y, float z)
	{
		_gl.Uniform3(GetUniformLocation(name), x, y, z);
	}
	public void SetUniform(string name, float x, float y, float z, float w)
	{
		_gl.Uniform4(GetUniformLocation(name), x, y, z, w);
	}
	public void SetUniform(string name, Color color)
	{
		_gl.Uniform4(GetUniformLocation(name), color.R, color.G, color.B, color.A);
	}
	public unsafe void SetUniform(string name, Matrix4X4<float> matrix)
	{
		_gl.UniformMatrix4(_gl.GetUniformLocation(Handle, name), 1, false, (float*)&matrix);
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

		_gl.GetShaderInfoLog(shader, out string info);
		if (!string.IsNullOrWhiteSpace(info))
		{
			Console.Write($"[{type} shader compilation error]: ");
			Console.WriteLine(info);
		}

		return shader;
	}

	private int GetUniformLocation(string name)
	{
		return _gl.GetUniformLocation(Handle, name);
	}
}
