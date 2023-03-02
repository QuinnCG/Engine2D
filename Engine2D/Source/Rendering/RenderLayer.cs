using System.Diagnostics;

namespace Engine2D.Rendering;

public class RenderLayer
{
    public static RenderLayer Default { get; } = new();
    private readonly static List<RenderLayer> _layers = new();

    static RenderLayer()
    {
        _layers.Add(Default);
    }

    public static bool Exists(RenderLayer layer)
    {
        return _layers.Contains(layer);
    }

    public static void Add(RenderLayer layer)
    {
        Debug.Assert(!Exists(layer));
        _layers.Add(layer);
    }

    public static void AddBefore(RenderLayer layer, RenderLayer reference)
    {
        Debug.Assert(!Exists(layer));
        int index = IndexOf(reference);
        _layers.Insert(index, layer);
    }

    public static void AddAfter(RenderLayer layer, RenderLayer reference)
    {
        Debug.Assert(!Exists(layer));
        int index = IndexOf(reference);

        if (index == _layers.Count - 1)
        {
            _layers.Add(layer);
        }
        else
        {
            _layers.Insert(index + 1, layer);
        }
    }

    public static void Remove(RenderLayer layer)
    {
        Debug.Assert(layer != Default, "Cannot remove the Default render layer from the stack.");
        _layers.Remove(layer);
    }

    public static int IndexOf(RenderLayer layer)
    {
        return _layers.IndexOf(layer);
    }

    public static void Insert(RenderLayer layer, int index)
    {
        _layers.Insert(index, layer);
    }
}
