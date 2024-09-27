namespace StardewUI.Framework.Sources;

/// <summary>
/// Abstract representation of the source of some value bound to a view's <see cref="IPropertyDescriptor"/>.
/// </summary>
/// <typeparam name="T">Type of value supplied.</typeparam>
public interface IValueSource<T>
{
    /// <summary>
    /// The value obtained as of the last <see cref="Update"/>.
    /// </summary>
    T? Value { get; }

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
