using StardewUI.Framework.Behaviors;
using StardewUI.Framework.Content;
using StardewUI.Framework.Converters;
using StardewUI.Framework.Dom;
using StardewUI.Framework.Sources;

namespace StardewUI.Framework.Binding;

/// <summary>
/// Wrapper for the entire set of behaviors attached to a single node/view.
/// </summary>
/// <param name="behaviorAttributes">List of all behavior attributes applied to the node.</param>
/// <param name="behaviorFactory">Factory for creating behaviors.</param>
/// <param name="valueSourceFactory">The factory responsible for creating <see cref="IValueSource{T}"/> instances from
/// attribute data.</param>
/// <param name="valueConverterFactory">The factory responsible for creating
/// <see cref="IValueConverter{TSource, TDestination}"/> instances, used to convert bound values to the data types
/// required by individual behaviors.</param>
/// <param name="resolutionScope">Scope for resolving externalized attributes, such as translation keys.</param>
public class ViewBehaviors(
    IEnumerable<IAttribute> behaviorAttributes,
    IBehaviorFactory behaviorFactory,
    IValueSourceFactory valueSourceFactory,
    IValueConverterFactory valueConverterFactory,
    IResolutionScope resolutionScope
) : IDisposable
{
    private class Binding(IAttribute attribute) : IDisposable
    {
        public IAttribute Attribute { get; } = attribute;
        public IViewBehavior? Behavior { get; set; }
        public IValueSource? DataSource { get; set; }

        public void Detach()
        {
            Behavior?.Dispose();
            Behavior = null;
        }

        public void Dispose()
        {
            Detach();
            DataSource = null;
            GC.SuppressFinalize(this);
        }
    }

    private readonly List<Binding> bindings = behaviorAttributes.Select(attr => new Binding(attr)).ToList();

    private BindingContext? context;
    private bool hasTarget;

    /// <inheritdoc />
    public void Dispose()
    {
        foreach (var binding in bindings)
        {
            binding.Dispose();
        }
        context = null;
        hasTarget = false;
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Updates or removes the binding context for all managed behaviors.
    /// </summary>
    /// <param name="context">The new context.</param>
    public void SetContext(BindingContext? context)
    {
        this.context = context;
        foreach (var binding in bindings)
        {
            if (binding.Behavior is not null)
            {
                UpdateDataSource(binding);
            }
        }
    }

    /// <summary>
    /// Updates the attached target (view and state) for all managed behaviors.
    /// </summary>
    /// <remarks>
    /// If the target is <c>null</c> then all behaviors will be disabled/removed.
    /// </remarks>
    /// <param name="target">The new behavior target, or <c>null</c> to remove behaviors.</param>
    public void SetTarget(BehaviorTarget? target)
    {
        hasTarget = target is not null;
        foreach (var binding in bindings)
        {
            if (binding.Behavior is null)
            {
                if (target is null)
                {
                    binding.Detach();
                }
                else
                {
                    binding.Behavior = CreateBehavior(target, binding.Attribute);
                }
                UpdateDataSource(binding);
            }
        }
    }

    /// <summary>
    /// Runs on every update tick.
    /// </summary>
    /// <param name="elapsed">Time elapsed since last tick.</param>
    public void Update(TimeSpan elapsed)
    {
        if (!hasTarget)
        {
            return;
        }
        foreach (var binding in bindings)
        {
            if (binding.Behavior is not IViewBehavior behavior)
            {
                continue;
            }
            if (binding.DataSource?.Update() == true)
            {
                behavior.SetData(binding.DataSource.Value);
            }
            if (behavior.CanUpdate())
            {
                binding.Behavior.Update(elapsed);
            }
        }
    }

    private IViewBehavior CreateBehavior(BehaviorTarget target, IAttribute attribute)
    {
        var argumentIndex = attribute.Name.IndexOf(':');
        var (name, argument) =
            argumentIndex >= 0
                ? (attribute.Name[..argumentIndex], attribute.Name[(argumentIndex + 1)..])
                : (attribute.Name, "");
        var behavior = behaviorFactory.CreateBehavior(target.View.GetType(), name, argument);
        behavior.Initialize(target);
        return behavior;
    }

    private void UpdateDataSource(Binding binding)
    {
        if (binding.Behavior is null)
        {
            return;
        }
        var valueType = valueSourceFactory.GetValueType(binding.Attribute, null, context);
        var valueSource = valueType is not null
            ? valueSourceFactory.GetValueSource(valueType, binding.Attribute, context, resolutionScope)
            : null;
        binding.DataSource = valueSource is not null
            ? ConvertedValueSource.Create(valueSource, binding.Behavior.DataType, valueConverterFactory)
            : null;
        binding.Behavior.SetData(binding.DataSource?.Value);
    }
}
