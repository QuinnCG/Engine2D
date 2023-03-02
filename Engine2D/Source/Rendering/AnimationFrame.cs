namespace Engine2D.Rendering;

public struct AnimationFrame
{
    public Sprite Sprite;
    public float Duration;

    public AnimationFrame(Sprite sprite, float duration)
    {
        Sprite = sprite;
        Duration = duration;
    }
}
