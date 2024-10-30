using System.Text;
using StardewUI.Framework.Grammar;

namespace StardewUI.Framework.Dom;

/// <summary>
/// Attribute of a StarML element.
/// </summary>
public interface IAttribute
{
    /// <summary>
    /// Specifies the redirect to use for a context binding, if applicable and if the <see cref="ValueType"/> is one of
    /// the context binding types.
    /// </summary>
    ContextRedirect? ContextRedirect { get; }

    /// <summary>
    /// The attribute name.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// The type of the attribute itself, defining how the <see cref="Name"/> should be interpreted.
    /// </summary>
    AttributeType Type { get; }

    /// <summary>
    /// The literal value text.
    /// </summary>
    string Value { get; }

    /// <summary>
    /// The type of the value expression, defining how the <see cref="Value"/> should be interpreted.
    /// </summary>
    AttributeValueType ValueType { get; }

    /// <summary>
    /// Prints the textual representation of this node.
    /// </summary>
    /// <param name="sb">Builder to receive the attribute's text output.</param>
    void Print(StringBuilder sb)
    {
        if (Type == AttributeType.Structural)
        {
            sb.Append('*');
        }
        sb.Append(Name);
        sb.Append('=');
        var (openChars, closeChars) = GetValueTypePair(ValueType);
        sb.Append(openChars);
        ContextRedirect?.Print(sb);
        sb.Append(Value);
        sb.Append(closeChars);
    }

    private static (string, string) GetValueTypePair(AttributeValueType type)
    {
        return type switch
        {
            AttributeValueType.Literal => ("\"", "\""),
            AttributeValueType.InputBinding => ("{<", "}"),
            AttributeValueType.OneTimeBinding => ("{<:", "}"),
            AttributeValueType.OutputBinding => ("{>", "}"),
            AttributeValueType.TwoWayBinding => ("{<>", "}"),
            AttributeValueType.AssetBinding => ("{@", "}"),
            _ => throw new ArgumentException($"Invalid attribute value type: {type}", nameof(type)),
        };
    }
}

/// <summary>
/// Describes how to redirect the target context of any <see cref="IAttribute"/> whose
/// <see cref="IAttribute.ValueType"/> is one of the <see cref="AttributeValueTypeExtensions.IsContextBinding"/>
/// matching types.
/// </summary>
public abstract record ContextRedirect
{
    /// <summary>
    /// Creates an optional <see cref="ContextRedirect"/> using the constituent parts parsed from a grammar element such
    /// as an <see cref="Grammar.Attribute"/>.
    /// </summary>
    /// <param name="parentDepth">Number of parents to traverse.</param>
    /// <param name="parentType">The <see cref="System.Reflection.MemberInfo.Name"/> of the target ancestor's
    /// <see cref="System.Type"/>.</param>
    /// <returns>A new <see cref="ContextRedirect"/> that performs the requested redirect, or <c>null</c> if the
    /// arguments would cause no redirection to occur.</returns>
    public static ContextRedirect? FromParts(uint parentDepth, ReadOnlySpan<char> parentType)
    {
        if (!parentType.IsEmpty)
        {
            return new Type(parentType.ToString());
        }
        if (parentDepth > 0)
        {
            return new Distance(parentDepth);
        }
        return null;
    }

    /// <summary>
    /// Prints the textual representation of this redirect.
    /// </summary>
    /// <param name="sb">Builder to receive the redirect's text output.</param>
    internal abstract void Print(StringBuilder sb);

    /// <summary>
    /// Redirects to an ancestor context by walking up a specified number of levels.
    /// </summary>
    /// <param name="Depth">Number of parents to traverse.</param>
    public sealed record Distance(uint Depth) : ContextRedirect
    {
        /// <inheritdoc />
        internal override void Print(StringBuilder sb)
        {
            sb.Append(new string('^', (int)Depth));
        }
    }

    /// <summary>
    /// Redirects to the nearest ancestor matching a specified type.
    /// </summary>
    /// <param name="TypeName">The <see cref="System.Reflection.MemberInfo.Name"/> of the target ancestor's
    /// <see cref="System.Type"/>.</param>
    public sealed record Type(string TypeName) : ContextRedirect
    {
        /// <inheritdoc />
        internal override void Print(StringBuilder sb)
        {
            sb.Append('~');
            sb.Append(TypeName);
            sb.Append('.');
        }
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
/// <param name="Value">The literal value text.</param>
/// <param name="Type">The type of the attribute itself, defining how the <paramref name="Name"/> should be
/// interpreted.</param>
/// <param name="ValueType">The type of the value expression, defining how the <paramref name="Value"/> should be
/// interpreted.</param>
/// <param name="ContextRedirect">Specifies the redirect to use for a context binding, if applicable and if the
/// <paramref name="ValueType"/> is one of the context binding types.</param>
public record SAttribute(
    string Name,
    string Value,
    AttributeType Type = AttributeType.Property,
    AttributeValueType ValueType = AttributeValueType.Literal,
    ContextRedirect? ContextRedirect = null
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
            ContextRedirect.FromParts(attribute.ParentDepth, attribute.ParentType)
        ) { }
}
