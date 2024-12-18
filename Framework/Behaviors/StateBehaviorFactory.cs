using System.Collections.Concurrent;
using System.Reflection;
using StardewUI.Framework.Binding;
using StardewUI.Framework.Descriptors;

namespace StardewUI.Framework.Behaviors;

/// <summary>
/// Factory for creating behaviors that apply single-property overrides on state transitions, such as
/// <c>hover:transform</c>.
/// </summary>
public class StateBehaviorFactory : IBehaviorFactory
{
    private delegate IViewBehavior BehaviorCreator(string propertyName);

    private record BehaviorPropertyKey(string Name, Type PropertyType);

    private record BehaviorViewKey(Type ViewType, string ArgumentName);

    private static readonly MethodInfo CreateHoverBehaviorMethod = typeof(StateBehaviorFactory).GetMethod(
        nameof(CreateHoverBehavior),
        BindingFlags.Static | BindingFlags.NonPublic
    )!;
    private static readonly ConcurrentDictionary<BehaviorPropertyKey, BehaviorCreator> creatorsByProperty = [];
    private static readonly ConcurrentDictionary<BehaviorViewKey, BehaviorCreator> creatorsByViewArg = [];

    /// <inheritdoc />
    public IViewBehavior CreateBehavior(Type viewType, string name, string argument)
    {
        var propertyName = argument.AsSpan().ToUpperCamelCase();
        var viewKey = new BehaviorViewKey(viewType, argument);
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
        return name.Equals("hover", StringComparison.OrdinalIgnoreCase);
    }

    private static BehaviorCreator GetCreatorDelegate(string stateName, Type valueType)
    {
        // Keep in sync with SupportsName.
        return stateName.ToLowerInvariant() switch
        {
            "hover" => CreateHoverBehaviorMethod.MakeGenericMethod(valueType).CreateDelegate<BehaviorCreator>(),
            _ => throw new ArgumentException($"Unrecognized state name '{stateName}'.", nameof(stateName)),
        };
    }

    private static IViewBehavior CreateHoverBehavior<TValue>(string propertyName)
    {
        return new HoverStateBehavior<TValue>(propertyName);
    }
}
