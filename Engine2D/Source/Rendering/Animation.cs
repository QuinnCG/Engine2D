using Engine2D.Math;

namespace Engine2D.Rendering;

public struct Animation
{
    public AnimationFrame[] Frames;

    public Animation(params AnimationFrame[] frames)
    {
        Frames = frames;
    }
    public Animation(float duration, params Sprite[] sprites)
    {
        Frames = CreateFrames(duration, sprites);
    }
    public Animation(float duration, Texture texture, Vector2 uvScale, Vector2 pivot)
    {
        var sprites = Sprite.CreateSpriteSheet(texture, uvScale, pivot);
        Frames = CreateFrames(duration, sprites);
    }
    public Animation(float duration, Texture texture, Vector2 uvScale, Vector2 pivot, int pixelsPerUnit)
    {
        var sprites = Sprite.CreateSpriteSheet(texture, uvScale, pixelsPerUnit, pivot);
        Frames = CreateFrames(duration, sprites);
    }

    private AnimationFrame[] CreateFrames(float duration, Sprite[] sprites)
    {
        Frames = new AnimationFrame[sprites.Length];
        for (int i = 0; i < sprites.Length; i++)
        {
            Frames[i] = new AnimationFrame(sprites[i], duration);
        }

        return Frames;
    }
}
