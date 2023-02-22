using Engine2D.Rendering.OpenGL;
using Silk.NET.OpenGL;

namespace Engine2D.Rendering;

internal static class Renderer
{
	private static GL _gl;
	private static GLShader _shader;

	private static readonly List<RenderBatch> _renderBatches = new();

	public static void Submit(RenderBatch renderBatch)
	{
		_renderBatches.Add(renderBatch);
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
		_shader.Bind();

		foreach (var batch in _renderBatches)
		{
			batch.Texture?.Bind();
			batch.Update();

			_shader.SetUniform("u_mvp", CameraView.Active.GetMatrix());
			_shader.SetUniform("u_textured", batch.Texture != null);
			_gl.DrawElements(PrimitiveType.Triangles, (uint)batch.IndexCount, DrawElementsType.UnsignedInt, null);
		}

		_renderBatches.Clear();
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
				layout (location = 2) in vec4 a_color;

				out vec2 v_uv;
				out vec4 v_color;

				uniform mat4 u_mvp;

				void main()
				{
					gl_Position = u_mvp * vec4(a_position, 0.0, 1.0);
					v_uv = a_uv;
					v_color = a_color;
				}
				""";
		string fSource = """
				#version 330 core

				in vec2 v_uv;
				in vec4 v_color;

				out vec4 f_color;

				uniform bool u_textured;
				uniform sampler2D u_texture;

				void main()
				{
					if (u_textured)
					{
						f_color = texture(u_texture, v_uv) * v_color;
					}
					else
					{
						f_color = v_color;
					}
				}
				""";

		_shader = new GLShader(_gl, vSource, fSource);
	}

	public static void Dispose()
	{
		_shader.Dispose();
	}
}
