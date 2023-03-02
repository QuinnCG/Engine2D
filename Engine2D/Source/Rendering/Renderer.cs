using Engine2D.Rendering.OpenGL;
using Engine2D.Math;
using Silk.NET.Maths;
using Silk.NET.OpenGL;

namespace Engine2D.Rendering;

internal static class Renderer
{
	struct AABB
	{
		public Vector2 Lower, Upper;
	};

	private static GL _gl;
	private static GLVertexArray _vao;
	private static GLShader _shader;

	private static HashSet<RenderObject> _renderObjects = new();

	public static void Submit(RenderObject renderObject)
	{
		_renderObjects.Add(renderObject);
	}

	public static void Draw()
	{
#if DEBUG
		if (CameraView.Active == null)
			throw new NullReferenceException("There must be an active camera for rendering.");
#endif

		var clearColor = CameraView.Active.ClearColor;
		_gl.ClearColor(clearColor.R, clearColor.G, clearColor.B, clearColor.A);
		_gl.Clear(ClearBufferMask.ColorBufferBit);

		_vao.Bind();
		_shader.Bind();

		_renderObjects = _renderObjects.OrderBy(x => RenderLayer.IndexOf(x.Layer)).ToHashSet();

		foreach (var renderObject in _renderObjects)
		{
            if (IsRenderObjectInView(renderObject))
            {
                DrawObject(renderObject);
            }
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

		var vbo = new GLBuffer<float>(_gl, BufferTargetARB.ArrayBuffer, sizeof(float) * Vertex.FloatCount * 4);
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

    private static bool IsRenderObjectInView(RenderObject renderObject)
    {
		// TODO: Implement proper render culling.
		bool isPivotValid = false;
		if (renderObject.Sprite != null
			&& renderObject.Sprite.Pivot == Vector2.Zero)
		{
				isPivotValid = true;
		}

        if (renderObject.Transform.Rotation != 0f || !isPivotValid)
        {
            return true;
        }

        var camTrans = CameraView.Active.GetComponent<Transform>();
        var camSize = new Vector2()
        {
            X = CameraView.Active.OrthographicScale * Application.Window.Ratio,
            Y = CameraView.Active.OrthographicScale
        };

        var cameraAABB = new AABB()
        {
            Lower = (camTrans.Position - (camSize / 2f)) * camTrans.Scale,
            Upper = (camTrans.Position + (camSize / 2f)) * camTrans.Scale
        };

        var objTrans = renderObject.Transform;

		var scale = objTrans.Scale;

		if (renderObject.Sprite != null)
		{
			var res = new Vector2(renderObject.Sprite.Width, renderObject.Sprite.Height);
			scale *= res / renderObject.Sprite.PixelsPerUnit;
		}

        var objAABB = new AABB()
        {
            Lower = objTrans.Position - (scale / 2f),
            Upper = objTrans.Position + (scale / 2f)
        };

        if (cameraAABB.Lower.X > objAABB.Upper.X || cameraAABB.Upper.X < objAABB.Lower.X) return false;
        if (cameraAABB.Lower.Y > objAABB.Upper.Y || cameraAABB.Upper.Y < objAABB.Lower.Y) return false;
        return true;
    }

    private unsafe static void DrawObject(RenderObject renderObject)
	{
        renderObject.Sprite?.Texture.Bind();

        UpdateBuffers(renderObject);
        ApplyShaders(renderObject);

        _gl.DrawElements(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, null);
    }

	private static void UpdateBuffers(RenderObject renderObject)
	{
        Vector2 uvScale = Vector2.One;
        Vector2 uvOffset = Vector2.Zero;

        if (renderObject.Sprite != null)
        {
            uvScale = renderObject.Sprite.UVScale;
            uvOffset = uvScale * renderObject.Sprite.UVIndex;
        }

        _vao.Update(new Vertex[]
		{
			new Vertex(new Vector2(-0.5f, -0.5f), (new Vector2(0f, 0f) * uvScale) + uvOffset),
			new Vertex(new Vector2(-0.5f,  0.5f), (new Vector2(0f, 1f) * uvScale) + uvOffset),
			new Vertex(new Vector2( 0.5f,  0.5f), (new Vector2(1f, 1f) * uvScale) + uvOffset),
			new Vertex(new Vector2( 0.5f, -0.5f), (new Vector2(1f, 0f) * uvScale) + uvOffset)
		});
	}

	private static void ApplyShaders(RenderObject renderObject)
	{
		var transform = renderObject.Transform;

		Vector2 pivotOffset = Vector2.Zero;
		Vector2 scale = transform.Scale;

		if (renderObject.Sprite != null)
		{
			pivotOffset -= renderObject.Sprite.Pivot / 2f;

			var res = new Vector2(renderObject.Sprite.Width, renderObject.Sprite.Height);
			scale *= res / renderObject.Sprite.PixelsPerUnit;
		}

		var mvp = Matrix4X4.CreateTranslation(pivotOffset.X, pivotOffset.Y, 0f);
		mvp *= Matrix4X4.CreateScale(scale.X, scale.Y, 1f);
		mvp *= Matrix4X4.CreateRotationZ(-transform.Rotation * (MathF.PI / 180f));
		mvp *= Matrix4X4.CreateTranslation(transform.Position.X, transform.Position.Y, 0f);

		mvp *= CameraView.Active.GetMatrix();

		_shader.SetUniform("u_mvp", mvp);
		_shader.SetUniform("u_textured", renderObject.Sprite != null);
		_shader.SetUniform("u_color", renderObject.Color);
	}
}
