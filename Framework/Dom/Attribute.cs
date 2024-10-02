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
    /// The depth to walk - i.e. number of parents to traverse - to find the context on which to evaluate a context
    /// binding. Only valid if the <paramref name="valueType"/> is a type that matches
    /// <see cref="AttributeValueTypeExtensions.IsContextBinding"/>.
    /// </summary>
    int ParentDepth { get; }

    /// <summary>
    /// The type of the attribute itself, defining how the <see cref="Name"/> should be interpreted.
    /// </summary>
    AttributeType Type { get; }

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
/// <param name="Value">The literal value text.</param>
/// <param name="Type">The type of the attribute itself, defining how the <see cref="Name"/> should be
/// interpreted.</param>
/// <param name="ValueType">The type of the value expression, defining how the <paramref name="value"/> should be
/// interpreted.</param>
/// <param name="ParentDepth">The depth to walk - i.e. number of parents to traverse - to find the context on which to
/// evaluate a context binding. Only valid if the <paramref name="valueType"/> is a type that matches
/// <see cref="AttributeValueTypeExtensions.IsContextBinding"/>.</param>
/// </summary>
public record SAttribute(
    string Name,
    string Value,
    AttributeType Type = AttributeType.Property,
    AttributeValueType ValueType = AttributeValueType.Literal,
    int ParentDepth = 0
) : IAttribute
{
    /// <summary>
    /// Initializes a new <see cref="SAttribute"/> from the data of a parsed attribute.
    /// </summary>
    /// <param name="attribute">The parsed attribute.</param>
    public SAttribute(Grammar.Attribute attribute)
        : this(
            attribute.Name.ToString(),
            attribute.Value.ToString(),
            attribute.Type,
            attribute.ValueType,
            attribute.ParentDepth
        ) { }
}
