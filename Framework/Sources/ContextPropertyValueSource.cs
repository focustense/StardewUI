using System.ComponentModel;
using StardewUI.Framework.Binding;
using StardewUI.Framework.Descriptors;

namespace StardewUI.Framework.Sources;

/// <summary>
/// Value source that obtains its value from a context (or "model") property.
/// </summary>
/// <typeparam name="T">The return type of the context property.</typeparam>
public class ContextPropertyValueSource<T> : IValueSource<T>, IDisposable
    where T : notnull
{
    /// <inheritdoc />
    public bool CanRead =>
        property?.CanRead
        // We actually can "read" from an unbound context; just return a null/default value.
        ?? true;

    /// <inheritdoc />
    public bool CanWrite =>
        property?.CanWrite
        // However, we cannot meaningfully _write_ to an unbound context and should probably error on an attempt to do so.
        ?? false;

    /// <inheritdoc />
    public string DisplayName => property is not null ? $"{property.DeclaringType.Name}.{property.Name}" : "(none)";

    /// <inheritdoc />
    public T? Value
    {
        get => value;
        set
        {
            if (data is not null && property is not null && property.CanWrite)
            {
                property.SetValue(data, value!);
            }
        }
    }

    object? IValueSource.Value
    {
        get => Value;
        set => Value = value is not null ? (T)value : default;
    }

    /// <inheritdoc />
    public Type ValueType => typeof(T);

    private static readonly string oneTimeBindingTip =
        "If this was intentional, consider using a one-time binding to suppress this warning, by prefixing the "
        + "property name with the ':' character.";

    private readonly object? data;
    private readonly IPropertyDescriptor<T>? property;
    private readonly string propertyName;

    private bool isDirty = true;
    private T? value = default;

    /// <summary>
    /// Initializes a new instance of <see cref="ContextPropertyValueSource{T}"/> using the specified context and
    /// property name.
    /// </summary>
    /// <remarks>
    /// If the <see cref="BindingContext.Data"/> of the supplied <paramref name="context"/> implements
    /// <see cref="INotifyPropertyChanged"/>, then <see cref="Update"/> and <see cref="Value"/> will respond to changes
    /// to the given <paramref name="propertyName"/>. Otherwise, the source is "static" and will never change its value
    /// or return <c>true</c> from <see cref="Update"/>.
    /// </remarks>
    /// <param name="context">Context used for the data binding.</param>
    /// <param name="propertyName">Property to read on the <see cref="BindingContext.Data"/> of the supplied
    /// <paramref name="context"/> when updating.</param>
    /// <param name="allowUpdates">Whether or not to allow <see cref="Update"/> to read a new value. <c>false</c>
    /// prevents all updates and makes the source read only one time.</param>
    public ContextPropertyValueSource(BindingContext? context, string propertyName, bool allowUpdates = true)
    {
        data = context?.Data;
        property = context?.Descriptor.GetProperty(propertyName) as IPropertyDescriptor<T>;
        this.propertyName = propertyName;
        if (allowUpdates && data is not null)
        {
            if (data is INotifyPropertyChanged npc)
            {
                npc.PropertyChanged += Context_PropertyChanged;
                if (property?.IsField == true)
                {
                    Logger.LogOnce(
                        $"Binding to field '{context!.Descriptor.TargetType.Name}.{propertyName}' will receive no "
                            + "updates or inconsistent updates. To receive updates, bind to a property instead. "
                            + oneTimeBindingTip,
                        LogLevel.Warn
                    );
                }
                else if (property?.IsAutoProperty == true)
                {
                    Logger.LogOnce(
                        $"Binding to property '{data.GetType().Name}.{propertyName}' may receive no updates or "
                            + "inconsistent updates because it appears to be an auto-implemented property, and will "
                            + "not emit PropertyChanged events even if the declaring type implements "
                            + "INotifyPropertyChanged. "
                            + oneTimeBindingTip,
                        LogLevel.Warn
                    );
                }
            }
            else
            {
                Logger.LogOnce(
                    $"Binding to property '{context!.Descriptor.TargetType.Name}.{propertyName}' will not receive "
                        + "updates because the type does not implement INotifyPropertyChanged. "
                        + oneTimeBindingTip,
                    LogLevel.Warn
                );
            }
        }
    }

    /// <inheritdoc />
    public void Dispose()
    {
        if (data is INotifyPropertyChanged npc)
        {
            npc.PropertyChanged -= Context_PropertyChanged;
        }
        GC.SuppressFinalize(this);
    }

    private void Context_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == propertyName)
        {
            isDirty = true;
        }
    }

    /// <inheritdoc />
    public bool Update(bool force = false)
    {
        if (!isDirty && !force)
        {
            return false;
        }
        value = property is not null ? property.GetValue(data!) : default;
        isDirty = false;
        return true;
    }
}
