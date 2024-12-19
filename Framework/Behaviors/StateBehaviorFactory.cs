using System.Collections.Concurrent;
using System.Reflection;
using StardewUI.Animation;
using StardewUI.Framework.Binding;
using StardewUI.Framework.Descriptors;

namespace StardewUI.Framework.Behaviors;

/// <summary>
/// Factory for creating behaviors that apply single-property overrides on state transitions, such as
/// <c>hover:transform</c>.
/// </summary>
/// <remarks>
/// Also handles transitions, which follow a similar creation mechanism.
/// </remarks>
public class StateBehaviorFactory : IBehaviorFactory
{
    private delegate IViewBehavior BehaviorCreator(string propertyName);

    private record BehaviorPropertyKey(string Name, Type PropertyType);

    private record BehaviorViewKey(Type ViewType, string ArgumentName);

    private static readonly MethodInfo CreateHoverBehaviorMethod = GetFactoryMethod(nameof(CreateHoverBehavior))!;
    private static readonly MethodInfo CreateTransitionBehaviorMethod = GetFactoryMethod(
        nameof(CreateTransitionBehavior)
    )!;

    private static readonly ConcurrentDictionary<BehaviorPropertyKey, BehaviorCreator> creatorsByProperty = [];
    private static readonly ConcurrentDictionary<BehaviorViewKey, BehaviorCreator> creatorsByViewArg = [];

    private static MethodInfo? GetFactoryMethod(string name)
    {
        return typeof(StateBehaviorFactory).GetMethod(name, BindingFlags.Static | BindingFlags.NonPublic);
    }

    /// <inheritdoc />
    public IViewBehavior CreateBehavior(Type viewType, string name, string argument)
    {
        var propertyName = argument.AsSpan().ToUpperCamelCase();
        var viewKey = new BehaviorViewKey(viewType, name + ':' + argument);
        var creator = creatorsByViewArg.GetOrAdd(
            viewKey,
            _ =>
            {
                var viewDescriptor = DescriptorFactory.GetViewDescriptor(viewType);
                var propertyDescriptor = viewDescriptor.GetProperty(propertyName);
                var propertyKey = new BehaviorPropertyKey(name, propertyDescriptor.ValueType);
                return creatorsByProperty.GetOrAdd(
                    propertyKey,
                    _ => GetCreatorDelegate(propertyKey.Name, propertyKey.PropertyType)
                );
            }
        );
        return creator(propertyName);
    }

    /// <inheritdoc />
    public bool SupportsName(string name)
    {
        // Keep in sync with GetCreatorDelegate.
        return name.Equals("hover", StringComparison.OrdinalIgnoreCase)
            || name.Equals("transition", StringComparison.OrdinalIgnoreCase);
    }

    private static BehaviorCreator GetCreatorDelegate(string stateName, Type valueType)
    {
        // Keep in sync with SupportsName.
        var method = stateName.ToLowerInvariant() switch
        {
            "hover" => CreateHoverBehaviorMethod.MakeGenericMethod(valueType),
            "transition" => CreateTransitionBehaviorMethod.MakeGenericMethod(valueType),
            _ => throw new ArgumentException($"Unrecognized state name '{stateName}'.", nameof(stateName)),
        };
        return method.CreateDelegate<BehaviorCreator>();
    }

    private static IViewBehavior CreateHoverBehavior<TValue>(string propertyName)
    {
        return new HoverStateBehavior<TValue>(propertyName);
    }

    private static IViewBehavior CreateTransitionBehavior<TValue>(string propertyName)
    {
        var lerp =
            Lerps.Get<TValue>()
            ?? throw new BindingException(
                $"Cannot transition property '{propertyName}' with type {typeof(TValue).FullName} because the value type "
                    + "does not have a defined interpolation (Lerp) function."
            );
        return new TransitionBehavior<TValue>(propertyName, lerp);
    }
}
