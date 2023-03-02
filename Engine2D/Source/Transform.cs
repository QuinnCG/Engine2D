using Engine2D.Math;

namespace Engine2D;

public class Transform : Component
{
	public Vector2 Position
	{
		get
		{
			if (_parentTransform == null || WorldSpacePosition)
				return _position;
			else
				return _position + _parentTransform.Position;
		}

		set => _position = value;
	}
	public float Rotation
	{
		get
		{
			if (_parentTransform == null || WorldSpaceRotation)
				return _rotation;
			else
				return _rotation - _parentTransform.Rotation;
		}

		set => _rotation = value;
	}
	public Vector2 Scale
	{
		get
		{
			if (_parentTransform == null || WorldSpaceScale)
				return _scale;
			else
				return _scale * _parentTransform.Scale;
		}

		set => _scale = value;
	}

	public bool WorldSpacePosition { get; set; } = false;
	public bool WorldSpaceRotation { get; set; } = false;
	public bool WorldSpaceScale { get; set; } = false;

	public bool WorldSpace
	{
		get => WorldSpacePosition && WorldSpaceRotation && WorldSpaceScale;
		set => WorldSpacePosition = WorldSpaceRotation = WorldSpaceScale = value;
	}

	private Vector2 _position;
	private float _rotation;
	private Vector2 _scale = Vector2.One;

	private Transform? _parentTransform;

	public Transform() { }
	public Transform(bool worldSpace = false)
	{
		WorldSpace = worldSpace;
	}
	public Transform(bool worldspacePosition = false, bool worldSpaceRotation = false, bool worldSpaceScale = false)
	{
		WorldSpacePosition = worldspacePosition;
		WorldSpaceRotation = worldSpaceRotation;
		WorldSpaceScale = worldSpaceScale;
	}

    protected override void OnEntitySet()
    {
        Entity.OnParentChange += entity =>
        {
            if (entity != null)
                _parentTransform = entity.GetComponent<Transform>();
            else
                _parentTransform = null;
        };
    }
}
