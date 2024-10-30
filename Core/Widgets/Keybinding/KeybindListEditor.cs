using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewModdingAPI.Utilities;
using StardewUI.Events;
using StardewUI.Graphics;
using StardewUI.Layout;
using StardewUI.Overlays;
using StardewValley;

namespace StardewUI.Widgets.Keybinding;

/// <summary>
/// Editor widget for a <see cref="KeybindList"/>.
/// </summary>
/// <remarks>
/// Displays all the configured keybinds in one row, and (<see cref="EditableType"/> is not <c>null</c>) opens up a
/// <see cref="KeybindOverlay"/> to edit the keybinds when clicked.
/// </remarks>
public class KeybindListEditor : ComponentView
{
    /// <summary>
    /// The <see cref="KeybindOverlay.AddButtonText"/> to display for adding new bindings.
    /// </summary>
    /// <remarks>
    /// Changing this while the overlay is already displayed will not update the overlay.
    /// </remarks>
    public string AddButtonText
    {
        get => addButtonText;
        set
        {
            if (value != addButtonText)
            {
                addButtonText = value;
                OnPropertyChanged(nameof(AddButtonText));
            }
        }
    }

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
            OnPropertyChanged(nameof(ButtonHeight));
        }
    }

    /// <summary>
    /// The <see cref="KeybindOverlay.DeleteButtonTooltip"/> to display for deleting bindings in a multiple-binding UI.
    /// </summary>
    /// <remarks>
    /// Changing this while the overlay is already displayed will not update the overlay.
    /// </remarks>
    public string DeleteButtonTooltip
    {
        get => deleteButtonTooltip;
        set
        {
            if (value != deleteButtonTooltip)
            {
                deleteButtonTooltip = value;
                OnPropertyChanged(nameof(DeleteButtonTooltip));
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
    public KeybindType? EditableType
    {
        get => editableType;
        set
        {
            if (value != editableType)
            {
                editableType = value;
                OnPropertyChanged(nameof(EditableType));
            }
        }
    }

    /// <summary>
    /// Placeholder text to display when the current keybind list is empty.
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
            UpdateEmptyText(EmptyTextColor);
            OnPropertyChanged(nameof(EmptyText));
        }
    }

    /// <summary>
    /// Color of the displayed <see cref="EmptyText"/> when the list is empty.
    /// </summary>
    public Color EmptyTextColor
    {
        get => emptyTextColor;
        set
        {
            if (value == emptyTextColor)
            {
                return;
            }
            emptyTextColor = value;
            UpdateEmptyText(EmptyTextColor);
            OnPropertyChanged(nameof(EmptyTextColor));
        }
    }

    /// <inheritdoc cref="View.Focusable" />
    public bool Focusable
    {
        get => rootLane.Focusable;
        set => rootLane.Focusable = value;
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
            foreach (var keybindView in KeybindViews)
            {
                keybindView.Font = value;
            }
            OnPropertyChanged(nameof(Font));
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
            OnPropertyChanged(nameof(KeybindList));
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
            foreach (var keybindView in KeybindViews)
            {
                keybindView.SpriteMap = value;
            }
            OnPropertyChanged(nameof(SpriteMap));
        }
    }

    private IEnumerable<KeybindView> KeybindViews =>
        rootLane.Children.OfType<Frame>().Select(frame => frame.Content).OfType<KeybindView>();

    private readonly Lane rootLane = new();

    private string addButtonText = "";
    private int buttonHeight = KeybindView.DEFAULT_BUTTON_HEIGHT;
    private string deleteButtonTooltip = "";
    private KeybindType? editableType;
    private string emptyText = "";
    private Color emptyTextColor = Game1.textColor;
    private SpriteFont font = Game1.smallFont;
    private KeybindList keybindList = new();
    private ISpriteMap<SButton>? spriteMap;

    /// <inheritdoc />
    protected override IView CreateView()
    {
        rootLane.LeftClick += RootLane_LeftClick;
        rootLane.RightClick += RootLane_RightClick;
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
        UpdateEmptyText(EmptyTextColor);
    }

    private void RootLane_LeftClick(object? sender, ClickEventArgs e)
    {
        if (EditableType is null)
        {
            return;
        }
        Game1.playSound("bigSelect");
        var overlay = new KeybindOverlay(spriteMap)
        {
            AddButtonText = AddButtonText,
            DeleteButtonTooltip = DeleteButtonTooltip,
            KeybindList = KeybindList,
            KeybindType = EditableType.Value,
        };
        overlay.Close += Overlay_Close;
        Overlay.Push(overlay);
        if (EditableType != KeybindType.MultipleKeybinds || !KeybindList.IsBound)
        {
            UI.InputHelper.Suppress(e.Button);
            overlay.StartCapturing();
        }
    }

    private void RootLane_PointerEnter(object? sender, PointerEventArgs e)
    {
        if (EditableType is null)
        {
            return;
        }
        UpdateTint(Color.Orange);
        UpdateEmptyText(Color.SaddleBrown);
    }

    private void RootLane_PointerLeave(object? sender, PointerEventArgs e)
    {
        UpdateTint(Color.White);
        UpdateEmptyText(EmptyTextColor);
    }

    private void RootLane_RightClick(object? sender, ClickEventArgs e)
    {
        if (EditableType is null)
        {
            return;
        }
        Game1.playSound("drumkit5");
        KeybindList = new();
    }

    private void UpdateAll()
    {
        var keybindViews = keybindList
            .Keybinds.Where(kb => kb.IsBound)
            .Select(
                (kb, index) =>
                    new Frame()
                    {
                        Layout = LayoutParameters.FitContent(),
                        Margin = index > 0 ? new Edges(Left: 16) : Edges.NONE,
                        Background = UiSprites.MenuSlotTransparent,
                        Padding = UiSprites.MenuSlotTransparent.FixedEdges! + new Edges(4),
                        Content = new KeybindView
                        {
                            ButtonHeight = buttonHeight,
                            Font = font,
                            Keybind = kb,
                            SpriteMap = spriteMap,
                        },
                    }
            )
            .Cast<IView>()
            .ToList();
        rootLane.Children = keybindViews.Count > 0 ? keybindViews : [Label.Simple(EmptyText)];
    }

    private void UpdateEmptyText(Color color)
    {
        if (rootLane.Children.Count == 1 && rootLane.Children[0] is Label label)
        {
            label.Text = EmptyText;
            label.Color = color;
        }
    }

    private void UpdateTint(Color tintColor)
    {
        foreach (var keybindView in KeybindViews)
        {
            keybindView.TintColor = tintColor;
        }
    }
}
