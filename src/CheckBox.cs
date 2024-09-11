using Microsoft.Xna.Framework;
using StardewValley;

namespace StardewUI;

/// <summary>
/// A togglable checkbox.
/// </summary>
/// <param name="uncheckedSprite">Sprite to display when the box is unchecked, if not using the default.</param>
/// <param name="checkedSprite">Sprite to display when the box is checked, if not using the default.</param>
public class CheckBox(Sprite? uncheckedSprite = null, Sprite? checkedSprite = null) : WrapperView<Lane>
{
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
        }
    }

    /// <summary>
    /// Color with which to render any <see cref="LabelText"/>.
    /// </summary>
    public Color LabelColor
    {
        get => label?.Color ?? Color.White;
        set => RequireView(() => label).Color = value;
    }

    /// <summary>
    /// Optional label text to be displayed to the right of the checkbox image.
    /// </summary>
    /// <remarks>
    /// The label text is clickable as part of the checkbox, but does not receive focus.
    /// </remarks>
    public string LabelText
    {
        get => label?.Text ?? "";
        set
        {
            RequireView(() => label).Text = value;
            Root.Children = !string.IsNullOrEmpty(value) ? [checkImage, label] : [checkImage];
        }
    }

    private bool isChecked;

    // Initialized in CreateView
    private Image checkImage = null!;
    private Label label = null!;

    protected override Lane CreateView()
    {
        label = new Label() { Layout = LayoutParameters.FitContent(), Margin = new(Left: 12) };
        checkImage = new Image() { Layout = LayoutParameters.FitContent(), IsFocusable = true };
        UpdateCheckImage();
        var lane = new Lane()
        {
            Layout = LayoutParameters.FitContent(),
            VerticalContentAlignment = Alignment.Middle,
            Children = [checkImage],
        };
        lane.Click += Lane_Click;
        return lane;
    }

    private void Lane_Click(object? sender, ClickEventArgs e)
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
