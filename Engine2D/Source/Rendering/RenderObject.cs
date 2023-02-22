namespace Engine2D.Rendering;

internal class RenderObject
{
	public Sprite? Sprite;
	public Color Color = Color.White;

	public RenderTransform Transform;
	public Vertex[] Vertices;
	public uint[] Indices;
}
