using StardewUI.Events;

namespace StardewUI.Framework.Behaviors;

/// <summary>
/// Behavior that applies a property override when a view enters a hover state.
/// </summary>
/// <remarks>
/// The override is added on pointer enter (i.e. when initiating hover) and removed on pointer leave.
/// </remarks>
/// <typeparam name="TValue">Value type for the overridden property.</typeparam>
/// <param name="propertyName">Name of the overridden property.</param>
public class HoverStateBehavior<TValue>(string propertyName) : ViewBehavior<IView, TValue>
{
    /// <inheritdoc />
    public override void Update(TimeSpan elapsed) { }

    /// <inheritdoc />
    protected override void OnDispose()
    {
        View.PointerEnter -= View_PointerEnter;
        View.PointerLeave -= View_PointerLeave;
    }

    /// <inheritdoc />
    protected override void OnInitialize()
    {
        View.PointerEnter += View_PointerEnter;
        View.PointerLeave += View_PointerLeave;
    }

    /// <inheritdoc />
    protected override void OnNewData(TValue? previousData)
    {
        ViewState.GetProperty<TValue>(propertyName)?.Replace("hover", Data);
    }

    private void View_PointerEnter(object? sender, PointerEventArgs e)
    {
        ViewState.GetOrAddProperty<TValue>(propertyName).Push("hover", Data);
    }

    private void View_PointerLeave(object? sender, PointerEventArgs e)
    {
        ViewState.GetProperty<TValue>(propertyName)?.TryRemove("hover", out _);
    }
}
