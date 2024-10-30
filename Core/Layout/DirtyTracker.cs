namespace StardewUI.Layout;

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
        set => SetIfChanged(value);
    }

    /// <summary>
    /// Resets the dirty flag, so that <see cref="IsDirty"/> returns <c>false</c> until the <see cref="Value"/> is
    /// changed again.
    /// </summary>
    public void ResetDirty()
    {
        IsDirty = false;
    }

    /// <summary>
    /// Updates the tracker with a new value, if it has changed since the last seen value.
    /// </summary>
    /// <remarks>
    /// If this method returns <c>true</c>, then <see cref="IsDirty"/> will always also be <c>true</c> afterward.
    /// However, if it returns <c>false</c> then the dirty state simply remains unchanged, and will only be <c>false</c>
    /// if the value was not already dirty from a previous change.
    /// </remarks>
    /// <param name="value">The new value.</param>
    /// <returns><c>true</c> if the <paramref name="value"/> was different from the previous <see cref="Value"/>,
    /// otherwise <c>false</c>.</returns>
    public bool SetIfChanged(T value)
    {
        if (!Equals(value, this.value))
        {
            this.value = value;
            IsDirty = true;
            return true;
        }
        return false;
    }
}
