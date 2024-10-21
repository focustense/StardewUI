using Microsoft.Xna.Framework;
using StardewUI.Events;
using StardewUI.Graphics;
using StardewUI.Layout;
using StardewValley;

namespace StardewUI.Widgets;

/// <summary>
/// A togglable checkbox.
/// </summary>
public class CheckBox : ComponentView<Lane>
{
    /// <summary>
    /// Event raised when the checked state changes.
    /// </summary>
    public event EventHandler<EventArgs>? Change;

    /// <summary>
    /// Sprite to display when the box is checked, if not using the default.
    /// </summary>
    public Sprite? CheckedSprite
    {
        get => checkedSprite;
        set
        {
            if (value == checkedSprite)
            {
                return;
            }
            checkedSprite = value;
            UpdateCheckImage();
            OnPropertyChanged(nameof(CheckedSprite));
        }
    }

    /// <summary>
    /// Whether or not the box is checked.
    /// </summary>
    public bool IsChecked
    {
        get => isChecked;
        set
        {
            if (value == isChecked)
            {
                return;
            }
            isChecked = value;
            UpdateCheckImage();
            Change?.Invoke(this, EventArgs.Empty);
            OnPropertyChanged(nameof(IsChecked));
        }
    }

    /// <summary>
    /// Color with which to render any <see cref="LabelText"/>.
    /// </summary>
    public Color LabelColor
    {
        get => label.Color;
        set
        {
            if (value != label.Color)
            {
                label.Color = value;
                OnPropertyChanged(nameof(LabelColor));
            }
        }
    }

    /// <summary>
    /// Optional label text to be displayed to the right of the checkbox image.
    /// </summary>
    /// <remarks>
    /// The label text is clickable as part of the checkbox, but does not receive focus.
    /// </remarks>
    public string LabelText
    {
        get => label.Text;
        set
        {
            if (value != label.Text)
            {
                label.Text = value;
                View.Children = !string.IsNullOrEmpty(value) ? [checkImage, label] : [checkImage];
                OnPropertyChanged(nameof(LabelText));
            }
        }
    }

    /// <summary>
    /// Sprite to display when the box is unchecked, if not using the default.
    /// </summary>
    public Sprite? UncheckedSprite
    {
        get => uncheckedSprite;
        set
        {
            if (value == uncheckedSprite)
            {
                return;
            }
            uncheckedSprite = value;
            UpdateCheckImage();
            OnPropertyChanged(nameof(UncheckedSprite));
        }
    }

    private Sprite? checkedSprite;
    private bool isChecked;
    private Sprite? uncheckedSprite;

    // Initialized in CreateView
    private Image checkImage = null!;
    private Label label = null!;

    /// <inheritdoc />
    protected override Lane CreateView()
    {
        label = new Label() { Layout = LayoutParameters.FitContent(), Margin = new(Left: 12) };
        checkImage = new Image() { Layout = LayoutParameters.FitContent(), Focusable = true };
        UpdateCheckImage();
        var lane = new Lane()
        {
            Layout = LayoutParameters.FitContent(),
            VerticalContentAlignment = Alignment.Middle,
            Children = [checkImage],
        };
        lane.LeftClick += Lane_LeftClick;
        return lane;
    }

    private void Lane_LeftClick(object? sender, ClickEventArgs e)
    {
        Game1.playSound("drumkit6");
        IsChecked = !IsChecked;
    }

    private void UpdateCheckImage()
    {
        if (checkImage is null)
        {
            return;
        }
        checkImage.Sprite = IsChecked
            ? checkedSprite ?? UiSprites.CheckboxChecked
            : uncheckedSprite ?? UiSprites.CheckboxUnchecked;
    }
}
