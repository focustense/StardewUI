using System.Reflection;

namespace StardewUI.Framework.Descriptors;

/// <summary>
/// Helper for creating <see cref="LazyExpressionFieldDescriptor{TValue}"/> with types not known at compile time.
/// </summary>
public static class LazyExpressionFieldDescriptor
{
    private static readonly MethodInfo createMethod = typeof(LazyExpressionFieldDescriptor).GetMethod(
        nameof(Create),
        BindingFlags.Static | BindingFlags.NonPublic
    )!;
    private static readonly Dictionary<FieldInfo, IPropertyDescriptor> descriptorCache = [];

    /// <summary>
    /// Creates a binding field from reflected field info.
    /// </summary>
    /// <param name="fieldInfo">The reflected field info.</param>
    /// <returns>
    /// A binding field of type <see cref="LazyExpressionFieldDescriptor{TValue}"/>, whose generic argument is the
    /// field's <see cref="FieldInfo.FieldType"/>.
    /// </returns>
    public static IPropertyDescriptor FromFieldInfo(FieldInfo fieldInfo)
    {
        using var _ = Trace.Begin(nameof(LazyExpressionFieldDescriptor), nameof(FromFieldInfo));
        if (!descriptorCache.TryGetValue(fieldInfo, out var descriptor))
        {
            if (fieldInfo.DeclaringType is null)
            {
                throw new ArgumentException($"Field '{fieldInfo.Name}' is missing a declaring type.");
            }
            var factoryMethod = createMethod.MakeGenericMethod(fieldInfo.DeclaringType, fieldInfo.FieldType);
            descriptor = (IPropertyDescriptor)factoryMethod.Invoke(null, [fieldInfo])!;
            descriptorCache.Add(fieldInfo, descriptor);
        }
        return descriptor;
    }

    private static IPropertyDescriptor Create<T, TValue>(FieldInfo fieldInfo)
    {
        var reflectionDescriptor = new ReflectionFieldDescriptor<TValue>(fieldInfo);
        var expressionDescriptorTask = Task.Run(
            () => ExpressionFieldDescriptor<T, TValue>.Build(fieldInfo) as IPropertyDescriptor<TValue>
        );
        return new LazyExpressionFieldDescriptor<TValue>(reflectionDescriptor, expressionDescriptorTask);
    }
}

/// <summary>
/// Implementation of a field descriptor that supports a transition between two inner descriptor types.
/// </summary>
/// <remarks>
/// Designed to initially use a "slow" descriptor that is poorly optimized for access times, but is available
/// immediately, and then transition to a "fast" descriptor that is created asynchronously and slowly, but is better
/// optimized for frequent access.
/// </remarks>
/// <typeparam name="TValue">The field's value type.</typeparam>
public class LazyExpressionFieldDescriptor<TValue> : IPropertyDescriptor<TValue>
{
    private IPropertyDescriptor<TValue> descriptor;

    /// <summary>
    /// Initializes a new <see cref="LazyExpressionFieldDescriptor{TValue}"/> instance.
    /// </summary>
    /// <param name="slowDescriptor">The slower but immediately-available descriptor to use initially; typically an
    /// instance of <see cref="ReflectionFieldDescriptor{TValue}"/>.</param>
    /// <param name="fastDescriptorTask">The faster, deferred descriptor to use once available; typically an instance of
    /// <see cref="ExpressionFieldDescriptor{T, TValue}"/>.</param>
    public LazyExpressionFieldDescriptor(
        IPropertyDescriptor<TValue> slowDescriptor,
        Task<IPropertyDescriptor<TValue>> fastDescriptorTask
    )
    {
        descriptor = slowDescriptor;
        fastDescriptorTask.ContinueWith(t => descriptor = t.Result);
    }

    /// <inheritdoc />
    /// <remarks>
    /// For fields, always returns <c>true</c>.
    /// </remarks>
    public bool CanRead => true;

    /// <inheritdoc />
    public bool CanWrite => descriptor.CanWrite;

    /// <inheritdoc />
    public Type DeclaringType => throw new NotImplementedException();

    /// <inheritdoc />
    public bool IsAutoProperty => false;

    /// <inheritdoc />
    public bool IsField => true;

    /// <inheritdoc />
    public string Name => throw new NotImplementedException();

    /// <inheritdoc />
    public Type ValueType => descriptor.ValueType;

    /// <inheritdoc />
    public TValue GetValue(object source)
    {
        return descriptor.GetValue(source);
    }

    /// <inheritdoc />
    public void SetValue(object target, TValue value)
    {
        descriptor.SetValue(target, value);
    }
}
