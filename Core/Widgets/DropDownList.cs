using Microsoft.Xna.Framework;
using StardewUI.Events;
using StardewUI.Graphics;
using StardewUI.Layout;
using StardewUI.Overlays;
using StardewValley;

namespace StardewUI.Widgets;

/// <summary>
/// Button/text field with a drop-down menu.
/// </summary>
/// <typeparam name="T">The type of list item that can be chosen.</typeparam>
public partial class DropDownList<T> : ComponentView
    where T : notnull
{
    /// <summary>
    /// Event raised when an item in the list is selected.
    /// </summary>
    public event EventHandler<EventArgs>? Select;

    /// <summary>
    /// Specifies how to format the <see cref="SelectedOption"/> in the label text.
    /// </summary>
    public Func<T, string>? OptionFormat
    {
        get => optionFormat;
        set
        {
            var newFormat = value ?? DefaultOptionFormat;
            if (newFormat != optionFormat)
            {
                optionFormat = newFormat;
                UpdateOptions();
                UpdateSelectedOption();
                OnPropertyChanged(nameof(OptionFormat));
            }
        }
    }

    /// <summary>
    /// Maximum number of lines to allow per option in the drop-down list.
    /// </summary>
    /// <remarks>
    /// Setting this allows options in the list to wrap lines instead of being truncated to one line. Applies only to
    /// the open list, and not the selected item text within the main border, which is always single-line.
    /// </remarks>
    public int OptionMaxLines
    {
        get => optionMaxLines;
        set
        {
            if (value == optionMaxLines)
            {
                return;
            }
            optionMaxLines = value;
            foreach (var optionView in optionsLane.Children.OfType<DropDownOptionView>())
            {
                optionView.MaxLines = value;
            }
            OnPropertyChanged(nameof(OptionMaxLines));
        }
    }

    /// <summary>
    /// Minimum width for the text area of an option.
    /// </summary>
    /// <remarks>
    /// If this is <c>0</c>, or if all options are larger, then the dropdown will expand horizontally.
    /// </remarks>
    [Obsolete("Use SelectionFrameLayout instead.")]
    public float OptionMinWidth
    {
        get => selectionFrame.Layout.MinWidth ?? 0;
        set
        {
            if (value != (selectionFrame.Layout.MinWidth ?? 0))
            {
                selectionFrame.Layout = new()
                {
                    Width = Length.Content(),
                    Height = Length.Content(),
                    MinWidth = value,
                };
                OnPropertyChanged(nameof(OptionMinWidth));
            }
        }
    }

    /// <summary>
    /// The options available to select.
    /// </summary>
    public IReadOnlyList<T> Options
    {
        get => options;
        set
        {
            if (options.SetItems(value))
            {
                OnPropertyChanged(nameof(Options));
                if (pendingSelectedOption is not null)
                {
                    SelectedIndex = options.IndexOf((T)pendingSelectedOption);
                    pendingSelectedOption = null;
                }
                UpdateSelectedOption();
            }
        }
    }

    /// <summary>
    /// Index of the currently-selected option in the <see cref="Options"/>, or <c>-1</c> if none selected.
    /// </summary>
    public int SelectedIndex
    {
        get => selectedIndex;
        set
        {
            var validIndex = options.Count > 0 ? Math.Clamp(value, -1, options.Count - 1) : -1;
            if (validIndex == selectedIndex)
            {
                return;
            }
            pendingSelectedOption = null;
            selectedIndex = validIndex;
            Select?.Invoke(this, EventArgs.Empty);
            OnPropertyChanged(nameof(SelectedIndex));
            OnPropertyChanged(nameof(SelectedOption));
            UpdateSelectedOption();
        }
    }

    /// <summary>
    /// The option that is currently selected, or <c>null</c> if there is no selection.
    /// </summary>
    public T? SelectedOption
    {
        get => SelectedIndex >= 0 && SelectedIndex < options.Count ? options[SelectedIndex] : default;
        set
        {
            if (
                EqualityComparer<T>.Default.Equals(value, SelectedOption)
                || (
                    pendingSelectedOption is not null
                    && EqualityComparer<T>.Default.Equals(value, (T)pendingSelectedOption)
                )
            )
            {
                return;
            }
            pendingSelectedOption = null;
            if (value is null)
            {
                SelectedIndex = -1;
                return;
            }
            var valueIndex = options.IndexOf(value);
            SelectedIndex = valueIndex;
            if (SelectedIndex < 0)
            {
                pendingSelectedOption = value;
            }
        }
    }

    /// <summary>
    /// The text of the currently-selected option.
    /// </summary>
    public string SelectedOptionText
    {
        get => selectedOptionLabel.Text;
    }

    /// <summary>
    /// Customizes the layout settings of the selection frame.
    /// </summary>
    /// <remarks>
    /// The selection frame is the frame surrounding the text of the selected item, not including the dropdown button
    /// (arrow) itself which is fixed size. By default, the selection frame matches the width of the selected item text;
    /// this can be changed, for example, to stretch to the available layout width, minus the button.
    /// </remarks>
    public LayoutParameters SelectionFrameLayout
    {
        get => selectionFrame.Layout;
        set
        {
            if (value != selectionFrame.Layout)
            {
                selectionFrame.Layout = value;
                OnPropertyChanged(nameof(SelectionFrameLayout));
            }
        }
    }

    private static readonly Func<T, string> DefaultOptionFormat = v => v.ToString() ?? "";

    private readonly DirtyTrackingList<T> options = [];

    private IOverlay? overlay;
    private Func<T, string> optionFormat = DefaultOptionFormat;
    private int optionMaxLines = 1;
    private int selectedIndex = -1;

    // Initialized in CreateView
    private Lane optionsLane = null!;
    private DropDownOverlayView overlayView = null!;
    private object? pendingSelectedOption;
    private Label selectedOptionLabel = null!;
    private Frame selectionFrame = null!;

    /// <inheritdoc />
    public override bool Measure(Vector2 availableSize)
    {
        if (options.IsDirty)
        {
            UpdateOptions();
            options.ResetDirty();
        }
        var wasDirty = base.Measure(availableSize);
        if (wasDirty)
        {
            overlayView.Layout = new LayoutParameters()
            {
                // Subtract padding from width.
                Width = Length.Px(selectionFrame.OuterSize.X - 4),
                Height = Length.Content(),
            };
        }
        return wasDirty;
    }

    /// <inheritdoc />
    protected override IView CreateView()
    {
        // Overlay
        optionsLane = new() { Layout = LayoutParameters.AutoRow(), Orientation = Orientation.Vertical };
        UpdateOptions();
        overlayView = new(this);

        // Always visible
        selectedOptionLabel = Label.Simple("");
        selectionFrame = new()
        {
            Layout = LayoutParameters.FitContent(),
            Padding = new(8, 4),
            Background = UiSprites.DropDownBackground,
            Content = selectedOptionLabel,
        };
        var buttonHeight = Game1.smallFont.LineSpacing + selectionFrame.Padding.Vertical;
        var button = new Image()
        {
            Layout = new() { Width = Length.Content(), Height = Length.Px(buttonHeight) },
            Sprite = UiSprites.DropDownButton,
            Focusable = true,
        };
        var lane = new Lane() { Layout = LayoutParameters.FitContent(), Children = [selectionFrame, button] };
        lane.LeftClick += Lane_LeftClick;
        return lane;
    }

    private void Lane_LeftClick(object? sender, ClickEventArgs e)
    {
        if (!e.IsPrimaryButton())
        {
            return;
        }
        ToggleOverlay();
    }

    private void OptionView_LeftClick(object? sender, ClickEventArgs e)
    {
        if (!e.IsPrimaryButton() || sender is not DropDownOptionView optionView)
        {
            return;
        }
        Game1.playSound("drumkit6");
        RemoveOverlay();
        SelectedOption = optionView.Value;
    }

    private void OptionView_Select(object? sender, EventArgs e)
    {
        SetSelectedOptionView(view => view == sender);
    }

    private bool RemoveOverlay()
    {
        if (overlay is null)
        {
            return false;
        }
        Overlay.Remove(overlay);
        overlay = null;
        return true;
    }

    private void SetSelectedOptionView(Predicate<DropDownOptionView> predicate)
    {
        foreach (var optionView in optionsLane.Children.OfType<DropDownOptionView>())
        {
            optionView.IsSelected = predicate(optionView);
        }
    }

    private void ToggleOverlay()
    {
        if (RemoveOverlay())
        {
            return;
        }
        var selectedOption = SelectedOption;
        SetSelectedOptionView(view => Equals(view.Value, selectedOption));
        Game1.playSound("shwip");
        overlay = new Overlay(
            overlayView,
            this,
            horizontalAlignment: Alignment.Start,
            horizontalParentAlignment: Alignment.Start,
            verticalAlignment: Alignment.Start,
            verticalParentAlignment: Alignment.End
        ).OnClose(() => overlay = null);
        Overlay.Push(overlay);
    }

    private void UpdateOptions()
    {
        optionsLane.Children = options
            .Select(
                (option, i) =>
                {
                    var optionView = new DropDownOptionView(option, optionFormat(option))
                    {
                        IsSelected = SelectedIndex == i,
                        MaxLines = optionMaxLines,
                    };
                    optionView.LeftClick += OptionView_LeftClick;
                    optionView.Select += OptionView_Select;
                    return optionView as IView;
                }
            )
            .ToList();
    }

    private void UpdateSelectedOption()
    {
        selectedOptionLabel.Text = SelectedOption is not null ? optionFormat(SelectedOption) : "";
        OnPropertyChanged(nameof(SelectedOptionText));
    }

    class DropDownOptionView(T value, string text) : ComponentView<Frame>
    {
        public event EventHandler<EventArgs>? Select;

        public bool IsSelected
        {
            get => isSelected;
            set
            {
                if (isSelected == value)
                {
                    return;
                }
                isSelected = value;
                View.BackgroundTint = GetBackgroundTint();
            }
        }

        public int MaxLines
        {
            get => label.MaxLines;
            set => label.MaxLines = value;
        }

        public T Value { get; } = value;

        private bool isSelected;
        private readonly Label label = Label.Simple(text);

        protected override Frame CreateView()
        {
            var frame = new Frame()
            {
                Layout = LayoutParameters.AutoRow(),
                Padding = new(4),
                Background = new(Game1.staminaRect),
                BackgroundTint = GetBackgroundTint(),
                Content = label,
                Focusable = true,
            };
            frame.PointerEnter += Frame_PointerEnter;
            return frame;
        }

        private void Frame_PointerEnter(object? sender, PointerEventArgs e)
        {
            IsSelected = true;
            Select?.Invoke(this, EventArgs.Empty);
        }

        private Color GetBackgroundTint()
        {
            return isSelected ? new(Color.White, 0.35f) : Color.Transparent;
        }
    }

    class DropDownOverlayView(DropDownList<T> owner) : ComponentView
    {
        public override ViewChild? GetDefaultFocusChild()
        {
            return owner
                .optionsLane.GetChildren()
                .FirstOrDefault(child => child.View is DropDownOptionView { IsSelected: true });
        }

        protected override IView CreateView()
        {
            return new Frame()
            {
                Layout = LayoutParameters.AutoRow(),
                Background = UiSprites.DropDownBackground,
                Padding = new(4),
                Content = owner.optionsLane,
            };
        }
    }
}
