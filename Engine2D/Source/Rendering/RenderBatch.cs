using Engine2D.Rendering.OpenGL;
using Engine2D.Math;

namespace Engine2D.Rendering;

internal class RenderBatch : IDisposable
{
	public bool IsEmpty => _renderObjects.Count == 0;
	public int Count => _renderObjects.Count;
	public int IndexCount => _indices.Length;

	public Texture? Texture { get; }

	private readonly GLVertexArray _vao;

	private Vertex[] _vertices;
	private uint[] _indices;

	private readonly List<RenderObject> _renderObjects = new();

	public RenderBatch(Texture? texture)
	{
		Texture = texture;

		_vao = new GLVertexArray(Application.GL);
		Update();
	}

	public void Submit(params RenderObject[] renderObjects)
	{
		_renderObjects.AddRange(renderObjects);
	}

	public void Clear()
	{
		_renderObjects.Clear();
	}

	public void Update()
	{
		int numVertices = 0;
		int numIndices = 0;

		foreach (var renderObject in _renderObjects)
		{
			numVertices += renderObject.Vertices.Length;
			numIndices += renderObject.Indices.Length;
		}

		_vertices = new Vertex[numVertices];
		_indices = new uint[numIndices];

		int vertexCounter = 0;
		int indexCounter = 0;

		uint highestIndex = 0;
		uint indexOffset = 0;

		foreach (var ro in _renderObjects)
		{
			// Vertices.
			var rawVertices = new Vertex[ro.Vertices.Length];
			ro.Vertices.CopyTo(rawVertices, 0);

			for (int i = 0; i < rawVertices.Length; i++)
			{
				ref var vertex = ref rawVertices[i];
				var transform = ro.Transform;
				
				float angle = -transform.Rotation * (MathF.PI / 180f);
				float x = vertex.Position.X;
				float y = vertex.Position.Y;

				vertex.Position = new Vector2()
				{
					X = (MathF.Cos(angle) * x) - (MathF.Sin(angle) * y),
					Y = (MathF.Cos(angle) * y) + (MathF.Sin(angle) * x)
				};


				vertex.Color = ro.Color;
				vertex.Position *= transform.Scale;

				if (ro.Sprite != null)
				{
					int p = ro.Sprite.PixelsPerUnit;
					int w = ro.Sprite.Texture.Width;
					int h = ro.Sprite.Texture.Height;
					vertex.Position *= new Vector2((float)w / p, (float)h / p);

					//Vector2 size = ro.Sprite.Size;
					//Vector2Int offset = ro.Sprite.Offset;
					//vertex.UV = size * offset;
				}

				vertex.Position += transform.Position;
			}

			rawVertices.CopyTo(_vertices, vertexCounter);
			vertexCounter += ro.Vertices.Length;

			// Indices.
			var rawIndices = new uint[ro.Indices.Length];
			ro.Indices.CopyTo(rawIndices, 0);

			for (int i = 0; i < rawIndices.Length; i++)
			{
				ref uint currentIndex = ref rawIndices[i];
				currentIndex += indexOffset;

				if (currentIndex > highestIndex)
				{
					highestIndex = currentIndex;
				}

				if (i == ro.Indices.Length - 1)
				{
					indexOffset = highestIndex + 1;
				}
			}

			rawIndices.CopyTo(_indices, indexCounter);
			indexCounter += ro.Indices.Length;
		}

		_vao.Update(_vertices, _indices);
	}

	public void Dispose()
	{
		_vao.Dispose();
	}
}
