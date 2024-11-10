namespace StardewUI;

/// <summary>
/// Specifies a property name to use for duck-type conversions, if different from the member name.
/// </summary>
/// <remarks>
/// <para>
/// This attribute is applied to the type being converted <em>from</em>, unlike <see cref="DuckTypeAttribute"/> which
/// applies to the target type. It is used to match a target property with a different name, or to match multiple target
/// properties with a single source field.
/// </para>
/// <para>
/// Multiple copies of the attribute can be used to match multiple target properties, with
/// <paramref name="targetTypeName"/> being used to optionally filter which type conversions it will apply to, if the
/// data type might be used in more than one kind of conversion.
/// </para>
/// </remarks>
/// <param name="targetPropertyName">The name of the property to match on the target type.</param>
/// <param name="targetTypeName">Name of the conversion target type (i.e. type with <see cref="DuckTypeAttribute"/>)
/// to which this rename applies, not including the namespace or generic arguments. If not set, the property will be
/// available under the specified <paramref name="targetPropertyName"/> for all conversions.</param>
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
public class DuckPropertyAttribute(string targetPropertyName, string? targetTypeName = null) : Attribute
{
    /// <summary>
    /// The name of the property to match on the target type.
    /// </summary>
    public string TargetPropertyName { get; } = targetPropertyName;

    /// <summary>
    /// Name of the conversion target type (i.e. type with <see cref="DuckTypeAttribute"/>) to which this rename
    /// applies, not including the namespace or generic arguments. If not set, the property will be available under the
    /// specified <see cref="TargetPropertyName"/> for all conversions.
    /// </summary>
    public string? TargetTypeName { get; } = targetTypeName;
}

/// <summary>
/// Specifies that a type is eligible for duck-type conversions in data bindings.
/// </summary>
/// <remarks>
/// <para>
/// This attribute is not used by the core library, only the data binding framework. When a type is decorated with it,
/// values of normally non-convertible types, such as user-defined types in a separate mod, can become eligible for
/// conversion to the decorated type and have converters generated at runtime, as long as the external type's properties
/// are sufficient to satisfy one of the decorated type's constructors; or, in the case of default constructors, when
/// the external type can contribute at least one property value.
/// </para>
/// <para>
/// Duck type conversions always match using the combined property type and (case-insensitive) name. The name of the
/// decorated type's property or constructor argument must match the name of the property on the source type, unless
/// <see cref="DuckPropertyAttribute"/> is specified, in which case it must match the specified name(s).
/// </para>
/// <para>
/// </para>
/// </remarks>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
public class DuckTypeAttribute : Attribute { }
