namespace StardewUI.Framework.Binding;

/// <summary>
/// A condition used in a <see cref="ConditionalNode"/>.
/// </summary>
public interface ICondition : IDisposable
{
    /// <summary>
    /// The context for evaluating the condition; i.e. the context of the node to which the condition applies.
    /// </summary>
    BindingContext? Context { get; set; }

    /// <summary>
    /// Whether or not the condition was passing as of the last <see cref="Update"/>.
    /// </summary>
    bool Passed { get; }

    /// <summary>
    /// Re-evaluates the condition and updates the <see cref="Passed"/> state.
    /// </summary>
    void Update();
}

/// <summary>
/// Extensions for the <see cref="ICondition"/> interface.
/// </summary>
public static class ConditionExtensions
{
    /// <summary>
    /// Checks a negation flag, and returns a negated version of the <paramref name="condition"/> if set.
    /// </summary>
    /// <param name="condition">The original condition.</param>
    /// <param name="isNegated">Whether or not to negate the <paramref name="condition"/>.</param>
    /// <returns>A negated version of the <paramref name="condition"/>, if <paramref name="isNegated"/> is <c>true</c>;
    /// otherwise, the original <paramref name="condition"/>.</returns>
    public static ICondition NegateIf(this ICondition condition, bool isNegated)
    {
        return isNegated ? new NegatedCondition(condition) : condition;
    }
}
