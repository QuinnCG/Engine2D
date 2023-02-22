using Engine2D.Rendering.OpenGL;
using Silk.NET.Maths;
using Silk.NET.OpenGL;

namespace Engine2D.Rendering;

internal static class Renderer
{
	private static GL _gl;
	private static GLVertexArray _vao;
	private static GLShader _shader;

	private static readonly HashSet<RenderObject> _renderObjects = new();

	public static void Submit(RenderObject renderObject)
	{
		_renderObjects.Add(renderObject);
	}

	public unsafe static void Draw()
	{
#if DEBUG
		if (CameraView.Active == null)
		{
			throw new NullReferenceException("There must be an active camera for rendering.");
		}
#endif

		_gl.Clear(ClearBufferMask.ColorBufferBit);

		_vao.Bind();
		_shader.Bind();

		foreach (var renderObject in _renderObjects)
		{
			renderObject.Sprite?.Texture.Bind();
			var transform = renderObject.Transform;

			var mvp = Matrix4X4.CreateScale(transform.Scale.X, transform.Scale.Y, 1f);
			mvp *= Matrix4X4.CreateRotationZ(-transform.Rotation * (MathF.PI / 180f));
			mvp *= Matrix4X4.CreateTranslation(transform.Position.X, transform.Position.Y, 0f);
			mvp *= CameraView.Active.GetMatrix();

			_shader.SetUniform("u_mvp", mvp);
			_shader.SetUniform("u_textured", renderObject.Sprite != null);
			_shader.SetUniform("u_color", renderObject.Color);

			_gl.DrawElements(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, null);
		}

		_renderObjects.Clear();
	}

	public static void Initialize(Window window)
	{
		_gl = Application.GL;
		_gl.FrontFace(FrontFaceDirection.CW);

		_gl.Enable(EnableCap.Blend);
		_gl.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

		window.OnResize += (width, height) =>
		{
			_gl.Viewport(0, 0, (uint)width, (uint)height);
		};

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

				uniform sampler2D u_texture;
				uniform vec4 u_color;
				uniform bool u_textured;

				void main()
				{
					if (u_textured)
					{
						f_color = texture(u_texture, v_uv) * u_color;
					}
					else
					{
						f_color = u_color;
					}
				}
				""";
		_shader = new GLShader(_gl, vSource, fSource);

		var vbo = new GLBuffer<float>(_gl, BufferTargetARB.ArrayBuffer, new float[]
		{
			-0.5f, -0.5f,	0f, 0f,
			-0.5f,  0.5f,   0f, 1f,
			 0.5f,  0.5f,   1f, 1f,
			 0.5f, -0.5f,   1f, 0f
		});
		var ibo = new GLBuffer<uint>(_gl, BufferTargetARB.ElementArrayBuffer, new uint[]
		{
			0, 1, 2,
			3, 0, 2
		});
		_vao = new GLVertexArray(_gl, vbo, ibo);
	}

	public static void Dispose()
	{
		_shader.Dispose();
		_vao.Dispose();
	}
}
