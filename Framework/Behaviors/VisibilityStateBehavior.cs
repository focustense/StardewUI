using System.ComponentModel;
using StardewUI.Events;
using StardewUI.Layout;

namespace StardewUI.Framework.Behaviors;

/// <summary>
/// Behavior that applies a property override when a view enters or leaves the visible state.
/// </summary>
/// <remarks>
/// The override is added when the view's <see cref="IView.Visibility"/> becomes <see cref="Visibility.Visibility"/> and
/// removed when it becomes <see cref="Visibility.Hidden"/>. In addition, the state corresponding to the view's initial
/// visibility is applied as soon as the behavior as initialized, allowing for transitions to occur when the view is
/// first created even if the visibility never explicitly changes.
/// </remarks>
/// <typeparam name="TValue">Value type for the overridden property.</typeparam>
/// <param name="propertyName">Name of the overridden property.</param>
public class VisibilityStateBehavior<TValue>(string propertyName) : ViewBehavior<IView, TValue>
{
    /// <inheritdoc />
    public override void Update(TimeSpan elapsed) { }

    /// <inheritdoc />
    protected override void OnDispose()
    {
        View.PropertyChanged -= View_PropertyChanged;
    }

    /// <inheritdoc />
    protected override void OnInitialize()
    {
        View.PropertyChanged += View_PropertyChanged;
        if (View.Visibility == Visibility.Visible)
        {
            ViewState.GetOrAddProperty<TValue>(propertyName).Push("visible", Data);
        }
    }

    /// <inheritdoc />
    protected override void OnNewData(TValue? previousData)
    {
        // In many instances, the state only gets pushed once when the behavior is first initialized, and never
        // recreated due to a visibility change; however, the attribute value can still change and this needs to be
        // updated in the state list.
        ViewState.GetProperty<TValue>(propertyName)?.Replace("visible", Data);
    }

    private void Apply()
    {
        if (View.Visibility == Visibility.Visible)
        {
            ViewState.GetOrAddProperty<TValue>(propertyName).Push("visible", Data);
        }
        else
        {
            ViewState.GetProperty<TValue>(propertyName)?.TryRemove("visible", out _);
        }
    }

    private void View_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(View.Visibility))
        {
            Apply();
        }
    }
}
