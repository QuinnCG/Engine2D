namespace Engine2D.Rendering;

internal struct RenderObject
{
	public Sprite? Sprite;
	public RenderTransform Transform;
	public Color Color;

	public RenderObject()
	{
		Sprite = null;
		Transform = new();
		Color = Color.White;
	}
}
