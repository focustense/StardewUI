namespace StardewUI.Widgets;

/// <summary>
/// Marks a child/children property as a named outlet.
/// </summary>
/// <remarks>
/// <para>
/// Outlets are used by the UI Framework, in StarML views, to differentiate between multiple child properties of the
/// same layout view. For example, the <see cref="Expander"/> defines both a <see cref="Expander.Content"/> view (the
/// "main" view) and a separate <see cref="Expander.Header"/> view, but normally only one children/content property is
/// allowed per layout view.
/// </para>
/// <para>
/// When a property is decorated with an <c>OutletAttribute</c>, it is ignored by the framework unless the markup
/// element includes an <c>*outlet</c> attribute with a value equal to the outlet <see cref="Name"/>, in which case the
/// element (or elements) will be added or assigned to that specific outlet.
/// </para>
/// <para>
/// The attribute should be omitted for whichever outlet is considered the default, i.e. to be targeted whenever the
/// markup element does not include an <c>*outlet</c> attribute.
/// </para>
/// <para>
/// Has no effect when used outside a data binding context, or when applied to any property that does not have either
/// <see cref="IView"/> or a collection of <see cref="IView"/> elements such as <see cref="IEnumerable{T}"/>.
/// </para>
/// </remarks>
/// <param name="name">The outlet name, to be matched in an <c>*outlet</c> attribute.</param>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class OutletAttribute(string name) : Attribute
{
    /// <summary>
    /// The outlet name, to be matched in an <c>*outlet</c> attribute.
    /// </summary>
    public string Name { get; } = name;
}
