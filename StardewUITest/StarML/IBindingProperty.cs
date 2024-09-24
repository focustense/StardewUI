namespace StardewUITest.StarML;

/// <summary>
/// Describes a single property on a bindable object (i.e. a view).
/// </summary>
public interface IBindingProperty
{
    /// <summary>
    /// Whether or not the property is readable, i.e. has a public getter.
    /// </summary>
    bool CanRead { get; }

    /// <summary>
    /// Whether or not the property is writable, i.e. has a public setter.
    /// </summary>
    bool CanWrite { get; }

    /// <summary>
    /// The type on which the property is declared.
    /// </summary>
    Type DeclaringType { get; }

    /// <summary>
    /// The property name.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// The property's value type.
    /// </summary>
    Type ValueType { get; }
}

/// <summary>
/// Describes a single property on a bindable object (i.e. a view) and provides methods to read or write the value.
/// </summary>
/// <remarks>
/// The read and write methods take <see cref="object"/> as a receiver, as they are intended to be invoked from a
/// non-generic context; however, they are mostly intended to be used on <see cref="StardewUI.IView"/>.
/// </remarks>
/// <typeparam name="T">The property type.</typeparam>
public interface IBindingProperty<T> : IBindingProperty
{
    /// <summary>
    /// Reads the current property value.
    /// </summary>
    /// <param name="source">An instance of the property's <see cref="IBindingProperty.DeclaringType"/>.</param>
    /// <returns>The current property value.</returns>
    T GetValue(object source);

    /// <summary>
    /// Writes a new property value.
    /// </summary>
    /// <param name="target">An instance of the property's <see cref="IBindingProperty.DeclaringType"/>.</param>
    /// <param name="value">The new property value.</param>
    void SetValue(object target, T value);
}