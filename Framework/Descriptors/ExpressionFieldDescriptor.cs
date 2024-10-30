using System.Linq.Expressions;
using System.Reflection;

namespace StardewUI.Framework.Descriptors;

/// <summary>
/// Implementation of a field descriptor using a compiled expression tree.
/// </summary>
/// <remarks>
/// Expression trees take a long time to compile and should only be compiled in the background, but once compiled are
/// nearly equivalent to a regular field access.
/// </remarks>
/// <typeparam name="T">The field's declaring type.</typeparam>
/// <typeparam name="TValue">The field's value type.</typeparam>
public class ExpressionFieldDescriptor<T, TValue> : IPropertyDescriptor<TValue>
{
    /// <inheritdoc />
    /// <remarks>
    /// For fields, always returns <c>true</c>.
    /// </remarks>
    public bool CanRead => true;

    /// <inheritdoc />
    public bool CanWrite => setter is not null;

    /// <inheritdoc />
    public Type DeclaringType { get; } = typeof(T);

    /// <inheritdoc />
    public string Name { get; }

    /// <inheritdoc />
    public Type ValueType { get; }

    /// <summary>
    /// Builds a new <see cref="ExpressionFieldDescriptor{T, TValue}"/> instance from the specified field.
    /// </summary>
    /// <param name="field">The reflected field.</param>
    public static ExpressionFieldDescriptor<T, TValue> Build(FieldInfo field)
    {
        var getter = BuildGetter(field);
        var canWrite = !field.IsLiteral && !field.IsInitOnly;
        var setter = canWrite ? BuildSetter(field) : null;
        return new(field, getter, setter);
    }

    private readonly Func<T, TValue> getter;
    private readonly Action<T, TValue>? setter;

    private ExpressionFieldDescriptor(FieldInfo field, Func<T, TValue> getter, Action<T, TValue>? setter)
    {
        Name = field.Name;
        ValueType = field.FieldType;
        this.getter = getter;
        this.setter = setter;
    }

    /// <inheritdoc />
    public TValue GetValue(object source)
    {
        return getter((T)source);
    }

    /// <inheritdoc />
    public void SetValue(object target, TValue value)
    {
        if (setter is null)
        {
            throw new InvalidOperationException("Field is literal or read-only.");
        }
        setter((T)target, value);
    }

    private static Func<T, TValue> BuildGetter(FieldInfo field)
    {
        var instanceParam = Expression.Parameter(typeof(T));
        return Expression.Lambda<Func<T, TValue>>(Expression.Field(instanceParam, field), instanceParam).Compile();
    }

    private static Action<T, TValue> BuildSetter(FieldInfo field)
    {
        var instanceParam = Expression.Parameter(typeof(T));
        var valueParam = Expression.Parameter(typeof(TValue));
        return Expression
            .Lambda<Action<T, TValue>>(
                Expression.Assign(Expression.Field(instanceParam, field), valueParam),
                instanceParam,
                valueParam
            )
            .Compile();
    }
}
