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
    private static readonly MethodInfo CreateFlagStateBehaviorMethod = GetFactoryMethod(
        nameof(CreateFlagStateBehavior)
    )!;
    private static readonly MethodInfo CreateTransitionBehaviorMethod = GetFactoryMethod(
        nameof(CreateTransitionBehavior)
    )!;
    private static readonly MethodInfo CreateVisibilityBehaviorMethod = GetFactoryMethod(
        nameof(CreateVisibilityBehavior)
    )!;

    private static readonly ConcurrentDictionary<BehaviorPropertyKey, BehaviorCreator> creatorsByProperty = [];
    private static readonly ConcurrentDictionary<BehaviorViewKey, BehaviorCreator> creatorsByViewArg = [];

    private static MethodInfo? GetFactoryMethod(string name)
    {
        return typeof(StateBehaviorFactory).GetMethod(name, BindingFlags.Static | BindingFlags.NonPublic);
    }

    /// <inheritdoc />
    public bool CanCreateBehavior(string name, string argument)
    {
        // Keep in sync with GetCreatorDelegate.
        return name.Equals("hover", StringComparison.OrdinalIgnoreCase)
            || name.Equals("show", StringComparison.OrdinalIgnoreCase)
            || (name.Equals("state", StringComparison.OrdinalIgnoreCase) && argument.Contains(':'))
            || name.Equals("transition", StringComparison.OrdinalIgnoreCase);
    }

    /// <inheritdoc />
    public IViewBehavior CreateBehavior(Type viewType, string name, string argument)
    {
        var viewKey = new BehaviorViewKey(viewType, name + ':' + argument);
        var creator = creatorsByViewArg.GetOrAdd(
            viewKey,
            _ =>
            {
                var propertyName = GetPropertyName(name, argument);
                var viewDescriptor = DescriptorFactory.GetViewDescriptor(viewType);
                var propertyDescriptor = viewDescriptor.GetProperty(propertyName);
                var propertyKey = new BehaviorPropertyKey(name, propertyDescriptor.ValueType);
                return creatorsByProperty.GetOrAdd(
                    propertyKey,
                    _ => GetCreatorDelegate(propertyKey.Name, propertyKey.PropertyType)
                );
            }
        );
        return creator(argument);
    }

    private static IViewBehavior CreateFlagStateBehavior<TValue>(string argument)
    {
        var separatorIndex = argument.IndexOf(':');
        var flagName = argument[..separatorIndex];
        var propertyName = argument[(separatorIndex + 1)..].AsSpan().ToUpperCamelCase();
        return new FlagStateBehavior<TValue>(flagName, propertyName);
    }

    private static IViewBehavior CreateHoverBehavior<TValue>(string argument)
    {
        return new HoverStateBehavior<TValue>(argument.AsSpan().ToUpperCamelCase());
    }

    private static IViewBehavior CreateTransitionBehavior<TValue>(string argument)
    {
        var propertyName = argument.AsSpan().ToUpperCamelCase();
        var lerp =
            Lerps.Get<TValue>()
            ?? throw new BindingException(
                $"Cannot transition property '{propertyName}' with type {typeof(TValue).FullName} because the value type "
                    + "does not have a defined interpolation (Lerp) function."
            );
        return new TransitionBehavior<TValue>(propertyName, lerp);
    }

    private static IViewBehavior CreateVisibilityBehavior<TValue>(string argument)
    {
        return new VisibilityStateBehavior<TValue>(argument.AsSpan().ToUpperCamelCase());
    }

    private static BehaviorCreator GetCreatorDelegate(string stateName, Type valueType)
    {
        // Keep in sync with SupportsName.
        var method = stateName.ToLowerInvariant() switch
        {
            "hover" => CreateHoverBehaviorMethod.MakeGenericMethod(valueType),
            "show" => CreateVisibilityBehaviorMethod.MakeGenericMethod(valueType),
            "state" => CreateFlagStateBehaviorMethod.MakeGenericMethod(valueType),
            "transition" => CreateTransitionBehaviorMethod.MakeGenericMethod(valueType),
            _ => throw new ArgumentException($"Unrecognized state name '{stateName}'.", nameof(stateName)),
        };
        return method.CreateDelegate<BehaviorCreator>();
    }

    private static string GetPropertyName(string behaviorName, string argument)
    {
        string propertyArgument = behaviorName.Equals("state", StringComparison.OrdinalIgnoreCase)
            ? argument[(argument.IndexOf(':') + 1)..]
            : argument;
        return propertyArgument.AsSpan().ToUpperCamelCase();
    }
}
