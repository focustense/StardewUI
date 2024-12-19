using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace StardewUI.Framework.Behaviors;

/// <summary>
/// Simple list-based implementation of <see cref="IPropertyStates{T}"/> optimized for low override counts, typically
/// fewer than 5 and never more than 10-20.
/// </summary>
/// <remarks>
/// Internally uses a <see cref="List{T}"/>, which is memory-efficient and has fast appends, but requires shifting items
/// when a state is removed from the middle of the list. This is suitable for small stacks (e.g. a pressed state on top
/// of a hover state, where the latter might be removed before the former), but if the stacks become very large, i.e.
/// having hundreds of items, then a different implementation such as linked list or linked hash set might be required.
/// Pushing a new state also requires checking for the existing state first, which is faster than hashing for very small
/// lists but, similar to removals, may be inefficient for very large ones.
/// </remarks>
/// <typeparam name="T">The property value type.</typeparam>
public class PropertyStateList<T> : IPropertyStates<T>, IReadOnlyList<KeyValuePair<string, T>>
{
    class Entry(string stateName, T initialValue)
    {
        public string StateName { get; } = stateName;
        public T Value { get; set; } = initialValue;

        public KeyValuePair<string, T> AsKeyValuePair()
        {
            return new(StateName, Value);
        }

        public bool IsNamed(string stateName)
        {
            return StateName.Equals(stateName, StringComparison.OrdinalIgnoreCase);
        }
    }

    /// <inheritdoc />
    public int Count => entries.Count;

    /// <inheritdoc />
    public KeyValuePair<string, T> this[int index] => entries[index].AsKeyValuePair();

    private readonly List<Entry> entries = [];

    /// <inheritdoc />
    public IEnumerator<KeyValuePair<string, T>> GetEnumerator()
    {
        return entries.Select(entry => entry.AsKeyValuePair()).GetEnumerator();
    }

    /// <inheritdoc />
    public void Push(string stateName, T value)
    {
        entries.RemoveAll(entry => entry.IsNamed(stateName));
        entries.Add(new(stateName, value));
    }

    /// <inheritdoc />
    public void Replace(string stateName, T value)
    {
        foreach (var entry in entries)
        {
            if (entry.IsNamed(stateName))
            {
                entry.Value = value;
                break;
            }
        }
    }

    /// <inheritdoc />
    public bool TryPeek([MaybeNullWhen(false)] out T value)
    {
        var entry = entries.Count > 0 ? entries[^1] : null;
        value = entry is not null ? entry.Value : default;
        return entry is not null;
    }

    /// <inheritdoc />
    public bool TryRemove(string stateName, [MaybeNullWhen(false)] out T value)
    {
        var index = entries.FindIndex(entry => entry.IsNamed(stateName));
        if (index >= 0)
        {
            value = entries[index].Value;
            entries.RemoveAt(index);
            return true;
        }
        value = default;
        return false;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
