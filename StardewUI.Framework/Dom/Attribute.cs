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
public record SAttribute(string Name, AttributeValueType ValueType, string Value) : IAttribute;
