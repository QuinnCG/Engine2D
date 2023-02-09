namespace Engine2D;

public class World
{
	internal static World? InjectionWorld { get; set; }

	internal static HashSet<World> LoadedWorlds = new();

	private readonly HashSet<Entity> _entities = new();
	private readonly bool _hasBegun = false;

	public World()
	{
		InjectionWorld ??= this;
	}

	public static void SetInjectionWorld(World world)
	{
		InjectionWorld = world;
	}

	public void MoveEntity(Entity entity, World newWorld)
	{
		RemoveEntity(entity);
		newWorld.AddEntity(entity);
	}

	public void Load()
	{
		LoadedWorlds.Add(this);

		foreach (Entity entity in _entities)
		{
			entity.Begin();
		}
	}

	public void Unload()
	{
		LoadedWorlds.Remove(this);

		foreach (Entity entity in _entities)
		{
			entity.End();
		}

		_entities.Clear();
	}

	internal void AddEntity(params Entity[] entities)
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

	internal void RemoveEntity(params Entity[] entities)
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

	internal void Update(float delta)
	{
		foreach (var entity in _entities)
		{
			entity.Update(delta);
		}
	}
}
