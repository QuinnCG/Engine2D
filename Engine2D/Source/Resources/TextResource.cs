using System.Text;

namespace Engine2D.Resources;

public class TextResource : Resource
{
	public string Text { get; private set; }

	protected override void OnLoad(byte[] data)
	{
		Text = Encoding.Default.GetString(data);
	}
}
