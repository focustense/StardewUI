using System.Reflection;
using StardewUI.Framework.Binding;
using StardewUI.Framework.Content;
using StardewUI.Framework.Descriptors;
using StardewUI.Framework.Dom;
using StardewUI.Framework.Grammar;

namespace StardewUI.Framework.Sources;

/// <summary>
/// Provides methods to look up runtime value types and build appropriate sources based on their binding information.
/// </summary>
public interface IValueSourceFactory
{
    /// <summary>
    /// Creates a value source that supplies values of a given type according to the specified binding attribute.
    /// </summary>
    /// <param name="attribute">The parsed markup attribute containing the binding info.</param>
    /// <param name="context">The binding context to use for any contextual bindings (those with
    /// <param name="type">The type of value to obtain; can be determined using <see cref="GetValueType"/>.</param>
    IValueSource GetValueSource(IAttribute attribute, BindingContext? context, Type type);

    /// <summary>
    /// Creates a value source that supplies values according to the specified binding attribute.
    /// </summary>
    /// <typeparam name="T">Type of value to obtain; same as the result of <see cref="GetValueType"/>.</typeparam>
    /// <param name="attribute">The parsed markup attribute containing the binding info.</param>
    /// <param name="context">The binding context to use for any contextual bindings (those with
    /// <see cref="AttributeValueType.InputBinding"/> that are not asset bindings).</param>
    IValueSource<T> GetValueSource<T>(IAttribute attribute, BindingContext? context)
        where T : notnull;

    /// <summary>
    /// Determines the type of value that will be supplied by a given binding, and with the specified context.
    /// </summary>
    /// <remarks>
    /// This provides the type argument that must be supplied to
    /// <see cref="GetValueSource{T}(IAttribute, BindingContext?)"/>.
    /// </remarks>
    /// <param name="attribute">The parsed markup attribute containing the binding info.</param>
    /// <param name="property">Binding metadata for the destination property; used when the source does not encode any
    /// independent type information. If not specified, some attribute values may be unsupported.</param>
    /// <param name="context">The binding context to use for any contextual bindings (those with
    /// <see cref="AttributeValueType.InputBinding"/> that are not asset bindings).</param>
    Type? GetValueType(IAttribute attribute, IPropertyDescriptor? property, BindingContext? context);
}

/// <summary>
/// Default implementation of the <see cref="IValueSourceFactory"/> supporting all binding types.
/// </summary>
/// <param name="assetCache">The current asset cache, for any asset-scoped bindings.</param>
public class ValueSourceFactory(IAssetCache assetCache) : IValueSourceFactory
{
    private static readonly MethodInfo getValueSourceGenericMethod = typeof(ValueSourceFactory).GetMethod(
        nameof(GetValueSource),
        1,
        [typeof(IAttribute), typeof(BindingContext)]
    )!;

    private readonly Dictionary<Type, Func<IAttribute, BindingContext?, IValueSource>> typeCache = [];

    public IValueSource GetValueSource(IAttribute attribute, BindingContext? context, Type type)
    {
        if (!typeCache.TryGetValue(type, out var valueSourceDelegate))
        {
            var typedMethod = getValueSourceGenericMethod.MakeGenericMethod(type);
            valueSourceDelegate = typedMethod.CreateDelegate<Func<IAttribute, BindingContext?, IValueSource>>(this);
            typeCache.Add(type, valueSourceDelegate);
        }
        return valueSourceDelegate(attribute, context);
    }

    public IValueSource<T> GetValueSource<T>(IAttribute attribute, BindingContext? context)
        where T : notnull
    {
        return attribute.ValueType switch
        {
            AttributeValueType.Literal => (IValueSource<T>)new ConstantValueSource<string>(attribute.Value),
            AttributeValueType.AssetBinding => new AssetValueSource<T>(assetCache, attribute.Value),
            AttributeValueType.InputBinding or AttributeValueType.OutputBinding or AttributeValueType.TwoWayBinding =>
                new ContextPropertyValueSource<T>(context, attribute.Value),
            _ => throw new ArgumentException($"Invalid attribute type {attribute.ValueType}.", nameof(attribute)),
        };
    }

    public Type? GetValueType(IAttribute attribute, IPropertyDescriptor? property, BindingContext? context)
    {
        return attribute.ValueType switch
        {
            AttributeValueType.Literal => typeof(string),
            AttributeValueType.AssetBinding => property?.ValueType,
            AttributeValueType.InputBinding or AttributeValueType.OutputBinding or AttributeValueType.TwoWayBinding =>
                context?.Descriptor.GetProperty(attribute.Value).ValueType,
            _ => throw new ArgumentException($"Invalid attribute type {attribute.ValueType}.", nameof(attribute)),
        };
    }
}
