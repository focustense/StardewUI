using System.Runtime.CompilerServices;

namespace StardewUI.Framework.Converters;

/// <summary>
/// String converter for the <see cref="Edges"/> type.
/// </summary>
public class EdgesConverter : IValueConverter<string, Edges>
{
    public Edges Convert(string value)
    {
        var valueAsSpan = value.AsSpan();
        // We use generic uninformative names for these variables because they mean different things depending on how
        // many of them appear in the string.
        int edge1 = ReadNextEdge(ref valueAsSpan);
        if (valueAsSpan.IsEmpty)
        {
            return new(edge1); // Same length for all edges
        }
        int edge2 = ReadNextEdge(ref valueAsSpan);
        if (valueAsSpan.IsEmpty)
        {
            return new(edge1, edge2); // Horizontal and vertical lengths
        }
        int edge3 = ReadNextEdge(ref valueAsSpan);
        int edge4 = ReadNextEdge(ref valueAsSpan);
        if (!valueAsSpan.IsEmpty)
        {
            throw new FormatException($"Too many edges specified in string '{value}' (cannot have more than 4).");
        }
        return new(edge1, edge2, edge3, edge4);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int ReadNextEdge(ref ReadOnlySpan<char> remaining)
    {
        int nextSeparatorIndex = remaining.IndexOf(',');
        var value = nextSeparatorIndex >= 0 ? int.Parse(remaining[0..nextSeparatorIndex]) : int.Parse(remaining);
        remaining = nextSeparatorIndex >= 0 ? remaining[(nextSeparatorIndex + 1)..] : [];
        return value;
    }
}
