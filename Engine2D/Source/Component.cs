using System.Diagnostics;

namespace Engine2D;

public abstract class Component
{
	public bool ReceiveUpdates { get; private set; } = true;

	protected Entity Entity;

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
		if (ReceiveUpdates)
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
		Entity = entity;
	}

	protected T GetComponent<T>() where T : Component
	{
		Debug.Assert(Entity != null);
		return Entity.GetComponent<T>();
	}

	protected virtual void OnBegin() { }

	protected virtual void OnUpdate(float delta) => ReceiveUpdates = false;
	protected virtual void OnRender(float delta) { }

	protected virtual void OnEnd() { }

}
