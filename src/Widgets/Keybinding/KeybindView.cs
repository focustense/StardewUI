using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewModdingAPI.Utilities;
using StardewValley;

namespace StardewUI.Widgets.Keybinding;

/// <summary>
/// Display widget for a single <see cref="Keybind"/> showing all required keys.
/// </summary>
/// <param name="spriteMap">Map of bindable buttons to sprite representations.</param>
public class KeybindView(ISpriteMap<SButton> spriteMap) : WrapperView<Lane>
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
            if (IsViewCreated && Root.Children.Count == 1 && Root.Children[0] is Label label)
            {
                label.Text = value;
            }
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
        }
    }

    /// <inheritdoc cref="View.Margin" />
    public Edges Margin
    {
        get => IsViewCreated ? Root.Margin : Edges.NONE;
        set => Root.Margin = value;
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
        }
    }

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
        }
    }

    private int buttonHeight = DEFAULT_BUTTON_HEIGHT;
    private string emptyText = "";
    private SpriteFont font = Game1.smallFont;
    private Keybind keybind = new();
    private float spacing;
    private Color tintColor = Color.White;

    protected override Lane CreateView()
    {
        var lane = new Lane() { Layout = LayoutParameters.FitContent(), VerticalContentAlignment = Alignment.Middle };
        UpdateContent(lane);
        return lane;
    }

    private IView CreateButtonImage(SButton button)
    {
        var sprite = spriteMap.Get(button, out var isPlaceholder);
        var image = new Image()
        {
            Layout = new() { Width = Length.Content(), Height = Length.Px(buttonHeight) },
            Fit = isPlaceholder ? ImageFit.Stretch : ImageFit.Contain,
            Sprite = sprite,
        };
        return isPlaceholder ? FillPlaceholder(image, button) : image;
    }

    private IView FillPlaceholder(Image image, SButton button)
    {
        var label = Label.Simple(GetButtonText(button), font);
        label.Margin = (image.Sprite!.FixedEdges ?? Edges.NONE) * (image.Sprite!.SliceSettings?.Scale ?? 1);
        image.Layout = new() { Width = Length.Stretch(), Height = image.Layout.Height };
        return new Panel()
        {
            Layout = LayoutParameters.FitContent(),
            HorizontalContentAlignment = Alignment.Middle,
            VerticalContentAlignment = Alignment.Middle,
            Children = [image, label],
        };
    }

    private static string GetButtonText(SButton button)
    {
        var text = button.ToString();
        return text.StartsWith("Controller") ? text[10..] : text;
    }

    private void UpdateContent(Lane? lane = null)
    {
        lane ??= Root;
        lane.Children = keybind.IsBound
            ? keybind
                .Buttons.SelectMany(button => new IView[] { Label.Simple("+", font), CreateButtonImage(button) })
                .Skip(1)
                .ToList()
            : [Label.Simple(EmptyText, font)];
    }

    private void UpdateTint()
    {
        if (IsViewCreated)
        {
            UpdateTint(Root);
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
