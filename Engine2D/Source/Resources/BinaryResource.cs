namespace Engine2D.Resources;

public class BinaryResource : Resource
{
	public byte[] Data { get; private set; }

	protected override void OnLoad(byte[] data)
	{
		Data = data;
	}
}
