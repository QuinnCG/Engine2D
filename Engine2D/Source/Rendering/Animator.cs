namespace Engine2D.Rendering;

[RequireComponent<SpriteRenderer>]
public class Animator : Component
{
    public Animation? Animation { get; set; }

    private SpriteRenderer _spriteRenderer;
    private int _frameIndex;
    private float _nextFrameTransition;

    protected override void OnBegin()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected override void OnUpdate(float delta)
    {
        if (Animation == null)
        {
            _spriteRenderer.Sprite = null;
        }
        else if (Time.Current >= _nextFrameTransition)
        {
            if (_frameIndex + 1 > Animation.Value.Frames.Length - 1)
                _frameIndex = 0;
            else
                _frameIndex++;

            var frame = Animation.Value.Frames[_frameIndex];

            _spriteRenderer.Sprite = frame.Sprite;
            _nextFrameTransition = Time.Current + frame.Duration;
        }
    }
}
