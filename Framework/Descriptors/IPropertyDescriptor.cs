namespace StardewUI.Framework.Descriptors;

/// <summary>
/// Describes a single property on a bindable object (i.e. a view).
/// </summary>
public interface IPropertyDescriptor : IMemberDescriptor
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
    /// Whether or not the property is likely auto-implemented.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Auto-property detection is heuristic, relying on the method's IL instructions and the name of its backing field.
    /// This can often be interpreted as a signal that the property won't receive property-change notifications, since
    /// to do so (whether explicitly or via some weaver/source generator) requires an implementation that is different
    /// from the auto-generated getter and setter.
    /// </para>
    /// <para>
    /// Caveats: This only works as a negative signal (a value of <c>false</c> does not prove that the property
    /// <em>will</em> receive notifications, even if the declaring type implements
    /// <see cref="System.ComponentModel.INotifyPropertyChanged"/>), and is somewhat fuzzy even as a negative signal;
    /// it is theoretically possible for a source generator or IL weaver to leave behind all the markers of an auto
    /// property and still emit notifications, although no known libraries actually do so.
    /// </para>
    /// </remarks>
    bool IsAutoProperty { get; }

    /// <summary>
    /// Whether or not the underlying member is a field, rather than a real property.
    /// </summary>
    /// <remarks>
    /// For binding convenience, fields and properties are both called "properties" for descriptors, as the external
    /// access pattern is the same; however, mutable fields can never reliably emit property-change notifications
    /// regardless of whether the declaring type implements <see cref="System.ComponentModel.INotifyPropertyChanged"/>,
    /// so this is usually used to emit some warning.
    /// </remarks>
    bool IsField { get; }

    /// <summary>
    /// The property's value type.
    /// </summary>
    Type ValueType { get; }
}

/// <summary>
/// Describes a single property on a bindable object (i.e. a view) and provides methods to read or write the value.
/// </summary>
/// <remarks>
/// The read and write methods take <see cref="object"/> as a receiver because they are intended to be invoked from a
/// non-generic context.
/// </remarks>
/// <typeparam name="T">The property type.</typeparam>
public interface IPropertyDescriptor<T> : IPropertyDescriptor
{
    /// <summary>
    /// Reads the current property value.
    /// </summary>
    /// <param name="source">An instance of the property's <see cref="IMemberDescriptor.DeclaringType"/>.</param>
    /// <returns>The current property value.</returns>
    T GetValue(object source);

    /// <summary>
    /// Writes a new property value.
    /// </summary>
    /// <param name="target">An instance of the property's <see cref="IMemberDescriptor.DeclaringType"/>.</param>
    /// <param name="value">The new property value.</param>
    void SetValue(object target, T value);
}
