namespace Engine2D.Resources;

public abstract class Resource
{
	protected Resource() { }

	public static T Load<T>(string name) where T : Resource, new()
	{
		var resource = new T();
		resource.OnLoad(GetData(name));
		return resource;
	}

	private static byte[] GetData(string name)
	{
		return File.ReadAllBytes($"Resources\\{name}");
	}

    protected abstract void OnLoad(byte[] data);
}
