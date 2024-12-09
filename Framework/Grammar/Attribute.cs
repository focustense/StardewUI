namespace StardewUI.Framework.Grammar;

/// <summary>
/// A complete attribute assignment parsed from StarML.
/// </summary>
/// <param name="name">The attribute name.</param>
/// <param name="type">The type of the attribute itself, i.e. how its <paramref name="name"/> should be
/// interpreted.</param>
/// <param name="isNegated">Whether the attribute has a negation (<c>!</c>) operator before assignment.</param>
/// <param name="valueType">The type of the value expression, defining how the <paramref name="value"/> should be
/// interpreted.</param>
/// <param name="value">The literal value text.</param>
/// <param name="parentDepth">The depth to walk - i.e. number of parents to traverse - to find the context on which to
/// evaluate a context binding. Only valid if the <paramref name="valueType"/> is a type that matches
/// <see cref="AttributeValueTypeExtensions.IsContextBinding"/>.</param>
/// <param name="parentType">The type name of the parent to search for, to find the the context on which to evaluate a
/// context binding. Exclusive with <paramref name="parentDepth"/> and only valid if the <paramref name="valueType"/> is
/// a type that matches <see cref="AttributeValueTypeExtensions.IsContextBinding"/>.</param>
public readonly ref struct Attribute(
    ReadOnlySpan<char> name,
    AttributeType type,
    bool isNegated,
    AttributeValueType valueType,
    ReadOnlySpan<char> value,
    uint parentDepth,
    ReadOnlySpan<char> parentType
)
{
    /// <summary>
    /// Whether the attribute has a negation (<c>!</c>) operator before assignment.
    /// </summary>
    /// <remarks>
    /// Negation behavior is specific to the exact attribute and is not supported for many/most attributes.
    /// </remarks>
    public bool IsNegated { get; } = isNegated;

    /// <summary>
    /// The attribute name.
    /// </summary>
    public ReadOnlySpan<char> Name { get; } = name;

    /// <summary>
    /// The depth to walk - i.e. number of parents to traverse - to find the context on which to evaluate a context
    /// binding. Exclusive with <see cref="ParentType"/> and only valid if the <see cref="ValueType"/> is a type that
    /// matches <see cref="AttributeValueTypeExtensions.IsContextBinding"/>.
    /// </summary>
    public uint ParentDepth { get; } = parentDepth;

    /// <summary>
    /// The type name of the parent to walk up to for a context redirect. Exclusive with <see cref="ParentDepth"/> and
    /// only valid if the <see cref="ValueType"/> is a type that matches
    /// <see cref="AttributeValueTypeExtensions.IsContextBinding"/>.
    /// </summary>
    public ReadOnlySpan<char> ParentType { get; } = parentType;

    /// <summary>
    /// The type of the attribute itself, i.e. how its <see cref="Name"/> should be interpreted.
    /// </summary>
    public AttributeType Type { get; } = type;

    /// <summary>
    /// The literal value text.
    /// </summary>
    public ReadOnlySpan<char> Value { get; } = value;

    /// <summary>
    /// The type of the value expression, defining how the <see cref="Value"/> should be interpreted.
    /// </summary>
    public AttributeValueType ValueType { get; } = valueType;
}

/// <summary>
/// The different types of an <see cref="Attribute"/>, independent of its value.
/// </summary>
public enum AttributeType
{
    /// <summary>
    /// Sets or binds a property on the target view.
    /// </summary>
    Property,

    /// <summary>
    /// Affects the structure or hierarchy of the view tree, e.g. by making a node conditional or repeated.
    /// </summary>
    Structural,
}

/// <summary>
/// Types allowed for the value of an <see cref="Attribute"/>.
/// </summary>
public enum AttributeValueType
{
    /// <summary>
    /// The value is the literal string in the markup, i.e. it is the actual string representation of the target data
    /// type such as an integer, enumeration or another string.
    /// </summary>
    Literal,

    /// <summary>
    /// A read-only binding which obtains the value from a named game asset.
    /// </summary>
    AssetBinding,

    /// <summary>
    /// A read-only binding which obtains the value from a translation key registered with SMAPI.
    /// </summary>
    TranslationBinding,

    /// <summary>
    /// A one-way data binding which obtains the value from the context data and assigns it to the view.
    /// </summary>
    InputBinding,

    /// <summary>
    /// A special type of <see cref="InputBinding"/> that only reads the value a single time, and does not update
    /// subsequently afterward.
    /// </summary>
    OneTimeBinding,

    /// <summary>
    /// A one-way data binding which obtains the value from the view and assigns it to the context data.
    /// </summary>
    OutputBinding,

    /// <summary>
    /// A two-way data binding which both assigns the context data's value to the view, and the view's value to the
    /// context data, depending on which one was most recently changed.
    /// </summary>
    TwoWayBinding,

    /// <summary>
    /// Binds to the attribute value of the containing template; only valid within a template node.
    /// </summary>
    TemplateBinding,
}

/// <summary>
/// Extensions for the <see cref="AttributeValueType"/> enum.
/// </summary>
public static class AttributeValueTypeExtensions
{
    /// <summary>
    /// Tests if a given <paramref name="valueType"/> is any type of context binding, regardless of its direction.
    /// </summary>
    /// <param name="valueType">The value type.</param>
    /// <returns><c>true</c> if the attribute binds to a context property; <c>false</c> if it is some other type of
    /// attribute such as <see cref="AttributeValueType.Literal"/> or
    /// <see cref="AttributeValueType.AssetBinding"/>.</returns>
    public static bool IsContextBinding(this AttributeValueType valueType)
    {
        return valueType == AttributeValueType.InputBinding
            || valueType == AttributeValueType.OneTimeBinding
            || valueType == AttributeValueType.OutputBinding
            || valueType == AttributeValueType.TwoWayBinding;
    }
}
