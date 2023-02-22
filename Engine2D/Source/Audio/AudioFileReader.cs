using System.Text;

namespace Engine2D.Audio;

internal static class WaveFileReader
{
	public static byte[] ReadWaveFile(in byte[] data, out int channels, out int sampleRate, out int bitsPerSample)
	{
		using var ms = new MemoryStream(data);

		try
		{
			var buffer = new byte[4];
			ms.ReadExactly(buffer);

			if (Encoding.ASCII.GetString(buffer) != "RIFF")
				throw new Exception();

			ms.Position = 22;
			buffer = new byte[2];
			ms.ReadExactly(buffer);
			channels = BitConverter.ToInt16(buffer);

			buffer = new byte[4];
			ms.ReadExactly(buffer);
			sampleRate = BitConverter.ToInt32(buffer);

			ms.Position = 34;
			buffer = new byte[2];
			bitsPerSample = BitConverter.ToInt32(buffer);

			ms.Position = 44;
			buffer = new byte[ms.Length - ms.Position];
			ms.ReadExactly(buffer);

			return buffer;
		}
		catch
		{
			throw new InvalidOperationException("Failed to load wave file.");
		}
	}

	public static float CalculateAudioLength(int samples, int bitsPerSample, int channels, int sampleRate)
	{
		return samples / (bitsPerSample / 8 * channels) / sampleRate;
	}
}
