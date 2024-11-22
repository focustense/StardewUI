﻿using System.Collections.Concurrent;
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
    /// Creates a value source that supplies values of a given type according to the specified argument binding.
    /// </summary>
    /// <param name="type">The type of value to obtain; can be determined using
    /// <see cref="GetValueType(IArgument, BindingContext?)"/>.</param>
    /// <param name="argument">The parsed markup argument containing the binding info.</param>
    /// <param name="context">The binding context to use for any contextual bindings (those with
    /// <see cref="ArgumentExpressionType.ContextBinding"/>).</param>
    IValueSource GetValueSource(Type type, IArgument argument, BindingContext? context);

    /// <summary>
    /// Creates a value source that supplies values of a given type according to the specified binding attribute.
    /// </summary>
    /// <param name="type">The type of value to obtain; can be determined using
    /// <see cref="GetValueType(IAttribute, IPropertyDescriptor?, BindingContext?)"/>.</param>
    /// <param name="attribute">The parsed markup attribute containing the binding info.</param>
    /// <param name="context">The binding context to use for any contextual bindings (those with
    /// <see cref="ArgumentExpressionType.ContextBinding"/>).</param>
    /// <param name="scope">Scope for resolving externalized attributes, such as translation keys.</param>
    IValueSource GetValueSource(Type type, IAttribute attribute, BindingContext? context, IResolutionScope scope);

    /// <summary>
    /// Creates a value source that supplies values according to the specified argument binding.
    /// </summary>
    /// <typeparam name="T">Type of value to obtain; same as the result of
    /// <see cref="GetValueType(IArgument, BindingContext?)"/>.</typeparam>
    /// <param name="argument">The parsed markup argument containing the binding info.</param>
    /// <param name="context">The binding context to use for any contextual bindings (those with
    /// <see cref="AttributeValueType.InputBinding"/>, <see cref="AttributeValueType.OneTimeBinding"/>,
    /// <see cref="AttributeValueType.OutputBinding"/> or <see cref="AttributeValueType.TwoWayBinding"/>).</param>
    IValueSource<T> GetValueSource<T>(IArgument argument, BindingContext? context);

    /// <summary>
    /// Creates a value source that supplies values according to the specified binding attribute.
    /// </summary>
    /// <typeparam name="T">Type of value to obtain; same as the result of
    /// <see cref="GetValueType(IAttribute, IPropertyDescriptor?, BindingContext?)"/>.</typeparam>
    /// <param name="attribute">The parsed markup attribute containing the binding info.</param>
    /// <param name="context">The binding context to use for any contextual bindings (those with
    /// <see cref="AttributeValueType.InputBinding"/>, <see cref="AttributeValueType.OneTimeBinding"/>,
    /// <see cref="AttributeValueType.OutputBinding"/> or <see cref="AttributeValueType.TwoWayBinding"/>).</param>
    /// <param name="scope">Scope for resolving externalized attributes, such as translation keys.</param>
    IValueSource<T> GetValueSource<T>(IAttribute attribute, BindingContext? context, IResolutionScope scope);

    /// <summary>
    /// Determines the type of value that will be supplied by a given argument binding, and with the specified context.
    /// </summary>
    /// <remarks>
    /// This provides the type argument that must be supplied to
    /// <see cref="GetValueSource(Type, IArgument, BindingContext?)"/>.
    /// </remarks>
    /// <param name="argument">The parsed markup argument containing the binding info.</param>
    /// <param name="context">The binding context to use for any contextual bindings (those with
    /// <see cref="ArgumentExpressionType.ContextBinding"/>).</param>
    Type? GetValueType(IArgument argument, BindingContext? context);

    /// <summary>
    /// Determines the type of value that will be supplied by a given attribute binding, and with the specified context.
    /// </summary>
    /// <remarks>
    /// This provides the type argument that must be supplied to
    /// <see cref="GetValueSource{T}(IAttribute, BindingContext?, IResolutionScope)"/>.
    /// </remarks>
    /// <param name="attribute">The parsed markup attribute containing the binding info.</param>
    /// <param name="property">Binding metadata for the destination property; used when the source does not encode any
    /// independent type information. If not specified, some attribute values may be unsupported.</param>
    /// <param name="context">The binding context to use for any contextual bindings (those with
    /// <see cref="AttributeValueType.InputBinding"/>, <see cref="AttributeValueType.OneTimeBinding"/>,
    /// <see cref="AttributeValueType.OutputBinding"/> or <see cref="AttributeValueType.TwoWayBinding"/>).</param>
    Type? GetValueType(IAttribute attribute, IPropertyDescriptor? property, BindingContext? context);
}

/// <summary>
/// Default implementation of the <see cref="IValueSourceFactory"/> supporting all binding types.
/// </summary>
/// <param name="assetCache">The current asset cache, for any asset-scoped bindings.</param>
public class ValueSourceFactory(IAssetCache assetCache) : IValueSourceFactory
{
    private static readonly MethodInfo getArgumentValueSourceMethod = typeof(ValueSourceFactory).GetMethod(
        nameof(GetValueSource),
        1,
        [typeof(IArgument), typeof(BindingContext)]
    )!;
    private static readonly MethodInfo getAttributeValueSourceMethod = typeof(ValueSourceFactory).GetMethod(
        nameof(GetValueSource),
        1,
        [typeof(IAttribute), typeof(BindingContext), typeof(IResolutionScope)]
    )!;

    private readonly ConcurrentDictionary<Type, Func<IArgument, BindingContext?, IValueSource>> argumentCache = [];
    private readonly ConcurrentDictionary<
        Type,
        Func<IAttribute, BindingContext?, IResolutionScope, IValueSource>
    > attributeCache = [];

    /// <summary>
    /// Pre-initializes some reflection state in order to make future invocations faster.
    /// </summary>
    internal void Warmup()
    {
        // The .NET runtime caches some aspects of MakeGenericMethod and MakeGenericType, so there tends to be a large
        // hit the very first time they are called for a given method/type definition, and much smaller afterward.
        // It's still important to cache the closed generics, but making the first call early on can reduce jank on
        // UI frames later.
        GetArgumentValueSourceDelegate(typeof(object));
        GetAttributeValueSourceDelegate(typeof(object));
    }

    /// <inheritdoc />
    public IValueSource GetValueSource(Type type, IArgument argument, BindingContext? context)
    {
        using var _ = Trace.Begin(this, nameof(GetValueSource) + "#Arg");
        var valueSourceDelegate = GetArgumentValueSourceDelegate(type);
        return valueSourceDelegate(argument, context);
    }

    /// <inheritdoc />
    public IValueSource<T> GetValueSource<T>(IArgument argument, BindingContext? context)
    {
        using var _ = Trace.Begin(this, nameof(GetValueSource) + "<T>#Arg");
        return argument.Type switch
        {
            ArgumentExpressionType.Literal => (IValueSource<T>)new ConstantValueSource<string>(argument.Expression),
            ArgumentExpressionType.ContextBinding => new ContextPropertyValueSource<T>(
                context?.Redirect(argument.ContextRedirect),
                argument.Expression,
                false
            ),
            _ => throw new ArgumentException(
                $"Invalid or unsupported argument type {argument.Type}.",
                nameof(argument)
            ),
        };
    }

    /// <inheritdoc />
    public IValueSource GetValueSource(
        Type type,
        IAttribute attribute,
        BindingContext? context,
        IResolutionScope resolutionScope
    )
    {
        using var _ = Trace.Begin(this, nameof(GetValueSource) + "#Prop");
        var valueSourceDelegate = GetAttributeValueSourceDelegate(type);
        return valueSourceDelegate(attribute, context, resolutionScope);
    }

    /// <inheritdoc />
    public IValueSource<T> GetValueSource<T>(
        IAttribute attribute,
        BindingContext? context,
        IResolutionScope resolutionScope
    )
    {
        using var _ = Trace.Begin(this, nameof(GetValueSource) + "<T>#Prop");
        return attribute.ValueType switch
        {
            AttributeValueType.Literal => (IValueSource<T>)new ConstantValueSource<string>(attribute.Value),
#nullable disable
            // Nullable constraint here emanates from SMAPI's Content.Load<T>, which is not very helpful to us here
            // because it only matters for the return value, and for attributes, nullness only matters in the context of
            // the particular binding/conversion happening.
            AttributeValueType.AssetBinding => new AssetValueSource<T>(assetCache, attribute.Value),
#nullable enable
            AttributeValueType.TranslationBinding => (IValueSource<T>)
                new TranslationValueSource(resolutionScope, attribute.Value),
            AttributeValueType.InputBinding
            or AttributeValueType.OneTimeBinding
            or AttributeValueType.OutputBinding
            or AttributeValueType.TwoWayBinding => new ContextPropertyValueSource<T>(
                context?.Redirect(attribute.ContextRedirect),
                attribute.Value,
                attribute.ValueType != AttributeValueType.OneTimeBinding
                    && attribute.ValueType != AttributeValueType.OutputBinding
            ),
            _ => throw new ArgumentException($"Invalid attribute type {attribute.ValueType}.", nameof(attribute)),
        };
    }

    /// <inheritdoc />
    public Type? GetValueType(IArgument argument, BindingContext? context)
    {
        using var _ = Trace.Begin(this, nameof(GetValueType) + "#Arg");
        return argument.Type switch
        {
            ArgumentExpressionType.Literal => typeof(string),
            ArgumentExpressionType.ContextBinding => context
                ?.Redirect(argument.ContextRedirect)
                ?.Descriptor.GetProperty(argument.Expression)
                .ValueType,
            _ => throw new ArgumentException(
                $"Invalid or unsupported argument type {argument.Type}.",
                nameof(argument)
            ),
        };
    }

    /// <inheritdoc />
    public Type? GetValueType(IAttribute attribute, IPropertyDescriptor? property, BindingContext? context)
    {
        using var _ = Trace.Begin(this, nameof(GetValueType) + "#Prop");
        return attribute.ValueType switch
        {
            AttributeValueType.Literal or AttributeValueType.TranslationBinding => typeof(string),
            AttributeValueType.AssetBinding => property?.ValueType,
            AttributeValueType.InputBinding
            or AttributeValueType.OneTimeBinding
            or AttributeValueType.OutputBinding
            or AttributeValueType.TwoWayBinding => context
                ?.Redirect(attribute.ContextRedirect)
                ?.Descriptor.GetProperty(attribute.Value)
                .ValueType,
            _ => throw new ArgumentException($"Invalid attribute type {attribute.ValueType}.", nameof(attribute)),
        };
    }

    private Func<IArgument, BindingContext?, IValueSource> GetArgumentValueSourceDelegate(Type type)
    {
        return argumentCache.GetOrAdd(
            type,
            _ =>
            {
                var typedMethod = getArgumentValueSourceMethod.MakeGenericMethod(type);
                return typedMethod.CreateDelegate<Func<IArgument, BindingContext?, IValueSource>>(this);
            }
        );
    }

    private Func<IAttribute, BindingContext?, IResolutionScope, IValueSource> GetAttributeValueSourceDelegate(Type type)
    {
        return attributeCache.GetOrAdd(
            type,
            _ =>
            {
                var typedMethod = getAttributeValueSourceMethod.MakeGenericMethod(type);
                return typedMethod.CreateDelegate<Func<IAttribute, BindingContext?, IResolutionScope, IValueSource>>(
                    this
                );
            }
        );
    }
}
