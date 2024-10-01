namespace StardewUI.Framework.Sources;

/// <summary>
/// Holds the type-independent data of an <see cref="IValueSource{T}"/>.
/// </summary>
/// <remarks>
/// Instances of this type should always implement <see cref="IValueSource{T}"/> as well; the non-generic version is
/// used when the type is unknown at compile time.
/// </remarks>
public interface IValueSource
{
    /// <summary>
    /// Whether or not the source can be read from, i.e. if an attempt to <b>get</b> the <see cref="Value"/> should
    /// succeed.
    /// </summary>
    bool CanRead { get; }

    /// <summary>
    /// Whether or not the source can be written back to, i.e. if an attempt to <b>set</b> the <see cref="Value"/>
    /// should succeed.
    /// </summary>
    bool CanWrite { get; }

    /// <summary>
    /// Descriptive name for the property, used primarily for debug views and log/exception messages.
    /// </summary>
    string DisplayName { get; }

    /// <summary>
    /// The compile-time type of the value tracked by this source; the type parameter for <see cref="IValueSource{T}"/>.
    /// </summary>
    Type ValueType { get; }

    /// <summary>
    /// Checks if the value needs updating, and if so, updates <see cref="Value"/> to the latest.
    /// </summary>
    /// <remarks>
    /// This method is called every frame, for every binding, and providing a correct return value is essential in order
    /// to avoid slowdowns due to unnecessary rebinds.
    /// </remarks>
    /// <returns><c>true</c> if the <see cref="Value"/> was updated; <c>false</c> if it already held the most recent
    /// value.</returns>
    bool Update();
}

/// <summary>
/// Abstract representation of the source of some value bound to a view's <see cref="IPropertyDescriptor"/>.
/// </summary>
/// <typeparam name="T">Type of value supplied.</typeparam>
public interface IValueSource<T> : IValueSource
{
    /// <summary>
    /// Gets the current value obtained as of the last <see cref="Update"/>, or writes a new value when set.
    /// </summary>
    T? Value { get; set; }
}
