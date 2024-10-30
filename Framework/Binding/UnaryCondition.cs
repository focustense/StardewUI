using StardewUI.Framework.Converters;
using StardewUI.Framework.Dom;
using StardewUI.Framework.Sources;

namespace StardewUI.Framework.Binding;

/// <summary>
/// A condition based on a single value that is convertible to a <see cref="bool"/>.
/// </summary>
/// <remarks>
/// Passes whenever the value's boolean representation is <c>true</c>. Used for <c>*if</c> attributes.
/// </remarks>
/// <param name="valueSourceFactory">The factory responsible for creating <see cref="IValueSource{T}"/> instances from
/// attribute data.</param>
/// <param name="valueConverterFactory">The factory responsible for creating
/// <see cref="IValueConverter{TSource, TDestination}"/> instances, used to convert bound values to the types required
/// by the target view.</param>
/// <param name="attribute">The attribute containing the conditional expression.</param>
public class UnaryCondition(
    IValueSourceFactory valueSourceFactory,
    IValueConverterFactory valueConverterFactory,
    IAttribute attribute
) : ICondition
{
    private static readonly IValueSource<bool> defaultSource = new ConstantValueSource<bool>(false);

    /// <inheritdoc />
    public BindingContext? Context
    {
        get => context;
        set
        {
            if (value != context)
            {
                context = value;
                wasContextChanged = true;
            }
        }
    }

    /// <inheritdoc />
    public bool Passed { get; private set; }

    private BindingContext? context;
    private IValueSource<bool> valueSource = defaultSource;
    private bool wasContextChanged;

    /// <inheritdoc />
    public void Dispose()
    {
        if (valueSource is IDisposable disposable)
        {
            disposable.Dispose();
        }
        Passed = false;
        context = null;
        wasContextChanged = false;
        GC.SuppressFinalize(this);
    }

    /// <inheritdoc />
    public void Update()
    {
        using var _ = Trace.Begin(this, nameof(Update));
        if (wasContextChanged)
        {
            var originalValueType = valueSourceFactory.GetValueType(attribute, null, context);
            var originalValueSource = originalValueType is not null
                ? valueSourceFactory.GetValueSource(attribute, context, originalValueType)
                : null;
            valueSource = originalValueSource is not null
                ? ConvertedValueSource.Create<bool>(originalValueSource, valueConverterFactory)
                : defaultSource;
            wasContextChanged = false;
        }
        valueSource.Update();
        Passed = valueSource.Value;
    }
}
