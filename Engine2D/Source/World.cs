namespace Engine2D;

public class World
{
	internal static HashSet<World> ActiveWorlds = new();

	private readonly HashSet<Entity> _entities = new();
	private readonly bool _hasBegun = false;

	public void AddEntity(params Entity[] entities)
	{
		if (_hasBegun)
		{
			foreach (Entity entity in entities)
			{
				_entities.Add(entity);
				entity.Begin();
			}
		}
		else
		{
			foreach (Entity entity in entities)
			{
				_entities.Add(entity);
			}
		}
	}

	public void RemoveEntity(params Entity[] entities)
	{
		if (_hasBegun)
		{
			foreach (Entity entity in entities)
			{
				entity.End();
				_entities.Remove(entity);
			}
		}
		else
		{
			foreach (Entity entity in entities)
			{
				_entities.Remove(entity);
			}
		}
	}

	public void MoveEntity(Entity entity, World newWorld)
	{
		RemoveEntity(entity);
		newWorld.AddEntity(entity);
	}

	public void Activate(World world)
	{
		ActiveWorlds.Add(world);
	}

	public void Deactivate(World world)
	{
		ActiveWorlds.Remove(world);
	}

	public void Load()
	{
		ActiveWorlds.Add(this);
		foreach (Entity entity in _entities)
		{
			entity.Begin();
		}
	}

	public void Unload()
	{
		ActiveWorlds.Remove(this);

		foreach (Entity entity in _entities)
		{
			entity.End();
		}

		_entities.Clear();
	}

	internal void Update(float delta)
	{
		foreach (var entity in _entities)
		{
			entity.Update(delta);
		}
	}
}
