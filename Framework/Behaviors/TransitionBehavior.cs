using StardewUI.Animation;
using StardewUI.Framework.Descriptors;

namespace StardewUI.Framework.Behaviors;

/// <summary>
/// Behavior that applies gradual transitions (AKA tweens) to view properties.
/// </summary>
/// <typeparam name="TValue">Value type for the transitioned property.</typeparam>
/// <param name="propertyName">Name of the overridden property.</param>
/// <param name="lerp">Interpolation function for the transitioned property type.</param>
public class TransitionBehavior<TValue>(string propertyName, Lerp<TValue> lerp) : ViewBehavior<IView, Transition>
{
    private TimeSpan elapsed;
    private TValue? fromValue;
    private bool hasTargetValue;
    private bool isTransitionActive;
    private IPropertyDescriptor<TValue> property = null!; // Initialized in OnInitialize
    private TValue? targetValue;
    private TValue? viewValue;

    /// <inheritdoc />
    public override void PreUpdate(TimeSpan elapsed)
    {
        viewValue = property.GetValue(View);
    }

    /// <inheritdoc />
    public override void Update(TimeSpan elapsed)
    {
        var propertyState = ViewState.GetOrAddProperty<TValue>(propertyName);
        if (isTransitionActive)
        {
            propertyState.TryRemove("transition", out _);
        }
        var targetValue = propertyState.TryPeekValue(out var value)
            ? value
            : ViewState.GetDefaultValue<TValue>(property.Name);
        if (!hasTargetValue || !EqualityComparer<TValue>.Default.Equals(targetValue, this.targetValue))
        {
            fromValue = viewValue;
            this.targetValue = targetValue;
            hasTargetValue = true;
            this.elapsed = TimeSpan.Zero;
            isTransitionActive = true;
        }
        if (!isTransitionActive)
        {
            return;
        }
        this.elapsed += elapsed;
        if (this.elapsed >= Data.TotalDuration)
        {
            isTransitionActive = false;
        }
        else
        {
            var position = Data.GetPosition(this.elapsed);
            var nextValue = lerp(fromValue!, this.targetValue!, position);
            propertyState.Push("transition", nextValue);
        }
    }

    /// <inheritdoc />
    protected override void OnAttached()
    {
        property =
            (IPropertyDescriptor<TValue>)DescriptorFactory.GetViewDescriptor(View.GetType()).GetProperty(propertyName);
        viewValue = property.GetValue(View);
    }

    /// <inheritdoc />
    protected override void OnDetached(IView view)
    {
        targetValue = default;
        hasTargetValue = false;
    }
}
