namespace StardewUI.Framework.Binding;

/// <summary>
/// Wrapper for an <see cref="ICondition"/> that negates its outcome.
/// </summary>
public class NegatedCondition(ICondition innerCondition) : ICondition
{
    /// <inheritdoc />
    public BindingContext? Context
    {
        get => innerCondition.Context;
        set => innerCondition.Context = value;
    }

    /// <inheritdoc />
    public bool Passed => !innerCondition.Passed;

    /// <inheritdoc />
    public void Dispose()
    {
        innerCondition.Dispose();
        GC.SuppressFinalize(this);
    }

    /// <inheritdoc />
    public void Update()
    {
        innerCondition.Update();
    }
}
