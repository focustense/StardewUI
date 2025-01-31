using System.Diagnostics;
using StardewUI.Framework.Descriptors;

namespace StardewUI.Framework.Behaviors;

/// <summary>
/// View state manager based on the view's runtime descriptor and defaults.
/// </summary>
/// <param name="viewDescriptor">Descriptor for the managed view type, providing property accessors.</param>
/// <param name="viewDefaults">Default values for the managed view type.</param>
/// <param name="previousState">The previous state from which to restore the transient property values.</param>
public class ViewState(IViewDescriptor viewDescriptor, IViewDefaults viewDefaults, ViewState? previousState = null)
    : IViewState
{
    /// <inheritdoc />
    public event EventHandler<FlagEventArgs>? FlagChanged;

    private readonly Dictionary<string, object> flags = previousState is not null ? new(previousState.flags) : [];
    private readonly Dictionary<string, IPropertyEntry> properties = previousState is not null
        ? previousState.properties.ToDictionary(p => p.Key, p => p.Value.WithDefaults(viewDefaults))
        : new(StringComparer.OrdinalIgnoreCase);

    /// <inheritdoc />
    public T GetDefaultValue<T>(string propertyName)
    {
        return viewDefaults.GetDefaultValue<T>(propertyName);
    }

    /// <inheritdoc />
    public T? GetFlag<T>(string name)
    {
        return flags.TryGetValue(name, out var value) ? (T)value : default;
    }

    /// <inheritdoc />
    public IPropertyStates<T> GetOrAddProperty<T>(string propertyName)
    {
        if (!properties.TryGetValue(propertyName, out var entry))
        {
            var descriptor = (IPropertyDescriptor<T>)viewDescriptor.GetProperty(propertyName);
            entry = new PropertyEntry<T>(
                descriptor,
                new PropertyStateList<T>(),
                () => viewDefaults.GetDefaultValue<T>(propertyName)
            );
            properties.Add(propertyName, entry);
        }
        return ((PropertyEntry<T>)entry).States;
    }

    /// <inheritdoc />
    public IPropertyStates<T>? GetProperty<T>(string propertyName)
    {
        return (properties.GetValueOrDefault(propertyName) as PropertyEntry<T>)?.States;
    }

    /// <inheritdoc />
    public void SetFlag(string name, object? value)
    {
        if (value is null && flags.Remove(name))
        {
            FlagChanged?.Invoke(this, new(name));
        }
        else if (value is not null && (!flags.TryGetValue(name, out var oldValue) || !oldValue.Equals(value)))
        {
            flags[name] = value;
            FlagChanged?.Invoke(this, new(name));
        }
    }

    /// <inheritdoc />
    public void Write(IView view)
    {
        foreach (var property in properties.Values)
        {
            property.Write(view);
        }
    }

    interface IPropertyEntry
    {
        IPropertyEntry WithDefaults(IViewDefaults newDefaults);
        void Write(IView view);
    }

    class PropertyEntry<T>(IPropertyDescriptor<T> descriptor, IPropertyStates<T> states, Func<T> defaultValue)
        : IPropertyEntry
    {
        public IPropertyStates<T> States => states;

        private string? lastStateName;
        private T? lastValue;

        public IPropertyEntry WithDefaults(IViewDefaults viewDefaults)
        {
            return new PropertyEntry<T>(descriptor, states, () => viewDefaults.GetDefaultValue<T>(descriptor.Name));
        }

        public void Write(IView view)
        {
            var (stateName, value) = States.TryPeek(out var state) ? state : (null, defaultValue());
            if (!EqualityComparer<T>.Default.Equals(value, lastValue) || stateName != lastStateName)
            {
                descriptor.SetValue(view, value);
                lastStateName = stateName;
                lastValue = value;
            }
        }
    }
}
