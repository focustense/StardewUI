namespace StardewUI.Framework.Behaviors;

/// <summary>
/// Behavior that applies a property override when a named view state flag is detected.
/// </summary>
/// <remarks>
/// The flag is generally added or removed by <see cref="ConditionalFlagBehavior"/> as part of a two-step approach to
/// creating conditional attributes.
/// </remarks>
/// <param name="flagName">Name of the flag to watch.</param>
/// <param name="propertyName">Name of the overridden property.</param>
/// <typeparam name="TValue">Value type for the overridden property.</typeparam>
public class FlagStateBehavior<TValue>(string flagName, string propertyName) : ViewBehavior<IView, TValue>
{
    private readonly string stateName = $"_Flag:{flagName}";

    /// <inheritdoc />
    protected override void OnAttached()
    {
        if (ViewState.GetFlag<bool>(flagName))
        {
            ViewState.GetOrAddProperty<TValue>(propertyName).ReplaceOrPush(stateName, Data);
        }
        ViewState.FlagChanged += ViewState_FlagChanged;
    }

    /// <inheritdoc />
    protected override void OnDetached(IView view)
    {
        ViewState.FlagChanged -= ViewState_FlagChanged;
    }

    /// <inheritdoc />
    protected override void OnNewData(TValue? previousData)
    {
        ViewState.GetProperty<TValue>(propertyName)?.Replace(stateName, Data);
    }

    private void ViewState_FlagChanged(object? sender, FlagEventArgs e)
    {
        bool flag = ViewState.GetFlag<bool>(flagName);
        if (flag)
        {
            ViewState.GetOrAddProperty<TValue>(propertyName).ReplaceOrPush(stateName, Data);
        }
        else
        {
            ViewState.GetProperty<TValue>(propertyName)?.TryRemove(stateName, out _);
        }
    }
}
