using System.Diagnostics;

namespace Engine2D;

public class Entity
{
	public bool ReceiveUpdates { get; private set; } = true;

	private readonly Dictionary<Type, Component> _components = new();

	public Entity()
	{
		World.InjectionWorld?.AddEntity(this);
	}

	public bool HasComponent<T>() where T : Component
	{
		return _components.ContainsKey(typeof(T));
	}

	public T GetComponent<T>() where T : Component
	{
		return (T)_components[typeof(T)];
	}

	public void AddComponent(Component component)
	{
		Debug.Assert(HasRequiredComponents(component));

		_components.Add(component.GetType(), component);
		component.SetEntity(this);
	}
	public void AddComponent(params Component[] components)
	{
		foreach (var component in components)
		{
			Debug.Assert(HasRequiredComponents(component));

			_components.Add(component.GetType(), component);
			component.SetEntity(this);
		}
	}
	public T AddComponent<T>() where T : Component, new()
	{
		var component = new T();
		Debug.Assert(HasRequiredComponents(component));

		_components.Add(typeof(T), component);
		component.SetEntity(this);

		return component;
	}

	public void RemoveComponent(Component component)
	{
		_components.Remove(component.GetType());
	}
	public void RemoveComponent(params Component[] components)
	{
		foreach (var component in components)
		{
			_components.Remove(component.GetType());
		}
	}
	public void RemoveComponent<T>() where T : Component
	{
		_components.Remove(typeof(T));
	}

	internal void Begin()
	{
		foreach (var component in _components.Values)
		{
			component.Begin();
		}

		OnBegin();
	}

	internal void Update(float delta)
	{
		if (ReceiveUpdates)
		{
			OnUpdate(delta);
		}

		foreach (var component in _components.Values)
		{
			component.Update(delta);
		}
	}

	internal void End()
	{
		OnEnd();

		foreach (var component in _components.Values)
		{
			component.End();
		}
	}

	protected virtual void OnBegin() { }

	protected virtual void OnUpdate(float delta) => ReceiveUpdates = false;

	protected virtual void OnEnd() { }

	private bool HasRequiredComponents(Component component)
	{
		var attributes = component.GetType().GetCustomAttributes(true);

		foreach (var attribute in attributes)
		{
			var type = attribute.GetType().GenericTypeArguments[0];
			if (!_components.ContainsKey(type)) return false;
		}

		return true;
	}
}
