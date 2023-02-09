using Engine2D.Rendering.OpenGL;
using Silk.NET.Maths;
using Silk.NET.OpenGL;

namespace Engine2D.Rendering;

internal static class Renderer
{
	private static GL _gl;

	private static GLVertexArray _vao;
	private static GLShader _shader;

	private static readonly List<RenderObject> _renderObjects = new();

	public static void Submit(RenderObject renderObject)
	{
		_renderObjects.Add(renderObject);
	}

	public static void Draw()
	{
		_gl.Clear(ClearBufferMask.ColorBufferBit);

		_vao.Bind();
		_shader.Bind();

		foreach (var renderObject in _renderObjects)
		{
			unsafe
			{
				var transform = renderObject.Transform;

				var mvp = Matrix4X4.CreateScale(transform.Scale.X, transform.Scale.Y, 1f);
				mvp *= Matrix4X4.CreateRotationZ(transform.Rotation * (180f / MathF.PI));
				mvp *= Matrix4X4.CreateTranslation(transform.Position.X, transform.Position.Y, 0f);

				mvp *= Camera.Active.GetMatrix();
				
				_gl.UniformMatrix4(_gl.GetUniformLocation(_shader.Handle, "u_mvp"), 1, false, (float*)&mvp);
				_gl.DrawElements(GLEnum.Triangles, 6, DrawElementsType.UnsignedInt, null);
			}
		}

		_renderObjects.Clear();
	}

	internal static void Initialize(Window window)
	{
		_gl = window.CreateOpenGLContext();
		window.OnResize += (width, height) =>
		{
			_gl.Viewport(0, 0, (uint)width, (uint)height);
		};

		var vbo = new GLBuffer<float>(_gl, BufferTargetARB.ArrayBuffer, new float[]
			{
				-0.5f, -0.5f, 0f, 0f,
				-0.5f,  0.5f, 0f, 1f,
				 0.5f,  0.5f, 1f, 1f,
				 0.5f, -0.5f, 1f, 0f
			});
		var ibo = new GLBuffer<uint>(_gl, BufferTargetARB.ElementArrayBuffer, new uint[]
		{
				0, 1, 2,
				3, 0, 2
		});
		_vao = new GLVertexArray(_gl, vbo, ibo);

		string vSource = """
				#version 330 core

				layout (location = 0) in vec2 a_position;
				layout (location = 1) in vec2 a_uv;

				out vec2 v_uv;

				uniform mat4 u_mvp;

				void main()
				{
					gl_Position = u_mvp * vec4(a_position, 0.0, 1.0);
					v_uv = a_uv;
				}
				""";
		string fSource = """
				#version 330 core

				in vec2 v_uv;

				out vec4 f_color;

				void main()
				{
					f_color = vec4(1.0, 0.0, 0.0, 1.0);
				}
				""";

		_shader = new GLShader(_gl, vSource, fSource);
	}

	internal static void Dispose()
	{
		_vao.Dispose();
		_shader.Dispose();
	}
}
