using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI.Utilities;
using StardewValley;

namespace StardewUI.Widgets.Keybinding;

/// <summary>
/// Editor widget for a <see cref="KeybindList"/>.
/// </summary>
/// <remarks>
/// Displays all the configured keybinds in one row, and (<see cref="EditableType"/> is not <c>null</c>) opens up a
/// <see cref="KeybindOverlay"/> to edit the keybinds when clicked.
/// </remarks>
/// <param name="spriteMap">Map of bindable buttons to sprite representations.</param>
/// <param name="emptyText">Text to display when the keybind is empty (has no buttons).</param>
public class KeybindListEditor(ISpriteMap<SButton> spriteMap, string emptyText) : WrapperView
{
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
            foreach (var keybindView in KeybindViews)
            {
                keybindView.ButtonHeight = value;
            }
        }
    }

    /// <summary>
    /// Specifies what kind of keybind the editor should allow.
    /// </summary>
    /// <remarks>
    /// The current value <see cref="KeybindList"/> is always fully displayed, even if it does not conform to the
    /// semantic type. It is up to the caller to ensure that the value initially assigned to the editor is of the
    /// correct kind. If this is <c>null</c>, the list is considered read-only.
    /// </remarks>
    public KeybindType? EditableType { get; set; }

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
            foreach (var keybindView in KeybindViews)
            {
                keybindView.Font = value;
            }
        }
    }

    /// <summary>
    /// The current keybinds to display in the list.
    /// </summary>
    /// <remarks>
    /// Changing these while the overlay is open may not update the overlay.
    /// </remarks>
    public KeybindList KeybindList
    {
        get => keybindList;
        set
        {
            if (value == keybindList)
            {
                return;
            }
            keybindList = value;
            UpdateAll();
        }
    }

    private IEnumerable<KeybindView> KeybindViews =>
        rootLane.Children.OfType<Frame>().Select(frame => frame.Content).OfType<KeybindView>();

    private readonly Lane rootLane = new();

    private int buttonHeight = KeybindView.DEFAULT_BUTTON_HEIGHT;
    private SpriteFont font = Game1.smallFont;
    private KeybindList keybindList = new();

    protected override IView CreateView()
    {
        rootLane.Click += RootLane_Click;
        rootLane.PointerEnter += RootLane_PointerEnter;
        rootLane.PointerLeave += RootLane_PointerLeave;
        return rootLane;
    }

    private void Overlay_Close(object? sender, EventArgs e)
    {
        if (sender is not KeybindOverlay overlay)
        {
            return;
        }
        KeybindList = overlay.KeybindList;
        // We generally won't receive the PointerLeave event after a full-screen overlay was open.
        // Known issue due to dependency on recursive PointerMove, hard to resolve.
        UpdateTint(Color.White);
    }

    private void RootLane_Click(object? sender, ClickEventArgs e)
    {
        if (EditableType is null)
        {
            return;
        }
        Game1.playSound("bigSelect");
        var overlay = new KeybindOverlay(spriteMap, emptyText) { KeybindList = KeybindList };
        overlay.Close += Overlay_Close;
        Overlay.Push(overlay);
    }

    private void RootLane_PointerEnter(object? sender, PointerEventArgs e)
    {
        if (EditableType is null)
        {
            return;
        }
        UpdateTint(Color.Orange);
    }

    private void RootLane_PointerLeave(object? sender, PointerEventArgs e)
    {
        UpdateTint(Color.White);
    }

    private void UpdateAll()
    {
        rootLane.Children = keybindList
            .Keybinds.Where(kb => kb.IsBound)
            .Select(
                (kb, index) =>
                    new Frame()
                    {
                        Layout = LayoutParameters.FitContent(),
                        Margin = index > 0 ? new Edges(Left: 16) : Edges.NONE,
                        Background = UiSprites.MenuSlotTransparent,
                        Padding = UiSprites.MenuSlotTransparent.FixedEdges! + new Edges(4),
                        Content = new KeybindView(spriteMap, emptyText)
                        {
                            ButtonHeight = buttonHeight,
                            Font = font,
                            Keybind = kb,
                        },
                    }
            )
            .Cast<IView>()
            .ToList();
    }

    private void UpdateTint(Color tintColor)
    {
        foreach (var keybindView in KeybindViews)
        {
            keybindView.TintColor = tintColor;
        }
    }
}
