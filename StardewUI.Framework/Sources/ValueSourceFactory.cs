using StardewUI.Framework.Binding;
using StardewUI.Framework.Content;
using StardewUI.Framework.Descriptors;
using StardewUI.Framework.Grammar;

namespace StardewUI.Framework.Sources;

/// <summary>
/// Provides methods to look up runtime value types and build appropriate sources based on their binding information.
/// </summary>
public interface IValueSourceFactory
{
    /// <summary>
    /// Creates a value source that supplies values according to the specified binding attribute.
    /// </summary>
    /// <typeparam name="T">Type of value to obtain; same as the result of <see cref="GetValueType"/>.</typeparam>
    /// <param name="attribute">The parsed markup attribute containing the binding info.</param>
    /// <param name="context">The binding context to use for any contextual bindings (those with
    /// <see cref="AttributeValueType.Binding"/> that are not asset bindings).</param>
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
    /// <param name="property">Binding metadata for the destination property. Used when the source does not encode any
    /// independent type information.</param>
    /// <param name="context">The binding context to use for any contextual bindings (those with
    /// <see cref="AttributeValueType.Binding"/> that are not asset bindings).</param>
    Type GetValueType(IAttribute attribute, IPropertyDescriptor property, BindingContext? context);
}

/// <summary>
/// Default implementation of the <see cref="IValueSourceFactory"/> supporting all binding types.
/// </summary>
/// <param name="assetCache">The current asset cache, for any asset-scoped bindings.</param>
public class ValueSourceFactory(IAssetCache assetCache) : IValueSourceFactory
{
    public IValueSource<T> GetValueSource<T>(IAttribute attribute, BindingContext? context)
        where T : notnull
    {
        return attribute.ValueType switch
        {
            AttributeValueType.Literal => (IValueSource<T>)new LiteralValueSource(attribute.Value),
            AttributeValueType.Binding => attribute.Value.StartsWith('@')
                ? new AssetValueSource<T>(assetCache, attribute.Value[1..])
                : new ContextPropertyValueSource<T>(context, attribute.Value),
            _ => throw new ArgumentException($"Invalid attribute type {attribute.ValueType}.", nameof(attribute)),
        };
    }

    public Type GetValueType(IAttribute attribute, IPropertyDescriptor property, BindingContext? context)
    {
        return attribute.ValueType switch
        {
            AttributeValueType.Literal => typeof(string),
            AttributeValueType.Binding => attribute.Value.StartsWith('@')
                // For now, assume that asset types must exactly match the property type.
                // SMAPI's content pipeline makes it a challenge to cleanly associate an asset name with a type.
                ? property.ValueType
                : context?.Descriptor.GetProperty(attribute.Value).ValueType ?? property.ValueType,
            _ => throw new ArgumentException($"Invalid attribute type {attribute.ValueType}.", nameof(attribute)),
        };
    }
}
