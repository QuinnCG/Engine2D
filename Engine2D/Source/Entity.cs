using System.Diagnostics;

namespace Engine2D;

public class Entity
{
	public event Action<Entity?> OnParentChange;

	public bool ReceiveUpdates { get; private set; } = true;
    public bool ReceiveRenderUpdates { get; private set; } = true;

    private readonly Dictionary<Type, Component> _components = new();

	public Entity? Parent
	{
		get => _parent;
		set
		{
			if (_parent == value) return;
			_parent?.RemoveChild(this);
			_parent = value;
			OnParentChange?.Invoke(value);
		}
	}

	private Entity? _parent;
	private readonly List<Entity> _children = new();

	public bool HasComponent<T>() 
		where T : Component
	{
		return _components.ContainsKey(typeof(T));
	}

	public T GetComponent<T>()
		where T : Component
	{
		Debug.Assert(HasComponent<T>());
		return (T)_components[typeof(T)];
	}

	public T? TryGetComponent<T>()
		where T : Component
	{
		if (HasComponent<T>())
			return (T?)_components[typeof(T)];
		else
			return null;
	}
    public bool TryGetComponent<T>(out T? component)
    where T : Component
    {
		if (HasComponent<T>())
		{
            component = (T?)_components[typeof(T)];
			return true;
        }
		else
		{
			component = null;
			return false;
		}
    }

    public void AddComponent(Component component)
	{
		_components.Add(component.GetType(), component);
		component.SetEntity(this);
	}
	public void AddComponent(params Component[] components)
	{
		foreach (var component in components)
		{
			_components.Add(component.GetType(), component);
			component.SetEntity(this);
		}
	}
	public T AddComponent<T>() 
		where T : Component, new()
	{
		var component = new T();

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

	public void AddChild(Entity entity)
	{
		_children.Add(entity);
		entity.Parent = this;
	}

	public void RemoveChild(Entity entity)
	{
		_children.Remove(entity);
		entity.Parent = null;
	}

	internal void Begin()
	{
		foreach (var component in _components.Values)
		{
			Debug.Assert(HasRequiredComponents(component));
			component.Begin();
		}

		OnBegin();
	}

	internal void Update(float delta)
	{
		if (ReceiveUpdates)
		{
			OnUpdate(delta);

            foreach (var component in _components.Values)
            {
                component.Update(delta);
            }
        }
	}

	internal void Render(float delta)
	{
		if (ReceiveRenderUpdates)
		{
			OnRender(delta);

            foreach (var component in _components.Values)
            {
                component.Render(delta);
            }
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

	protected virtual void OnUpdate(float delta) { }
	protected virtual void OnRender(float delta) { }

	protected virtual void OnEnd() { }

	private bool HasRequiredComponents(Component component)
	{
		var attributes = component.GetType().GetCustomAttributes(true);
		foreach (var attribute in attributes)
		{
			if (attribute.GetType() != typeof(RequireComponentAttribute<>))
			{
				continue;
			}

			var args = attribute.GetType().GenericTypeArguments;
			if (args.Length > 0)
			{
				var type = args[0];
				if (!_components.ContainsKey(type)) return false;
			}
		}

		return true;
	}
}
