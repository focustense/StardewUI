using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewModdingAPI.Utilities;
using StardewUI.Graphics;
using StardewUI.Input;
using StardewUI.Layout;
using StardewValley;

namespace StardewUI.Widgets.Keybinding;

/// <summary>
/// Display widget for a single <see cref="Keybind"/> showing all required keys.
/// </summary>
public class KeybindView : ComponentView<Lane>
{
    /// <summary>
    /// Default setting for <see cref="ButtonHeight"/>.
    /// </summary>
    public const int DEFAULT_BUTTON_HEIGHT = 64;

    /// <summary>
    /// The height for button images/sprites. Images are scaled uniformly, preserving source aspect ratio.
    /// </summary>
    public int ButtonHeight
    {
        get => buttonHeight;
        set
        {
            if (value == buttonHeight)
            {
                return;
            }
            buttonHeight = value;
            UpdateContent();
            OnPropertyChanged(nameof(ButtonHeight));
        }
    }

    /// <summary>
    /// Minimum width for button images/sprites, used if the layout width would be less than that implied by the
    /// <see cref="ButtonHeight"/> and placeholder content (if any).
    /// </summary>
    public int? ButtonMinWidth
    {
        get => buttonMinWidth;
        set
        {
            if (value == buttonMinWidth)
            {
                return;
            }
            buttonMinWidth = value;
            UpdateContent();
            OnPropertyChanged(nameof(ButtonMinWidth));
        }
    }

    /// <summary>
    /// Placeholder text to display when the current keybind is empty.
    /// </summary>
    public string EmptyText
    {
        get => emptyText;
        set
        {
            if (value == emptyText)
            {
                return;
            }
            emptyText = value;
            if (View?.Children.Count == 1 && View.Children[0] is Label label)
            {
                label.Text = value;
            }
            OnPropertyChanged(nameof(EmptyText));
        }
    }

    /// <summary>
    /// Font used to display text in button/key placeholders.
    /// </summary>
    /// <remarks>
    /// Only applies for buttons that use a placeholder sprite (i.e. set the <c>isPlaceholder</c> output of
    /// <see cref="ISpriteMap{T}.Get(T, out bool)"/> to <c>true</c>). In these cases, the actual button text drawn
    /// inside the sprite will be drawn using the specified font.
    /// </remarks>
    public SpriteFont Font
    {
        get => font;
        set
        {
            if (value == font)
            {
                return;
            }
            font = value;
            UpdateContent();
            OnPropertyChanged(nameof(Font));
        }
    }

    /// <summary>
    /// The current keybind.
    /// </summary>
    public Keybind Keybind
    {
        get => keybind;
        set
        {
            if (value.Buttons.SequenceEqual(keybind.Buttons))
            {
                return;
            }
            keybind = value;
            UpdateContent();
            OnPropertyChanged(nameof(Keybind));
        }
    }

    /// <inheritdoc cref="View.Margin" />
    public Edges Margin
    {
        get => View.Margin;
        set => View.Margin = value;
    }

    /// <summary>
    /// Extra spacing between displayed button sprites, if the sprites do not have implicit wide margins.
    /// </summary>
    public float Spacing
    {
        get => spacing;
        set
        {
            if (value == spacing)
            {
                return;
            }
            spacing = value;
            UpdateContent();
            OnPropertyChanged(nameof(Spacing));
        }
    }

    /// <summary>
    /// Map of bindable buttons to sprite representations.
    /// </summary>
    public ISpriteMap<SButton>? SpriteMap
    {
        get => spriteMap;
        set
        {
            if (value == spriteMap)
            {
                return;
            }
            spriteMap = value;
            UpdateContent();
            OnPropertyChanged(nameof(SpriteMap));
        }
    }

    /// <summary>
    /// Text color for the button text inside any placeholder sprites.
    /// </summary>
    public Color TextColor
    {
        get => textColor;
        set
        {
            if (value == textColor)
            {
                return;
            }
            textColor = value;
            if (View?.Children.Count == 1 && View.Children[0] is Label label)
            {
                label.Color = value;
            }
            OnPropertyChanged(nameof(TextColor));
        }
    }

    /// <summary>
    /// Color to tint the background/sprite of each key.
    /// </summary>
    public Color TintColor
    {
        get => tintColor;
        set
        {
            if (value == tintColor)
            {
                return;
            }
            tintColor = value;
            UpdateTint();
            OnPropertyChanged(nameof(TintColor));
        }
    }

    private int buttonHeight = DEFAULT_BUTTON_HEIGHT;
    private int? buttonMinWidth = null;
    private string emptyText = "";
    private SpriteFont font = Game1.smallFont;
    private Keybind keybind = new();
    private float spacing;
    private ISpriteMap<SButton>? spriteMap;
    private Color textColor = Game1.textColor;
    private Color tintColor = Color.White;

    /// <inheritdoc />
    protected override Lane CreateView()
    {
        var lane = new Lane() { Layout = LayoutParameters.FitContent(), VerticalContentAlignment = Alignment.Middle };
        UpdateContent(lane);
        return lane;
    }

    private IView CreateButtonImage(SButton button)
    {
        bool isPlaceholder = false;
        var sprite = SpriteMap?.Get(button, out isPlaceholder);
        var image = new Image()
        {
            Layout = new()
            {
                Width = Length.Content(),
                Height = Length.Px(buttonHeight),
                MinWidth = buttonMinWidth,
            },
            Fit = isPlaceholder ? ImageFit.Stretch : ImageFit.Contain,
            Sprite = sprite,
            Tint = tintColor,
        };
        return isPlaceholder ? FillPlaceholder(image, button) : image;
    }

    private IView FillPlaceholder(Image image, SButton button)
    {
        var label = Label.Simple(ButtonName.ForButton(button), font, textColor);
        label.Margin = (image.Sprite!.FixedEdges ?? Edges.NONE) * (image.Sprite!.SliceSettings?.Scale ?? 1);
        image.Layout = new()
        {
            Width = Length.Stretch(),
            Height = image.Layout.Height,
            MinWidth = buttonMinWidth,
        };
        return new Panel()
        {
            Layout = LayoutParameters.FitContent(),
            HorizontalContentAlignment = Alignment.Middle,
            VerticalContentAlignment = Alignment.Middle,
            Children = [image, label],
        };
    }

    private void UpdateContent(Lane? lane = null)
    {
        lane ??= View;
        lane.Children = keybind.IsBound
            ? keybind
                .Buttons.SelectMany(button => new IView[] { Label.Simple("+", font), CreateButtonImage(button) })
                .Skip(1)
                .ToList()
            : [Label.Simple(EmptyText, font)];
        UpdateTint(lane);
    }

    private void UpdateTint()
    {
        if (View is not null)
        {
            UpdateTint(View);
        }
    }

    private void UpdateTint(IView view)
    {
        if (view is Image image)
        {
            image.Tint = tintColor;
            return;
        }
        foreach (var child in view.GetChildren())
        {
            UpdateTint(child.View);
        }
    }
}
