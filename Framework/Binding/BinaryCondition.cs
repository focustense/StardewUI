using System.Reflection;
using StardewUI.Framework.Converters;
using StardewUI.Framework.Dom;
using StardewUI.Framework.Sources;

namespace StardewUI.Framework.Binding;

/// <summary>
/// A condition based on the comparison of two values.
/// </summary>
/// <remarks>
/// <para>
/// Passes whenever both values are equal. Used for <c>*switch</c> and <c>*case</c> attributes.
/// </para>
/// <para>
/// Any type may be used for either operand, but a conversion must be available from one of the types to the other type
/// in order for the condition to ever pass. If the two types are different, and conversion both ways is possible, then
/// priority will be given to the type implementing <see cref="IEquatable{T}"/> on itself; if the best type is still
/// ambiguous, then left->right conversion will be chosen over right->left.
/// </para>
/// </remarks>
public class BinaryCondition(
    IValueSourceFactory valueSourceFactory,
    IValueConverterFactory valueConverterFactory,
    IAttribute leftAttribute,
    IAttribute rightAttribute
) : ICondition
{
    object? ICondition.Context
    {
        get => RightContext;
        set => RightContext = value;
    }

    /// <summary>
    /// The context data used to derive the LHS value, if the left attribute is a context binding.
    /// </summary>
    public object? LeftContext
    {
        get => leftContext?.Data;
        set
        {
            if (value != leftContext?.Data)
            {
                leftContext = value is not null ? BindingContext.Create(value) : null;
                wasLeftContextChanged = true;
            }
        }
    }

    /// <summary>
    /// Optional source for automatically updating the <see cref="LeftContext"/>.
    /// </summary>
    /// <remarks>
    /// If specified, then this selector will be automatically run on every <see cref="Update"/> and assigned to the
    /// <see cref="LeftContext"/>; otherwise, the <see cref="LeftContext"/> must be set explicitly in order to change
    /// the evaluation context for the left-hand value.
    /// </remarks>
    public Func<object?>? LeftContextSelector { get; set; }

    /// <summary>
    /// The context data used to derive the RHS value, if the right attribute is a context binding.
    /// </summary>
    public object? RightContext
    {
        get => rightContext?.Data;
        set
        {
            if (value != rightContext?.Data)
            {
                rightContext = value is not null ? BindingContext.Create(value) : null;
                wasRightContextChanged = true;
            }
        }
    }

    /// <summary>
    /// Optional source for automatically updating the <see cref="RightContext"/>.
    /// </summary>
    /// <remarks>
    /// If specified, then this selector will be automatically run on every <see cref="Update"/> and assigned to the
    /// <see cref="RightContext"/>; otherwise, the <see cref="RightContext"/> must be set explicitly in order to change
    /// the evaluation context for the right-hand value.
    /// </remarks>
    public Func<object?>? RightContextSelector { get; set; }

    public bool Passed => isPassing;

    private IComparison? comparison;
    private bool isPassing;
    private BindingContext? leftContext;
    private IValueSource? leftValueSource;
    private BindingContext? rightContext;
    private IValueSource? rightValueSource;
    private bool wasLeftContextChanged;
    private bool wasRightContextChanged;

    public void Update()
    {
        bool anyContextChanged = false;
        if (LeftContextSelector is not null)
        {
            LeftContext = LeftContextSelector();
        }
        if (wasLeftContextChanged)
        {
            anyContextChanged = true;
            var leftValueType = valueSourceFactory.GetValueType(leftAttribute, null, leftContext);
            leftValueSource = leftValueType is not null
                ? valueSourceFactory.GetValueSource(leftAttribute, leftContext, leftValueType)
                : null;
            wasLeftContextChanged = false;
        }
        if (RightContextSelector is not null)
        {
            RightContext = RightContextSelector();
        }
        if (wasRightContextChanged)
        {
            anyContextChanged = true;
            var rightValueType = valueSourceFactory.GetValueType(rightAttribute, null, rightContext);
            rightValueSource = rightValueType is not null
                ? valueSourceFactory.GetValueSource(rightAttribute, rightContext, rightValueType)
                : null;
            wasRightContextChanged = false;
        }
        if (anyContextChanged)
        {
            comparison =
                leftValueSource is not null && rightValueSource is not null
                    ? Comparison.Create(leftValueSource, rightValueSource, valueConverterFactory)
                    : null;
            isPassing = comparison?.Evaluate(true) ?? false;
        }
        else
        {
            isPassing = comparison?.Evaluate() ?? false;
        }
    }

    interface IComparison
    {
        Type LeftType { get; }
        Type RightType { get; }

        bool Evaluate(bool force = false);
    }

    static class Comparison
    {
        delegate IComparison? Factory(IValueSource left, IValueSource right);

        private static readonly MethodInfo createFactoryDelegateMethod = typeof(Comparison).GetMethod(
            nameof(CreateFactoryDelegate),
            BindingFlags.Static | BindingFlags.NonPublic
        )!;
        private static readonly Dictionary<(Type, Type), Factory> factoryDelegates = [];

        public static IComparison? Create(
            IValueSource left,
            IValueSource right,
            IValueConverterFactory converterFactory
        )
        {
            var delegateKey = (left.ValueType, right.ValueType);
            if (!factoryDelegates.TryGetValue(delegateKey, out var factory))
            {
                factory = (Factory)
                    createFactoryDelegateMethod
                        .MakeGenericMethod(left.ValueType, right.ValueType)
                        .Invoke(null, [converterFactory])!;
                factoryDelegates.Add(delegateKey, factory);
            }
            return factory(left, right);
        }

        private static Factory CreateFactoryDelegate<T, U>(IValueConverterFactory converterFactory)
        {
            var leftToRightConverter = converterFactory.GetConverter<T, U>();
            var rightToLeftConverter = converterFactory.GetConverter<U, T>();
            if (leftToRightConverter is null && rightToLeftConverter is null)
            {
                return (_, _) => null;
            }
            if (leftToRightConverter is null || typeof(IEquatable<U>).IsAssignableFrom(typeof(U)))
            {
                return (l, r) => new Comparison<U, T>((IValueSource<U>)r, (IValueSource<T>)l, rightToLeftConverter!);
            }
            return (l, r) => new Comparison<T, U>((IValueSource<T>)l, (IValueSource<U>)r, leftToRightConverter!);
        }
    }

    // By convention, the Comparison is always "left to right". However, this can be made "right to left" by simply
    // swapping the T and U types when creating it.
    class Comparison<T, U>(
        IValueSource<T> leftValueSource,
        IValueSource<U> rightValueSource,
        IValueConverter<T, U> leftToRightConverter
    ) : IComparison
    {
        public Type LeftType => typeof(T);
        public Type RightType => typeof(U);

        private U? convertedLeftValue;
        private bool result;

        public bool Evaluate(bool force = false)
        {
            bool valuesChanged = false;
            if (leftValueSource.Update() || force)
            {
                valuesChanged = true;
                convertedLeftValue = leftValueSource.Value is not null
                    ? leftToRightConverter.Convert(leftValueSource.Value)
                    : default;
            }
            valuesChanged |= rightValueSource.Update();
            if (valuesChanged || force)
            {
                result = convertedLeftValue is IEquatable<U> equatable
                    ? equatable.Equals(rightValueSource.Value)
                    : EqualityComparer<U>.Default.Equals(convertedLeftValue, rightValueSource.Value);
            }
            return result;
        }
    }
}
