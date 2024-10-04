namespace StardewUI.Framework.Grammar;

/// <summary>
/// A complete method argument parsed from StarML.
/// </summary>
/// <param name="expressionType">The type describing how <paramref name="expression"/> should be interpreted.</param>
/// <param name="expression">The literal expression text.</param>
/// <param name="parentDepth">The depth to walk - i.e. number of parents to traverse - to find the context on which to
/// evaluate the <paramref name="expression"/> when the <paramref name="expressionType"/> is
/// <see cref="ArgumentExpressionType.ContextBinding"/>.</param>
/// <param name="parentType">The type name of the parent to walk up to for a context redirect. Exclusive with
/// <paramref name="parentDepth"/> and only valid if the <paramref name="expressionType"/> is
/// <see cref="ArgumentExpressionType.ContextBinding"/>.</param>
public readonly ref struct Argument(
    ArgumentExpressionType expressionType,
    ReadOnlySpan<char> expression,
    uint parentDepth,
    ReadOnlySpan<char> parentType
)
{
    /// <summary>
    /// The literal expression text.
    /// </summary>
    public ReadOnlySpan<char> Expression { get; } = expression;

    /// <summary>
    /// The type describing how <paramref name="Expression"/> should be interpreted.
    /// </summary>
    public ArgumentExpressionType ExpressionType { get; } = expressionType;

    /// <summary>
    /// The depth to walk - i.e. number of parents to traverse - to find the context on which to evaluate the
    /// <see cref="Expression"/> when the <see cref="ExpressionType"/> is
    /// <see cref="ArgumentExpressionType.ContextBinding"/>.
    /// </summary>
    public uint ParentDepth { get; } = parentDepth;

    /// <summary>
    /// The type name of the parent to walk up to for a context redirect. Exclusive with <see cref="ParentDepth"/> and
    /// only valid if the <see cref="ExpressionType"/> is <see cref="ArgumentExpressionType.ContextBinding"/>.
    /// </summary>
    public ReadOnlySpan<char> ParentType { get; } = parentType;
}

/// <summary>
/// Defines the possible types of an <see cref="IArgument"/>, which specifies how to resolve its value at runtime.
/// </summary>
public enum ArgumentExpressionType
{
    /// <summary>
    /// The value is the literal string in the markup, i.e. it is the actual string representation of the target data
    /// type such as an integer, enumeration or another string.
    /// </summary>
    Literal,

    /// <summary>
    /// The current value of some property in the context data.
    /// </summary>
    /// <remarks>
    /// This has behavior similar to <see cref="Grammar.AttributeValueType.InputBinding"/>, but is not specifically
    /// classified as "input" because arguments are always read-only/input-only.
    /// </remarks>
    ContextBinding,
}
