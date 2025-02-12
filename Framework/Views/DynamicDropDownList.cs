using System.Collections;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using StardewUI.Layout;
using StardewUI.Widgets;

namespace StardewUI.Framework.Views;

/// <summary>
/// Adapter for a drop-down list with a dynamic value type, i.e. that can change the type of its option list, formatter,
/// etc. according to the current bindings.
/// </summary>
/// <remarks>
/// This class is only intended for framework use. When creating drop-down lists in custom widgets or app views, callers
/// should use the generic <see cref="DropDownList{T}"/> instead. Dynamic lists require additional overhead, and may not
/// fully sync their properties until the next update tick, unlike regular drop-down lists which update immediately.
/// </remarks>
internal class DynamicDropDownList : DecoratorView
{
    delegate IAdapter AdapterFactory();

    /// <inheritdoc cref="DropDownList{T}.Select" />
    public event EventHandler<EventArgs>? Select;

    /// <inheritdoc cref="DropDownList{T}.MaxListHeight" />
    public float? MaxListHeight
    {
        get => adapter.MaxListHeight;
        set => adapter.MaxListHeight = value;
    }

    /// <inheritdoc cref="DropDownList{T}.OptionFormat" />
    public Delegate? OptionFormat
    {
        get => adapter.OptionFormat;
        set
        {
            if (optionFormat.SetIfChanged(value))
            {
                if (adapter.IsValidFormat(value))
                {
                    adapter.OptionFormat = value;
                }
            }
        }
    }

    /// <inheritdoc cref="DropDownList{T}.OptionMaxLines" />
    public int OptionMaxLines
    {
        get => adapter.OptionMaxLines;
        set => adapter.OptionMaxLines = value;
    }

    /// <inheritdoc cref="DropDownList{T}.OptionMinWidth" />
    [Obsolete("Use SelectionFrameLayout instead.")]
    public float OptionMinWidth
    {
        get => adapter.OptionMinWidth;
        set => adapter.OptionMinWidth = value;
    }

    /// <inheritdoc cref="DropDownList{T}.Options" />
    public IEnumerable Options
    {
        get => options.Value;
        set
        {
            if (options.SetIfChanged(value))
            {
                MaybeRecreateAdapter();
                adapter.Options = value;
                if (selectedOption.IsDirty)
                {
                    TrySetSelectedOption(selectedOption.Value?.Value);
                }
            }
        }
    }

    /// <inheritdoc cref="DropDownList{T}.SelectedIndex" />
    public int SelectedIndex
    {
        get => selectedIndex.Value;
        set
        {
            if (selectedIndex.SetIfChanged(value))
            {
                adapter.SelectedIndex = value;
                selectedOption.Value = new AnyCastValue(adapter.SelectedOption);
            }
        }
    }

    /// <inheritdoc cref="DropDownList{T}.SelectedOption" />
    public IAnyCast? SelectedOption
    {
        get => selectedOption.Value;
        set
        {
            if (selectedOption.SetIfChanged(value))
            {
                TrySetSelectedOption(value?.Value);
            }
        }
    }

    /// <inheritdoc cref="DropDownList{T}.SelectionFrameLayout" />
    public LayoutParameters SelectionFrameLayout
    {
        get => adapter.SelectionFrameLayout;
        set => adapter.SelectionFrameLayout = value;
    }

    private void TrySetSelectedOption(object? value)
    {
        if (!adapter.IsValidOption(value))
        {
            return;
        }
        adapter.SelectedOption = value;
        if (adapter.SelectedIndex >= 0 || !selectedIndex.IsDirty)
        {
            selectedIndex.Value = adapter.SelectedIndex;
        }
    }

    /// <inheritdoc cref="DropDownList{T}.SelectedOptionText" />
    public string SelectedOptionText => adapter.SelectedOptionText;

    private static readonly Dictionary<Type, AdapterFactory> adapterFactoryCache = [];
    private static readonly MethodInfo createDropDownAdapterMethodDefinition = typeof(DynamicDropDownList).GetMethod(
        nameof(CreateDropDownAdapter),
        BindingFlags.Static | BindingFlags.NonPublic
    )!;
    private static readonly Dictionary<Type, Type> optionsTypeToElementTypeCache = [];

    private readonly DirtyTracker<Delegate?> optionFormat = new(null);
    private readonly DirtyTracker<IEnumerable> options = new(Array.Empty<object>());
    private readonly DirtyTracker<int> selectedIndex = new(-1);
    private readonly DirtyTracker<IAnyCast?> selectedOption = new(null);

    private IAdapter adapter;
    private bool wasAdapterRecreated;

    /// <summary>
    /// Initializes a new <see cref="DynamicDropDownList"/> instance.
    /// </summary>
    public DynamicDropDownList()
    {
        SetAdapter(CreateDropDownAdapter<object>());
    }

    /// <inheritdoc />
    public override void Dispose()
    {
        DisposeAdapterView();
        base.Dispose();
    }

    /// <inheritdoc />
    public override void OnUpdate(TimeSpan elapsed)
    {
        if (wasAdapterRecreated)
        {
            // We can't guarantee the order of property assignments within a single frame, so it's possible that our
            // lenient setters for format, selected index/option, etc. were all skipped because of a mismatched type.
            //
            // The update tick can be used as a subsequent validation, in which the lenient "insta-updates" become
            // strict, giving the benefit of useful errors but still enabling the updates to be attempted before the
            // update tick, similar to a regular dropdown.
            //
            // These assignments will be ignored if they don't change the properties of the adapter - i.e. if the
            // various properties all became self-consistent during the assignments. On the other hand, if an assignment
            // was skipped due to being an invalid type, and is still invalid, then it will finally throw here.
            adapter.OptionFormat = OptionFormat;
            SetInitialAdapterIndex(adapter);
            wasAdapterRecreated = false;
        }
        options.ResetDirty();
        optionFormat.ResetDirty();
        selectedIndex.ResetDirty();
        selectedOption.ResetDirty();

        base.OnUpdate(elapsed);
    }

    private static IAdapter CreateDropDownAdapter<T>()
        where T : notnull
    {
        var dropdown = new DropDownList<T>();
        return new Adapter<T>(dropdown);
    }

    private void DisposeAdapterView()
    {
        adapter.Select -= View_Select;
        if (adapter.View is IDisposable disposable)
        {
            disposable.Dispose();
        }
    }

    private static Type GetElementType(Type optionsType)
    {
        if (!optionsTypeToElementTypeCache.TryGetValue(optionsType, out var elementType))
        {
            elementType = optionsType.GetEnumerableElementType() ?? typeof(object);
            optionsTypeToElementTypeCache.Add(optionsType, elementType);
        }
        return elementType;
    }

    private void MaybeRecreateAdapter()
    {
        var elementType = GetElementType(Options.GetType());
        if (elementType == adapter.OptionType)
        {
            return;
        }
        var maxListHeight = adapter.MaxListHeight;
        var optionMaxLines = adapter.OptionMaxLines;
        var optionMinWidth = adapter.OptionMinWidth;
        if (!adapterFactoryCache.TryGetValue(elementType, out var factory))
        {
            factory = createDropDownAdapterMethodDefinition
                .MakeGenericMethod(elementType)
                .CreateDelegate<AdapterFactory>();
            adapterFactoryCache.Add(elementType, factory);
        }
        var newAdapter = factory();
        newAdapter.MaxListHeight = maxListHeight;
        if (newAdapter.IsValidFormat(OptionFormat))
        {
            newAdapter.OptionFormat = OptionFormat;
        }
        newAdapter.OptionMaxLines = optionMaxLines;
        newAdapter.OptionMinWidth = optionMinWidth;
        SetInitialAdapterIndex(newAdapter);
        SetAdapter(newAdapter);
    }

    [MemberNotNull(nameof(adapter))]
    private void SetAdapter(IAdapter adapter)
    {
        if (this.adapter is not null)
        {
            DisposeAdapterView();
        }
        this.adapter = adapter;
        adapter.Select += View_Select;
        adapter.PropertyChanged += View_PropertyChanged;
        View = adapter.View;
        wasAdapterRecreated = true;
    }

    private void SetInitialAdapterIndex(IAdapter adapter)
    {
        if (selectedIndex.IsDirty || !selectedOption.IsDirty)
        {
            adapter.SelectedIndex = SelectedIndex;
            selectedOption.Value = new AnyCastValue(adapter.SelectedOption);
        }
        else if (adapter.IsValidOption(SelectedOption))
        {
            adapter.SelectedOption = SelectedOption;
        }
    }

    private void View_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        OnPropertyChanged(e);
    }

    private void View_Select(object? sender, EventArgs e)
    {
        selectedIndex.Value = adapter.SelectedIndex;
        selectedIndex.ResetDirty();
        selectedOption.Value = new AnyCastValue(adapter.SelectedOption);
        selectedOption.ResetDirty();
        Select?.Invoke(this, e);
    }

    interface IAdapter
    {
        event PropertyChangedEventHandler PropertyChanged;
        event EventHandler<EventArgs> Select;

        float? MaxListHeight { get; set; }
        Delegate? OptionFormat { get; set; }
        int OptionMaxLines { get; set; }
        float OptionMinWidth { get; set; }
        IEnumerable Options { get; set; }
        Type OptionType { get; }
        LayoutParameters SelectionFrameLayout { get; set; }
        int SelectedIndex { get; set; }
        object? SelectedOption { get; set; }
        string SelectedOptionText { get; }
        IView View { get; }

        bool IsValidFormat(Delegate? format);
        bool IsValidOption(object? option);
    }

    class Adapter<T> : IAdapter
        where T : notnull
    {
        public event PropertyChangedEventHandler PropertyChanged
        {
            add => view.PropertyChanged += value;
            remove => view.PropertyChanged -= value;
        }

        public event EventHandler<EventArgs> Select
        {
            add => view.Select += value;
            remove => view.Select -= value;
        }

        public float? MaxListHeight
        {
            get => view.MaxListHeight;
            set => view.MaxListHeight = value;
        }

        public Delegate? OptionFormat
        {
            get => view.OptionFormat;
            set => view.OptionFormat = (Func<T, string>?)value;
        }

        public int OptionMaxLines
        {
            get => view.OptionMaxLines;
            set => view.OptionMaxLines = value;
        }

        [Obsolete("Use SelectionFrameLayout instead.")]
        public float OptionMinWidth
        {
            get => view.OptionMinWidth;
            set => view.OptionMinWidth = value;
        }

        public IEnumerable Options
        {
            get => view.Options;
            set => view.Options = value is IReadOnlyList<T> optionList ? optionList : value.Cast<T>().ToList();
        }

        public Type OptionType => typeof(T);

        public int SelectedIndex
        {
            get => view.SelectedIndex;
            set => view.SelectedIndex = value;
        }

        public object? SelectedOption
        {
            get => view.SelectedOption;
            set => view.SelectedOption = value is not null ? (T)value : default;
        }

        public string SelectedOptionText => view.SelectedOptionText;

        public LayoutParameters SelectionFrameLayout
        {
            get => view.SelectionFrameLayout;
            set => view.SelectionFrameLayout = value;
        }

        public IView View => view;

        private readonly DropDownList<T> view;

        public Adapter(DropDownList<T> view)
        {
            this.view = view;
        }

        public bool IsValidFormat(Delegate? format)
        {
            return format is null || format is Func<T, string>;
        }

        public bool IsValidOption(object? option)
        {
            return option is null || option is T;
        }
    }
}
