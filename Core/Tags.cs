namespace StardewUI;

/// <summary>
/// Typesafe heterogeneous container for associating arbitrary data with a view or other UI object.
/// </summary>
public class Tags
{
    /// <summary>
    /// Empty tags that can be used as a placeholder.
    /// </summary>
    public static readonly Tags Empty = new();

    /// <summary>
    /// Creates a new <see cref="Tags"/> holding a single initial value.
    /// </summary>
    /// <typeparam name="T">Type of tag value.</typeparam>
    /// <param name="value">The tag value.</param>
    public static Tags Create<T>(T value)
    {
        var tags = new Tags();
        tags.Set(value);
        return tags;
    }

    /// <summary>
    /// Creates a new <see cref="Tags"/> holding two initial values.
    /// </summary>
    /// <typeparam name="T1">Type of the first value.</typeparam>
    /// <typeparam name="T2">Type of the second value.</typeparam>
    /// <param name="value1">The first value.</param>
    /// <param name="value2">The second value.</param>
    public static Tags Create<T1, T2>(T1 value1, T2 value2)
    {
        var tags = new Tags();
        tags.Set(value1);
        tags.Set(value2);
        return tags;
    }

    /// <summary>
    /// Creates a new <see cref="Tags"/> holding three initial values.
    /// </summary>
    /// <typeparam name="T1">Type of the first value.</typeparam>
    /// <typeparam name="T2">Type of the second value.</typeparam>
    /// <typeparam name="T3">Type of the third value.</typeparam>
    /// <param name="value1">The first value.</param>
    /// <param name="value2">The second value.</param>
    /// <param name="value3">The third value.</param>
    /// <returns></returns>
    public static Tags Create<T1, T2, T3>(T1 value1, T2 value2, T3 value3)
    {
        var tags = new Tags();
        tags.Set(value1);
        tags.Set(value2);
        tags.Set(value3);
        return tags;
    }

    private readonly Dictionary<Type, object> values = [];

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return obj is Tags other && other.values.Count == values.Count && !other.values.Except(values).Any();
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        var hashCode = new HashCode();
        foreach (var entry in values.OrderBy(entry => entry.Key))
        {
            hashCode.Add(entry.Key);
            hashCode.Add(entry.Value);
        }
        return hashCode.ToHashCode();
    }

    /// <summary>
    /// Gets the tag value of the specified type, if one exists.
    /// </summary>
    /// <typeparam name="T">The value type.</typeparam>
    /// <returns>The stored value of type <typeparamref name="T"/>, if any; otherwise <c>null</c>.</returns>
    public T? Get<T>()
    {
        return values.TryGetValue(typeof(T), out var value) ? (T)value : default;
    }

    /// <summary>
    /// Replaces the tag value of the specified type.
    /// </summary>
    /// <typeparam name="T">Thee value type.</typeparam>
    /// <param name="value">The new tag value.</param>
    public void Set<T>(T value)
    {
        if (value is not null)
        {
            values[typeof(T)] = value;
        }
        else
        {
            values.Remove(typeof(T));
        }
    }
}
