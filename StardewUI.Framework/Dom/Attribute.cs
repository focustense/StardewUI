using StardewUI.Framework.Grammar;

namespace StardewUI.Framework.Dom;

/// <summary>
/// Attribute of a StarML element.
/// </summary>
public interface IAttribute
{
    /// <summary>
    /// The attribute name.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// The literal value text.
    /// </summary>
    string Value { get; }

    /// <summary>
    /// The type of the value expression, defining how the <paramref name="Value"/> should be interpreted.
    /// </summary>
    AttributeValueType ValueType { get; }
}

/// <summary>
/// Extension methods for the <see cref="IAttribute"/> interface.
/// </summary>
public static class AttributeExtensions
{
    /// <summary>
    /// Gets the binding path (property name or asset path) of an attribute.
    /// </summary>
    /// <param name="attribute">The attribute to resolve.</param>
    /// <returns>The attribute's binding path; either the asset name, for an asset binding, or the property name, for a
    /// context binding.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="attribute"/> does not have a bound value, i.e.
    /// has a <see cref="AttributeValueType.Literal"/> type instead.</exception>
    public static string GetBindingPath(this IAttribute attribute)
    {
        if (attribute.ValueType != AttributeValueType.Binding)
        {
            throw new ArgumentException("Specified attribute does not have a bound value type.", nameof(attribute));
        }
        return IsAssetBinding(attribute) ? attribute.Value[1..] : attribute.Value;
    }

    /// <summary>
    /// Tests whether an attribute is an asset binding.
    /// </summary>
    /// <param name="attribute">The attribute to check.</param>
    /// <returns><c>true</c> if the <paramref name="attribute"/> value references a game asset; <c>false</c> if it is a
    /// context binding or literal attribute.</returns>
    public static bool IsAssetBinding(this IAttribute attribute)
    {
        return attribute.ValueType == AttributeValueType.Binding && attribute.Value.StartsWith('@');
    }
}

/// <summary>
/// Record implementation of a StarML <see cref="IAttribute"/>.
/// </summary>
/// <remarks>
/// Must be separate from the grammar's <see cref="Attribute"/> since <c>ref struct</c>s currently are not allowed to
/// implement interfaces.
/// </remarks>
/// <param name="Name">The attribute name.</param>
/// <param name="ValueType">The type of the value expression, defining how the <paramref name="value"/> should be
/// interpreted.</param>
/// <param name="Value">The literal value text.</param>
public record SAttribute(string Name, AttributeValueType ValueType, string Value) : IAttribute
{
    /// <summary>
    /// Initializes a new <see cref="SAttribute"/> from the data of a parsed attribute.
    /// </summary>
    /// <param name="attribute">The parsed attribute.</param>
    public SAttribute(Grammar.Attribute attribute)
        : this(attribute.Name.ToString(), attribute.ValueType, attribute.Value.ToString()) { }
}
