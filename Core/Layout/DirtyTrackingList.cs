using System.Collections;

namespace StardewUI.Layout;

/// <summary>
/// List wrapper that tracks whether changes have been made.
/// </summary>
/// <typeparam name="T">The type of elements in the list.</typeparam>
public class DirtyTrackingList<T> : IList<T>, IReadOnlyList<T>
{
    private readonly List<T> items = [];

    /// <summary>
    /// Whether changes have been made since the last call to <see cref="ResetDirty"/>.
    /// </summary>
    public bool IsDirty { get; private set; }

    /// <inheritdoc />
    public T this[int index]
    {
        get => items[index];
        set
        {
            if (!Equals(items[index], value))
            {
                items[index] = value;
                IsDirty = true;
            }
        }
    }

    /// <inheritdoc />
    public int Count => items.Count;

    /// <inheritdoc />
    public bool IsReadOnly => false;

    /// <inheritdoc />
    public void Add(T item)
    {
        items.Add(item);
        IsDirty = true;
    }

    /// <inheritdoc />
    public void Clear()
    {
        if (items.Count == 0)
        {
            return;
        }
        items.Clear();
        IsDirty = true;
    }

    /// <inheritdoc />
    public bool Contains(T item)
    {
        return items.Contains(item);
    }

    /// <inheritdoc />
    public void CopyTo(T[] array, int arrayIndex)
    {
        items.CopyTo(array, arrayIndex);
    }

    /// <inheritdoc />
    public IEnumerator<T> GetEnumerator()
    {
        return items.GetEnumerator();
    }

    /// <inheritdoc />
    public int IndexOf(T item)
    {
        return items.IndexOf(item);
    }

    /// <inheritdoc />
    public void Insert(int index, T item)
    {
        items.Insert(index, item);
        IsDirty = true;
    }

    /// <inheritdoc />
    public bool Remove(T item)
    {
        var wasRemoved = items.Remove(item);
        if (wasRemoved)
        {
            IsDirty = true;
        }
        return wasRemoved;
    }

    /// <inheritdoc />
    public void RemoveAt(int index)
    {
        items.RemoveAt(index);
        IsDirty = true;
    }

    /// <summary>
    /// Resets the dirty state; <see cref="IsDirty"/> will return <c>false</c> until another mutation occurs.
    /// </summary>
    public void ResetDirty()
    {
        IsDirty = false;
    }

    /// <summary>
    /// Replaces the entire list with the specified sequence.
    /// </summary>
    /// <param name="items">The new list items.</param>
    public bool SetItems(IEnumerable<T> items)
    {
        if (items.SequenceEqual(this.items))
        {
            return false;
        }
        this.items.Clear();
        this.items.AddRange(items);
        IsDirty = true;
        return true;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
