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
public class UnaryCondition(
    IValueSourceFactory valueSourceFactory,
    IValueConverterFactory valueConverterFactory,
    IAttribute attribute
) : ICondition
{
    private static readonly IValueSource<bool> defaultSource = new ConstantValueSource<bool>(false);

    public object? Context
    {
        get => context?.Data;
        set
        {
            if (value != context?.Data)
            {
                context = value is not null ? BindingContext.Create(value) : null;
                wasContextChanged = true;
            }
        }
    }

    public bool Passed { get; private set; }

    private BindingContext? context;
    private IValueSource<bool> valueSource = defaultSource;
    private bool wasContextChanged;

    public void Update()
    {
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
