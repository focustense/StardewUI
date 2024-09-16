namespace StardewUI;

/// <summary>
/// Convenience class for tracking properties that have changed, i.e. for layout dirty checking.
/// </summary>
/// <remarks>
/// Will not flag changes as dirty unless the changed value is different from previous. Requires a correct
/// <see cref="object.Equals"/> implementation for this to work, typically meaning strings, primitives and records.
/// </remarks>
/// <typeparam name="T">Type of value held by the tracker.</typeparam>
/// <param name="initialValue">Value to initialize with.</param>
public sealed class DirtyTracker<T>(T initialValue)
{
    private T value = initialValue;

    /// <summary>
    /// Whether or not the value is dirty, i.e. has changed since the last call to <see cref="ResetDirty"/>.
    /// </summary>
    public bool IsDirty { get; private set; }

    /// <summary>
    /// The currently-held value.
    /// </summary>
    public T Value
    {
        get => value;
        set
        {
            if (!Equals(value, this.value))
            {
                this.value = value;
                IsDirty = true;
            }
        }
    }

    /// <summary>
    /// Resets the dirty flag, so that <see cref="IsDirty"/> returns <c>false</c> until the <see cref="Value"/> is
    /// changed again.
    /// </summary>
    public void ResetDirty()
    {
        IsDirty = false;
    }
}
