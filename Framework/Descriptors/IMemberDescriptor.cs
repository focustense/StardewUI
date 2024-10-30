namespace StardewUI.Framework.Descriptors;

/// <summary>
/// Describes a single member (property, method, or event) of a bindable object, such as a view.
/// </summary>
public interface IMemberDescriptor
{
    /// <summary>
    /// The type on which the member is declared.
    /// </summary>
    Type DeclaringType { get; }

    /// <summary>
    /// The member name.
    /// </summary>
    string Name { get; }
}
