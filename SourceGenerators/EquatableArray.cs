// Derived from: https://github.com/andrewlock/StronglyTypedId/blob/master/src/StronglyTypedIds/EquatableArray.cs

using System.Collections;
using System.Collections.Immutable;

namespace StardewUI.SourceGenerators;

/// <summary>
/// An immutable, equatable array.
/// </summary>
/// <remarks>
/// This is equivalent to <see cref="ImmutableArray{T}"/> but with sequence equality support.
/// </remarks>
/// <typeparam name="T">The type of values in the array.</typeparam>
/// <param name="array">The input <see cref="ImmutableArray"/> to wrap.</param>
internal readonly struct EquatableArray<T>(ImmutableArray<T> array) : IEquatable<EquatableArray<T>>, IEnumerable<T>
    where T : IEquatable<T>
{
    public static readonly EquatableArray<T> Empty = new([]);

    /// <summary>
    /// Gets the element at a specified index.
    /// </summary>
    /// <param name="index">The array index.</param>
    /// <returns>The element at the specified <paramref name="index"/>.</returns>
    public T this[int index]
    {
        get => array[index];
    }

    /// <sinheritdoc/>
    public bool Equals(EquatableArray<T> array)
    {
        return AsSpan().SequenceEqual(array.AsSpan());
    }

    /// <sinheritdoc/>
    public override bool Equals(object? obj)
    {
        return obj is EquatableArray<T> array && Equals(array);
    }

    /// <sinheritdoc/>
    public override int GetHashCode()
    {
        HashCode hashCode = default;
        foreach (T item in array)
        {
            hashCode.Add(item);
        }
        return hashCode.ToHashCode();
    }

    /// <summary>
    /// Returns a <see cref="ReadOnlySpan{T}"/> wrapping the current items.
    /// </summary>
    /// <returns>A <see cref="ReadOnlySpan{T}"/> wrapping the current items.</returns>
    public ReadOnlySpan<T> AsSpan()
    {
        return array.AsSpan();
    }

    /// <summary>
    /// Gets the underlying array if there is one
    /// </summary>
    public ImmutableArray<T> GetArray() => array;

    /// <sinheritdoc/>
    IEnumerator<T> IEnumerable<T>.GetEnumerator()
    {
        return array.AsEnumerable().GetEnumerator();
    }

    /// <sinheritdoc/>
    IEnumerator IEnumerable.GetEnumerator()
    {
        return array.AsEnumerable().GetEnumerator();
    }

    public int Count => array.Length;

    /// <summary>
    /// Checks whether two <see cref="EquatableArray{T}"/> values are the same.
    /// </summary>
    /// <param name="left">The first <see cref="EquatableArray{T}"/> value.</param>
    /// <param name="right">The second <see cref="EquatableArray{T}"/> value.</param>
    /// <returns>Whether <paramref name="left"/> and <paramref name="right"/> are equal.</returns>
    public static bool operator ==(EquatableArray<T> left, EquatableArray<T> right)
    {
        return left.Equals(right);
    }

    /// <summary>
    /// Checks whether two <see cref="EquatableArray{T}"/> values are not the same.
    /// </summary>
    /// <param name="left">The first <see cref="EquatableArray{T}"/> value.</param>
    /// <param name="right">The second <see cref="EquatableArray{T}"/> value.</param>
    /// <returns>Whether <paramref name="left"/> and <paramref name="right"/> are not equal.</returns>
    public static bool operator !=(EquatableArray<T> left, EquatableArray<T> right)
    {
        return !left.Equals(right);
    }
}

internal static class EquatableArrayExtensions
{
    public static EquatableArray<T> ToEquatable<T>(this ImmutableArray<T>.Builder builder)
        where T : IEquatable<T>
    {
        return new(builder.ToImmutable());
    }
}
