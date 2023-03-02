using System.Runtime.InteropServices;
using StbiSharp;

namespace Engine2D.Resources;

public unsafe class TextureResource : Resource
{
	public byte[] Pixels { get; private set; }

	public int Width { get; private set; }
	public int Height { get; private set; }
	public int Channels { get; private set; }

	protected override void OnLoad(byte[] data)
	{
		fixed (byte* bytes = &data[0])
		{
			Stbi.SetFlipVerticallyOnLoad(true);
			byte* import = Stbi.LoadFromMemory(bytes, data.Length, out int width, out int height, out int channels, 0);
			var pixels = new ReadOnlySpan<byte>(import, width * height * channels);

			Pixels = new byte[width * height * channels];
			Marshal.Copy((nint)import, Pixels, 0, Pixels.Length);

			Width = width;
			Height = height;
			Channels = channels;
		}
	}
}
