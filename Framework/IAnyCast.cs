namespace StardewUI.Framework;

/// <summary>
/// A marker interface that, when used in place of <see cref="Object"/>, forces the framework to attempt an explicit
/// conversion/cast to the expected destination type.
/// </summary>
/// <remarks>
/// Casts are normally only permitted when the source type is actually assignable to the destination type.
/// <c>IAnyCast</c> is used to indicate that the source is intentionally ambiguous or boxed, and the real type cannot be
/// known until the conversion is attempted, at which point it is assumed to be assignment-compatible or have an
/// explicit conversion operator.
/// </remarks>
public interface IAnyCast
{
    /// <summary>
    /// The boxed value.
    /// </summary>
    Object? Value { get; }
}

/// <summary>
/// Value type used for an <see cref="IAnyCast"/>.
/// </summary>
/// <param name="Value">The boxed value.</param>
internal record struct AnyCastValue(Object? Value) : IAnyCast;
