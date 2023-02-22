namespace Engine2D;

public class World
{
	public static World DefaultWorld
	{
		get
		{
			if (_defaultWorld == null)
				throw new Exception("Default world is set to null, but something is trying to access it.");
			return _defaultWorld;
		}
		set => _defaultWorld = value;
	}

	internal static HashSet<World> ActiveWorlds = new();

	private static World? _defaultWorld;

	private readonly HashSet<Entity> _entities = new();
	private readonly HashSet<Entity> _entitiesToAdd = new();
	private readonly HashSet<Entity> _entitiesToRemove = new();

	private bool _hasBegun = false;

	public World()
	{
		_defaultWorld ??= this;
	}

	public void AddEntity(params Entity[] entities)
	{
		foreach (var entity in entities)
		{
			_entitiesToAdd.Add(entity);
		}
	}

	public void RemoveEntity(params Entity[] entities)
	{
		foreach (var entity in entities)
		{
			_entitiesToRemove.Add(entity);
		}
	}

	public void MoveEntity(Entity entity, World newWorld)
	{
		RemoveEntity(entity);
		newWorld.AddEntity(entity);
	}

	public void Load()
	{
		Activate();
		_hasBegun = true;

		foreach (Entity entity in _entities)
		{
			entity.Begin();
		}
	}

	public void Unload()
	{
		Deactivate();
		_hasBegun = false;

		foreach (Entity entity in _entities)
		{
			entity.End();
		}

		_entities.Clear();
	}

	public void Activate()
	{
		ActiveWorlds.Add(this);
	}

	public void Deactivate()
	{
		ActiveWorlds.Remove(this);
	}

	internal void Update(float delta)
	{
		foreach (var entity in _entitiesToAdd)
		{
			_entities.Add(entity);
			if (_hasBegun)
			{
				entity.Begin();
			}
		}
		_entitiesToAdd.Clear();

		foreach (var entity in _entitiesToRemove)
		{
			if (_hasBegun)
			{
				entity.End();
			}
			_entities.Remove(entity);
		}
		_entitiesToRemove.Clear();

		foreach (var entity in _entities)
		{
			entity.Update(delta);
		}
	}

	internal void Render(float delta)
	{
		foreach (var entity in _entities)
		{
			entity.Render(delta);
		}
	}
}
