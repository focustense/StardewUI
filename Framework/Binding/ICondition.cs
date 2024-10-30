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
