namespace StardewUI.Framework.Behaviors;

/// <summary>
/// Updates a view state flag with a boolean value corresponding to the behavior's data.
/// </summary>
/// <remarks>
/// Essentially enables arbitrary state names to be linked with context properties that are boolean-valued or
/// convertible to boolean, primarily as a bridge for the <see cref="FlagStateBehavior{TValue}"/> which in turn allows
/// property changes to be associated with the state. In other words, part one of the two-part process used to create
/// conditional attributes.
/// </remarks>
/// <param name="flagName">Name of the flag to set when <see cref="ViewBehavior{TView, TData}.Data"/> is
/// <c>true</c>.</param>
public class ConditionalFlagBehavior(string flagName) : ViewBehavior<IView, bool>
{
    /// <inheritdoc />
    protected override void OnAttached()
    {
        UpdateViewData();
    }

    /// <inheritdoc />
    protected override void OnNewData(bool previousData)
    {
        UpdateViewData();
    }

    private void UpdateViewData()
    {
        ViewState.SetFlag(flagName, Data ? true : null);
    }
}
