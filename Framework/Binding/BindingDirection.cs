namespace StardewUI.Framework.Binding;

/// <summary>
/// The direction of data flow in a data binding.
/// </summary>
public enum BindingDirection
{
    /// <summary>
    /// An input binding, i.e. the view receives its value from the context.
    /// </summary>
    /// <remarks>
    /// This is implicitly the binding type for all read-only sources, such as assets.
    /// </remarks>
    In,

    /// <summary>
    /// An output binding, i.e. the view publishes its value to the context.
    /// </summary>
    Out,

    /// <summary>
    /// A binding that is both input and output, i.e. the view receives its value from the context and also publishes
    /// its value to the context, depending on where the most recent change occurred.
    /// </summary>
    InOut,
}

/// <summary>
/// Extension methods for the <see cref="BindingDirection"/> enum.
/// </summary>
public static class BindingDirectionExtensions
{
    /// <summary>
    /// Gets whether or not a direction includes an input binding, i.e. is either <see cref="BindingDirection.In"/>
    /// or <see cref="BindingDirection.InOut"/>.
    /// </summary>
    /// <param name="direction">The binding direction.</param>
    public static bool IsIn(this BindingDirection direction)
    {
        return direction == BindingDirection.In || direction == BindingDirection.InOut;
    }

    /// <summary>
    /// Gets whether or not a direction includes an output binding, i.e. is either <see cref="BindingDirection.Out"/>
    /// or <see cref="BindingDirection.InOut"/>.
    /// </summary>
    /// <param name="direction">The binding direction.</param>
    public static bool IsOut(this BindingDirection direction)
    {
        return direction == BindingDirection.Out || direction == BindingDirection.InOut;
    }
}
