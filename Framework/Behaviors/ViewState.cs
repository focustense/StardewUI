using System.Diagnostics;
using StardewUI.Framework.Descriptors;

namespace StardewUI.Framework.Behaviors;

/// <summary>
/// View state manager based on the view's runtime descriptor and defaults.
/// </summary>
/// <param name="viewDescriptor">Descriptor for the managed view type, providing property accessors.</param>
/// <param name="viewDefaults">Default values for the managed view type.</param>
public class ViewState(IViewDescriptor viewDescriptor, IViewDefaults viewDefaults) : IViewState
{
    private readonly Dictionary<string, IPropertyEntry> properties = new(StringComparer.OrdinalIgnoreCase);

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
    public void Write(IView view)
    {
        foreach (var property in properties.Values)
        {
            property.Write(view);
        }
    }

    interface IPropertyEntry
    {
        void Write(IView view);
    }

    class PropertyEntry<T>(IPropertyDescriptor<T> descriptor, IPropertyStates<T> states, Func<T> defaultValue)
        : IPropertyEntry
    {
        public IPropertyStates<T> States => states;

        private T? lastValue;

        public void Write(IView view)
        {
            var value = States.TryPeek(out var stateValue) ? stateValue : defaultValue();
            if (!EqualityComparer<T>.Default.Equals(value, lastValue))
            {
                descriptor.SetValue(view, value);
                lastValue = value;
            }
        }
    }
}
