using System.Diagnostics;

namespace Engine2D;

public abstract class Component
{
	public bool ReceiveUpdates { get; private set; } = true;
	public bool ReceiveRenderUpdates { get; private set; } = true;

	public Entity Entity { get { Debug.Assert(_entity != null);  return _entity; } }

	private Entity? _entity;

    public T GetComponent<T>() where T : Component
    {
        Debug.Assert(_entity != null);
        return Entity.GetComponent<T>();
    }

    internal void Begin()
	{
		OnBegin();
	}

	internal void Update(float delta)
	{
		if (ReceiveUpdates)
		{
			OnUpdate(delta);
		}
	}

	internal void Render(float delta)
	{
		if (ReceiveRenderUpdates)
		{
			OnRender(delta);
		}
	}

	internal void End()
	{
		OnEnd();
	}

	internal void SetEntity(Entity entity)
	{
		_entity = entity;
		OnEntitySet();
	}

	protected virtual void OnEntitySet() { }

	protected virtual void OnBegin() { }

	protected virtual void OnUpdate(float delta) => ReceiveUpdates = false;
	protected virtual void OnRender(float delta) => ReceiveRenderUpdates = false;

	protected virtual void OnEnd() { }
}
