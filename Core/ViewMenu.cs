﻿using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewModdingAPI;
using StardewUI.Animation;
using StardewUI.Data;
using StardewUI.Events;
using StardewUI.Graphics;
using StardewUI.Input;
using StardewUI.Layout;
using StardewUI.Overlays;
using StardewUI.Widgets;
using StardewValley;
using StardewValley.Menus;

namespace StardewUI;

/// <summary>
/// Generic menu implementation based on a root <see cref="IView"/>.
/// </summary>
public abstract class ViewMenu : IClickableMenu, IDisposable
{
    /// <summary>
    /// Event raised when the menu is closed.
    /// </summary>
    public event EventHandler<EventArgs>? Closed;

    /// <summary>
    /// Offset from the menu view's top-right edge to draw the close button, if a <see cref="CloseButtonSprite"/> is
    /// also specified.
    /// </summary>
    public Vector2 CloseButtonOffset { get; set; } = new(36, -8);

    /// <summary>
    /// The sprite to draw for the close button shown on the upper right. If no value is specified, then no close button
    /// will be drawn. The default behavior is to not show any close button.
    /// </summary>
    public Sprite? CloseButtonSprite
    {
        get => closeButton.Sprite;
        set => closeButton.Sprite = value;
    }

    /// <summary>
    /// Whether to automatically close the menu when a mouse click is detected outside the bounds of the menu and any
    /// floating elements.
    /// </summary>
    /// <remarks>
    /// This setting is primarily intended for submenus and makes them behave more like overlays.
    /// </remarks>
    public bool CloseOnOutsideClick { get; set; }

    /// <summary>
    /// Additional cursor to draw below or adjacent to the normal mouse cursor.
    /// </summary>
    public Cursor? CursorAttachment { get; set; }

    /// <summary>
    /// Amount of dimming between 0 and 1; i.e. opacity of the background underlay.
    /// </summary>
    /// <remarks>
    /// Underlay is only drawn when game options do not force clear backgrounds.
    /// </remarks>
    public float DimmingAmount { get; set; } = 0.75f;

    /// <summary>
    /// Whether to display tooltips on mouse hover.
    /// </summary>
    /// <remarks>
    /// Tooltips should normally always be left enabled; one reason to disable them would be if a
    /// <see cref="CursorAttachment"/> is set that would overlap.
    /// </remarks>
    public bool TooltipsEnabled { get; set; } = true;

    /// <summary>
    /// The view to display with this menu.
    /// </summary>
    public IView View => view;

    /// <summary>
    /// Gets or sets the menu's gutter edges, which constrain the portion of the viewport in which any part of the menu
    /// may be drawn.
    /// </summary>
    /// <remarks>
    /// Gutters effectively shrink the viewport for both measurement (size calculation) and layout (centering) by
    /// clipping the screen edges.
    /// </remarks>
    protected Edges? Gutter
    {
        get => gutter;
        set => gutter = value;
    }

    private static readonly ActionRepeat ButtonRepeat = ActionRepeat.Default;
    private static readonly Edges DefaultGutter = new(100, 50);

    // SMAPI intercepts calls to the MouseState, but only syncs it on the update tick, meaning if we call
    // Game1.setMousePosition followed by Game1.getMousePosition in the same frame, we get the old position.
    // As a workaround, we track this separately each frame, on the assumption that it gets cleared in
    // performHoverAction which should only run for the topmost menu (thus multiple menus won't interfere with each
    // other).
    private static Point? mousePositionOverride;

    private readonly Dictionary<SButton, (long duration, bool isRepeating)> buttonHeldDurations = [];
    private readonly Image closeButton = new();
    private readonly HashSet<SButton> handledPressedButtons = [];

    // For tracking activation paths, we not only want a weak table for the overlay itself (to prevent overlays from
    // being leaked) but also for the ViewChild path used to activate it, because these views may go out of scope while
    // the overlay is open.
    private readonly ConditionalWeakTable<IOverlay, WeakViewChild[]> overlayActivationPaths = [];
    private readonly OverlayContext overlayContext = new();
    private readonly ConditionalWeakTable<IOverlay, OverlayLayoutData> overlayCache = [];
    private readonly RenderTargetPool renderTargetPool = new(Game1.graphics.GraphicsDevice, slack: 2);
    private readonly IView view;
    private readonly bool wasHudDisplayed;

    private ViewChild[] focusableHoverPath = [];
    private Edges? gutter;
    private ViewChild[] hoverPath = [];
    private bool isRehoverScheduled;
    private int? rehoverRequestTick;
    private bool wasChildMenuAttached;

    // When clearing the activeClickableMenu, the game will call its Dispose method BEFORE actually changing the field
    // value to null or the new menu. If a Close handler then tries to open a different menu (which is really the
    // primary use case for the Close event) then this could trigger an infinite loop/stack overflow, i.e.
    // Dispose -> Close (Handler) -> set Game1.activeClickableMenu -> Dispose again
    // As a workaround, we can track when dispose has been requested and suppress duplicates.
    private bool isDisposed;
    private bool isLeftClickSuppressed; // Stops button-held from leaking into new overlays.
    private bool isRightClickSuppressed;
    private bool justPushedOverlay; // Whether the overlay was pushed within the last frame.
    private WeakViewChild[] keyboardCaptureActivationPath = [];
    private Point previousHoverPosition;
    private Point previousDragPosition;
    private bool wasKeyboardCaptured;

    /// <summary>
    /// Initializes a new instance of <see cref="ViewMenu"/>.
    /// </summary>
    /// <param name="gutter">Gutter edges, in which no content should be drawn. Used for overscan, or general
    /// aesthetics.</param>
    /// <param name="forceDefaultFocus">Whether to always focus (snap the cursor to) the default element, even if the
    /// menu was triggered by keyboard/mouse.</param>
    public ViewMenu(Edges? gutter = null, bool forceDefaultFocus = false)
    {
        using var _ = Diagnostics.Trace.Begin(this, "ctor");

        Game1.playSound("bigSelect");

        this.gutter = gutter;
        overlayContext.Pushed += OverlayContext_Pushed;
        view = CreateView();
        MeasureAndCenterView();

        if (forceDefaultFocus || Game1.options.gamepadControls)
        {
            SetDefaultFocus(view, new(xPositionOnScreen, yPositionOnScreen));
        }

        wasHudDisplayed = Game1.displayHUD;
        Game1.displayHUD = false;

        HoverScale.Attach(closeButton, 1.2f, TimeSpan.FromMilliseconds(150));
    }

    /// <summary>
    /// Creates the view.
    /// </summary>
    /// <remarks>
    /// Subclasses will generally create an entire tree in this method and store references to any views that might
    /// require content updates.
    /// </remarks>
    /// <returns>The created view.</returns>
    protected abstract IView CreateView();

    /// <summary>
    /// Initiates a focus search in the specified direction.
    /// </summary>
    /// <param name="directionValue">An integer value corresponding to the direction; one of 0 (up), 1 (right), 2 (down)
    /// or 3 (left).</param>
    public override void applyMovementKey(int directionValue)
    {
        using var trace = Diagnostics.Trace.Begin(this, nameof(applyMovementKey));
        using var _ = OverlayContext.PushContext(overlayContext);
        var direction = (Direction)directionValue;
        var mousePosition = GetMousePosition();
        OnViewOrOverlay(
            (view, origin) =>
            {
                var found = view.FocusSearch(mousePosition.ToVector2() - origin, direction);
                if (found is not null)
                {
                    FinishFocusSearch(view, origin.ToPoint(), found);
                    RequestRehover();
                }
            }
        );
    }

    /// <summary>
    /// Returns whether or not the menu wants <b>exclusive</b> gamepad controls.
    /// </summary>
    /// <remarks>
    /// This implementation always returns <c>false</c>. Contrary to what the name in Stardew's code implies, this
    /// setting is not required for <see cref="receiveGamePadButton(Buttons)"/> to work; instead, when enabled, it
    /// suppresses the game's default mapping of button presses to clicks, and would therefore require reimplementing
    /// key-repeat and other basic behaviors. There is no reason to enable it here.
    /// </remarks>
    /// <returns>Always <c>false</c>.</returns>
    public override bool areGamePadControlsImplemented()
    {
        return false;
    }

    /// <summary>
    /// Closes this menu, either by removing it from the parent if it is a child menu, or removing it as the game's
    /// active menu if it is standalone.
    /// </summary>
    public void Close()
    {
        var behavior = GetCloseBehavior();
        if (behavior == MenuCloseBehavior.Disabled)
        {
            return;
        }
        bool hasCloseSound = !string.IsNullOrEmpty(closeSound);
        if (behavior == MenuCloseBehavior.Custom)
        {
            behaviorBeforeCleanup?.Invoke(this);
            cleanupBeforeExit();
            if (hasCloseSound)
            {
                Game1.playSound(closeSound);
            }
            CustomClose();
            if (exitFunction != null)
            {
                onExit onExit = exitFunction;
                exitFunction = null;
                onExit();
            }
        }
        else if (IsTitleSubmenu())
        {
            if (hasCloseSound)
            {
                Game1.playSound(closeSound);
            }
            TitleMenu.subMenu = null;
        }
        else
        {
            exitThisMenu(hasCloseSound);
            // Calling exitThisMenu may or may not dispose us, depending on whether we are the main menu or a child
            // menu. Since it's safe to Dispose multiple times, do it an extra time here to ensure proper disposal.
            Dispose();
        }
    }

    /// <inheritdoc />
    public void Dispose()
    {
        if (isDisposed)
        {
            return;
        }
        isDisposed = true;
        Game1.displayHUD = wasHudDisplayed;
        OnClosed(EventArgs.Empty);
        view.Dispose();
        renderTargetPool.Dispose();
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Draws the current menu content.
    /// </summary>
    /// <param name="b">The target batch.</param>
    public override void draw(SpriteBatch b)
    {
        using var trace = Diagnostics.Trace.Begin(this, nameof(draw));

        // Hack: We can't intercept the child menu assignment without a Harmony patch, and to add insult to injury,
        // menus with child menus don't even receive Update ticks. However, they do receive draw calls, so as long as
        // the logic is kept very simple, we can approximate the result with relatively low cost.
        if (GetChildMenu() is not null)
        {
            wasChildMenuAttached = true;
        }

        var viewportBounds = Game1.graphics.GraphicsDevice.Viewport.Bounds;
        if (!Game1.options.showClearBackgrounds && DimmingAmount > 0)
        {
            b.Draw(Game1.fadeToBlackRect, viewportBounds, Color.Black * DimmingAmount);
        }

        using var _ = OverlayContext.PushContext(overlayContext);

        MeasureAndCenterView();

        var origin = new Point(xPositionOnScreen, yPositionOnScreen);
        using ISpriteBatch viewBatch = new PropagatedSpriteBatch(b, GlobalTransform.Default, renderTargetPool);
        viewBatch.Translate(origin.ToVector2());
        using (viewBatch.SaveTransform())
        {
            view.Draw(viewBatch);
        }

        if (shouldDrawCloseButton() && closeButton.Sprite is not null)
        {
            closeButton.Measure(viewportBounds.Size.ToVector2());
            using (viewBatch.SaveTransform())
            {
                viewBatch.Translate(GetCloseButtonTranslation());
                closeButton.Draw(viewBatch);
            }
        }

        viewBatch.Dispose(); // Clear any lazy state, like transforms.

        foreach (var overlay in overlayContext.BackToFront())
        {
            b.Draw(Game1.fadeToBlackRect, viewportBounds, Color.Black * overlay.DimmingAmount);
            var overlayData = MeasureAndPositionOverlay(overlay);
            using ISpriteBatch overlayBatch = new PropagatedSpriteBatch(b, GlobalTransform.Default, renderTargetPool);
            overlayBatch.Translate(overlayData.Position);
            overlay.View.Draw(overlayBatch);
        }

        if (justPushedOverlay && overlayContext.Front is IOverlay frontOverlay && Game1.options.gamepadControls)
        {
            var defaultFocusPosition = frontOverlay
                .View.GetDefaultFocusPath()
                .ToGlobalPositions()
                .LastOrDefault()
                ?.Center();
            if (defaultFocusPosition.HasValue)
            {
                var overlayData = GetOverlayLayoutData(frontOverlay);
                SetMousePosition((overlayData.Position + defaultFocusPosition.Value).ToPoint());
            }
        }
        justPushedOverlay = false;

        if (TooltipsEnabled && IsTopmost())
        {
            var tooltip = BuildTooltip(hoverPath);
            if (tooltip is not null)
            {
                string? extraItemToShowIndex = TooltipData.ValidateItemId(tooltip.RequiredItemId);
                drawToolTip(
                    b,
                    tooltip.Text,
                    tooltip.Title ?? "",
                    tooltip.Item,
                    moneyAmountToShowAtBottom: tooltip.CurrencyAmount ?? -1,
                    currencySymbol: tooltip.CurrencySymbol,
                    extraItemToShowIndex: extraItemToShowIndex,
                    extraItemToShowAmount: tooltip.RequiredItemAmount,
                    craftingIngredients: tooltip.CraftingRecipe,
                    additionalCraftMaterials: tooltip.AdditionalCraftingMaterials
                );
            }
        }

        Game1.mouseCursorTransparency = 1.0f;
        if (!IsInputCaptured())
        {
            if (CursorAttachment is not null)
            {
                var cursorSize = CursorAttachment.Size ?? CursorAttachment.Sprite.Size;
                var cursorPosition = GetMousePosition() + (CursorAttachment.Offset ?? Cursor.DefaultOffset);
                var cursorRect = new Rectangle(cursorPosition, cursorSize);
                b.Draw(
                    CursorAttachment.Sprite.Texture,
                    cursorRect,
                    CursorAttachment.Sprite.SourceRect,
                    CursorAttachment.Tint ?? Cursor.DefaultTint
                );
            }
            var pointerStyle = hoverPath.Select(x => x.View.PointerStyle).LastOrDefault(PointerStyle.Default);
            drawMouse(b, cursor: (int)pointerStyle);
        }

        // This "should" be done in Update, not Draw, but the game won't send any updates to the menu while the capture
        // target is active.
        HandleKeyboardCaptureChange();

        // Reset this every frame - it is a per-frame signal for suppressing default behavior.
        //
        // Another thing that really should be done in Update, but can't be because the timing is insane, and Update
        // happens *between* the button press and key press events instead of after (or before) both.
        foreach (var button in handledPressedButtons)
        {
            if (!UI.InputHelper.IsDown(button))
            {
                handledPressedButtons.Remove(button);
            }
        }
    }

    /// <summary>
    /// Invoked on every frame during which a controller button is down, once for each held button.
    /// </summary>
    /// <param name="b">The button that is down.</param>
    public override void gamePadButtonHeld(Buttons b)
    {
        using var trace = Diagnostics.Trace.Begin(this, nameof(gamePadButtonHeld));

        if (!CanReceiveButton(b, out var button))
        {
            return;
        }

        var elapsed = Game1.currentGameTime.ElapsedGameTime.Ticks;
        if (buttonHeldDurations.TryGetValue(button, out var entry))
        {
            var nextDuration = entry.duration + elapsed;
            var maxDuration =
                (!entry.isRepeating && ButtonRepeat.InitialDelay.HasValue)
                    ? ButtonRepeat.InitialDelay.Value.Ticks
                    : ButtonRepeat.RepeatInterval.Ticks;
            bool willRepeat = nextDuration >= maxDuration;
            if (willRepeat)
            {
                using var _ = OverlayContext.PushContext(overlayContext);
                InitiateButtonPress(button, repeat: true);
            }
            buttonHeldDurations[button] = (nextDuration % maxDuration, willRepeat || entry.isRepeating);
        }
        else
        {
            // Because the initial press is already handled in receiveGamePadButton, we don't explicitly dispatch here,
            // just start tracking the button.
            //
            // Also, because gamePadButtonHeld runs after receiveGamePadButton, we start with a duration of 0 rather
            // than the last frame delta; that is, the current frame doesn't count.
            buttonHeldDurations.Add(button, (duration: 0, isRepeating: false));
        }
    }

    /// <summary>
    /// Invoked on every frame in which a mouse button is down, regardless of the state in the previous frame.
    /// </summary>
    /// <param name="x">The mouse's current X position on screen.</param>
    /// <param name="y">The mouse's current Y position on screen.</param>
    public override void leftClickHeld(int x, int y)
    {
        if (isLeftClickSuppressed)
        {
            return;
        }

        using var trace = Diagnostics.Trace.Begin(this, nameof(leftClickHeld));

        if (Game1.options.gamepadControls || IsInputCaptured())
        {
            // No dragging with gamepad.
            return;
        }
        var dragPosition = new Point(x, y);
        if (dragPosition == previousDragPosition)
        {
            return;
        }
        previousDragPosition = dragPosition;
        using var _ = OverlayContext.PushContext(overlayContext);
        OnViewOrOverlay((view, origin) => view.OnDrag(new(dragPosition.ToVector2() - origin)));
    }

    /// <summary>
    /// Opens this menu, i.e. makes it active if it is not already active.
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <param name="activationMode">The activation behavior which determines which (if any) other active menu this one
    /// can replace. Ignored when the game's title menu is open.</param>
    public void Open(MenuActivationMode activationMode = MenuActivationMode.Standalone)
    {
        using var _ = Diagnostics.Trace.Begin(this, nameof(Open));
        if (Game1.activeClickableMenu is TitleMenu)
        {
            TitleMenu.subMenu = this;
        }
        else
        {
            for (var menu = Game1.activeClickableMenu; menu is not null; menu = menu.GetParentMenu())
            {
                if (menu == this)
                {
                    return;
                }
            }
            var parentMenu = activationMode switch
            {
                MenuActivationMode.Child => Game1.activeClickableMenu,
                MenuActivationMode.Sibling => Game1.activeClickableMenu?.GetParentMenu(),
                _ => null,
            };
            if (parentMenu is not null)
            {
                parentMenu.SetChildMenu(this);
            }
            else
            {
                Game1.activeClickableMenu = this;
            }
        }
    }

    /// <summary>
    /// Invoked on every frame with the mouse's current coordinates.
    /// </summary>
    /// <remarks>
    /// Essentially the same as <see cref="update(GameTime)"/> but slightly more convenient for mouse hover/movement
    /// effects because of the arguments provided.
    /// </remarks>
    /// <param name="x">The mouse's current X position on screen.</param>
    /// <param name="y">The mouse's current Y position on screen.</param>
    public override void performHoverAction(int x, int y)
    {
        using var trace = Diagnostics.Trace.Begin(this, nameof(performHoverAction));
        mousePositionOverride = null;
        bool rehover = isRehoverScheduled || rehoverRequestTick.HasValue;
        if (rehover || (previousHoverPosition.X != x || previousHoverPosition.Y != y))
        {
            using var _ = OverlayContext.PushContext(overlayContext);
            OnViewOrOverlay((view, origin) => PerformHoverAction(view, origin));
        }

        // We use two flags for this in order to repeat the re-hover after one frame, because (a) input events won't
        // always get handled in the ideal order to track any specific change, and (b) even if they do, when operating
        // menus from the Framework, there can often by a one-frame delay before everything gets perfectly in sync due
        // to coordination between the view model, in vs. out bindings, reactions to INPC or other change events, etc.
        //
        // A delay of exactly 1 frame isn't always going to be perfect either, but it handles the majority of cases such
        // as wheel scrolling and controller-triggered tab/page navigation.
        isRehoverScheduled = rehoverRequestTick.HasValue;
        if (rehoverRequestTick <= Game1.ticks)
        {
            rehoverRequestTick = null;
        }
    }

    /// <inheritdoc />
    /// <remarks>
    /// Always a no-op for menus in StardewUI.
    /// </remarks>
    public override void populateClickableComponentList()
    {
        // The base class does a bunch of nasty reflection to populate the list, none of which is compatible with how
        // this menu works. To save time, we can simply do nothing here.
    }

    /// <summary>
    /// Checks if the menu is allowed to be closed by the game's default input handling.
    /// </summary>
    public override bool readyToClose()
    {
        return GetCloseBehavior() == MenuCloseBehavior.Default;
    }

    /// <summary>
    /// Invoked whenever a controller button is newly pressed.
    /// </summary>
    /// <param name="b">The button that was pressed.</param>
    public override void receiveGamePadButton(Buttons b)
    {
        using var trace = Diagnostics.Trace.Begin(this, nameof(receiveGamePadButton));

        if (!CanReceiveButton(b, out var button))
        {
            return;
        }

        // When the game performs updateActiveMenu, it checks areGamePadControlsImplemented(), and if false, translates
        // those buttons into clicks.
        //
        // We still receive this event regardless of areGamePadControlsImplemented(), but letting updateActiveMenu
        // convert the A and X button presses into clicks makes receiveLeftClick and receiveRightClick fire repeatedly
        // as the button is held, which is generally the behavior that users will be accustomed to. If we override the
        // gamepad controls then we'd have to reimplement the repeating-click logic.

        using var _ = OverlayContext.PushContext(overlayContext);
        InitiateButtonPress(button);
        switch (button)
        {
            case SButton.LeftTrigger:
                OnTabbable(p => p.PreviousTab());
                break;
            case SButton.RightTrigger:
                OnTabbable(p => p.NextTab());
                break;
            case SButton.LeftShoulder:
                OnPageable(p => p.PreviousPage());
                break;
            case SButton.RightShoulder:
                OnPageable(p => p.NextPage());
                break;
            case SButton.ControllerB:
                if (ShouldForceCloseOnMenuButton() && overlayContext.Pop() is null)
                {
                    Close();
                }
                break;
        }
    }

    /// <summary>
    /// Invoked whenever a keyboard key is newly pressed.
    /// </summary>
    /// <param name="key">The key that was pressed.</param>
    public override void receiveKeyPress(Keys key)
    {
        using var _ = Diagnostics.Trace.Begin(this, nameof(receiveKeyPress));
        var realButton = ButtonResolver.GetPressedButton(key.ToSButton());
        // See comments on receiveGamePadButton for why we don't dispatch the key itself.
        if (IsInputCaptured() || UI.InputHelper.IsSuppressed(realButton))
        {
            return;
        }
        var action = ButtonResolver.GetButtonAction(realButton);
        // Key presses "fall through" from receiveGamepadButton. Both handlers are invoked, first gamepad and then
        // keyboard, the latter with the "mapped" key. Here we try to dedupe it and skip actions that would have already
        // been performed in the gamepad handler.
        var isController = realButton.TryGetController(out var _);
        if (
            action == ButtonAction.Cancel
            // This intentionally skips the entire keyboard event, and not just the overlay pop, if it "should have"
            // been handled by the gamepad event. Some follow-up logic is probably wrong to do even when there is no
            // active overlay.
            && ((isController && ShouldForceCloseOnMenuButton()) || overlayContext.Pop() is not null)
        )
        {
            return;
        }
        if (!isController && !Game1.isAnyGamePadButtonBeingHeld())
        {
            InitiateButtonPress(realButton);
        }
        // The choices we have for actually "capturing" the captured input aren't awesome. Since it's a *keyboard*
        // input, we really don't want to let keyboard events through, like having the "e" key dismiss the menu while
        // trying to type in a field. On the other hand, blocking all key presses will also block gamepad buttons when
        // areGamepadControlsImplemented() is false, and it is false for the reasons documented in receiveGamePadButton.
        //
        // The best workaround seems to be to keep track of whether or not any gamepad button is being pressed, and use
        // that to allow "keyboard" input on the basis of it not being "real" keyboard input. This could run into issues
        // in some far-out scenarios like simultaneous keyboard + controller presses, but eliminates much more obvious
        // and frustrating issues like not being able to navigate or dismiss the menu with a controller after typing on
        // a regular keyboard.
        if (
            key == Keys.Escape
            || Game1.keyboardDispatcher.Subscriber is not ICaptureTarget
            || Game1.isAnyGamePadButtonBeingHeld()
        )
        {
            if (ShouldForceCloseOnMenuButton() && !isController && Game1.options.menuButton.Any(b => b.key == key))
            {
                Close();
            }
            else if (handledPressedButtons.Count == 0)
            {
                base.receiveKeyPress(key);
            }
        }
    }

    /// <summary>
    /// Invoked whenever the left mouse button is newly pressed.
    /// </summary>
    /// <param name="x">The mouse's current X position on screen.</param>
    /// <param name="y">The mouse's current Y position on screen.</param>
    /// <param name="playSound">Currently not used.</param>
    public override void receiveLeftClick(int x, int y, bool playSound = true)
    {
        using var trace = Diagnostics.Trace.Begin(this, nameof(receiveLeftClick));
        if (isLeftClickSuppressed || IsInputCaptured())
        {
            return;
        }
        var button = ButtonResolver.GetPressedButton(SButton.MouseLeft);
        if (UI.InputHelper.IsSuppressed(button))
        {
            return;
        }
        using var _ = OverlayContext.PushContext(overlayContext);
        InitiateClick(button, new(x, y));
    }

    /// <summary>
    /// Invoked whenever the right mouse button is newly pressed.
    /// </summary>
    /// <param name="x">The mouse's current X position on screen.</param>
    /// <param name="y">The mouse's current Y position on screen.</param>
    /// <param name="playSound">Currently not used.</param>
    public override void receiveRightClick(int x, int y, bool playSound = true)
    {
        using var trace = Diagnostics.Trace.Begin(this, nameof(receiveRightClick));
        if (isRightClickSuppressed || IsInputCaptured())
        {
            return;
        }
        var button = ButtonResolver.GetPressedButton(SButton.MouseRight);
        if (UI.InputHelper.IsSuppressed(button))
        {
            return;
        }
        using var _ = OverlayContext.PushContext(overlayContext);
        InitiateClick(button, new(x, y));
    }

    /// <summary>
    /// Invoked whenever the mouse wheel is used. Only works with vertical scrolls.
    /// </summary>
    /// <param name="value">A value indicating the desired vertical scroll direction; negative values indicate "down"
    /// and positive values indicate "up".</param>
    public override void receiveScrollWheelAction(int value)
    {
        using var trace = Diagnostics.Trace.Begin(this, nameof(receiveScrollWheelAction));
        if (IsInputCaptured())
        {
            return;
        }
        using var _ = OverlayContext.PushContext(overlayContext);
        // IClickableMenu calls the value "direction" but it is actually a magnitude, and always in the Y direction
        // (negative is down).
        var direction = value > 0 ? Direction.North : Direction.South;
        InitiateWheel(direction);
    }

    /// <summary>
    /// Invoked whenever the left mouse button is just released, after being pressed/held on the last frame.
    /// </summary>
    /// <param name="x">The mouse's current X position on screen.</param>
    /// <param name="y">The mouse's current Y position on screen.</param>
    public override void releaseLeftClick(int x, int y)
    {
        using var trace = Diagnostics.Trace.Begin(this, nameof(releaseLeftClick));
        if (isLeftClickSuppressed || IsInputCaptured())
        {
            return;
        }
        using var _ = OverlayContext.PushContext(overlayContext);
        var mousePosition = new Point(x, y);
        previousDragPosition = mousePosition;
        OnViewOrOverlay((view, origin) => view.OnDrop(new(mousePosition.ToVector2() - origin)));
    }

    /// <summary>
    /// Returns whether or not to draw a button on the upper right that closes the menu when clicked.
    /// </summary>
    /// <remarks>
    /// Regardless of this value, a close button will never be drawn unless <see cref="CloseButtonSprite"/> is set.
    /// </remarks>
    public override bool shouldDrawCloseButton()
    {
        return CloseButtonSprite is not null && GetCloseBehavior() != MenuCloseBehavior.Disabled;
    }

    /// <summary>
    /// Runs on every update tick.
    /// </summary>
    /// <param name="time">The current <see cref="GameTime"/> including the time elapsed since last update tick.</param>
    public override void update(GameTime time)
    {
        using var _ = Diagnostics.Trace.Begin(this, nameof(update));

        // We don't get an explicit "release" event for gamepad buttons, so need to check for releases ourselves.
        foreach (var button in buttonHeldDurations.Keys)
        {
            if (!UI.InputHelper.IsDown(button))
            {
                buttonHeldDurations.Remove(button);
            }
        }

        // In real event loops the child menu close detection will have already happened in performHoverAction, making
        // this instance moot, but it is technically possible for a child menu to be opened and closed without the
        // pointer ever having moved.
        HandleChildMenuClosed();
        View.OnUpdate(time.ElapsedGameTime);
        foreach (var overlay in overlayContext.FrontToBack())
        {
            overlay.Update(time.ElapsedGameTime);
        }

        // This is done in the update loop instead of in releaseLeftClick because we aren't guaranteed to receive that
        // event in some situations, like when an overlay starts capturing.
        if (isLeftClickSuppressed || isRightClickSuppressed)
        {
            var mouseState = Game1.input.GetMouseState();
            isLeftClickSuppressed &= mouseState.LeftButton == ButtonState.Pressed;
            isRightClickSuppressed &= mouseState.RightButton == ButtonState.Pressed;
        }
    }

    /// <summary>
    /// Builds/formats a tooltip given the sequence of views from root to the lowest-level hovered child.
    /// </summary>
    /// <remarks>
    /// The default implementation reads the value of the <em>last</em> (lowest-level) view with a non-null
    /// <see cref="IView.Tooltip"/>, and breaks <see cref="TooltipData.Text"/> and <see cref="TooltipData.Title"/> lines
    /// longer than 640px, which is the default vanilla tooltip width.
    /// </remarks>
    /// <param name="path">Sequence of all elements, and their relative positions, that the mouse coordinates are
    /// currently within.</param>
    /// <returns>The tooltip string to display, or <c>null</c> to not show any tooltip.</returns>
    protected virtual TooltipData? BuildTooltip(IEnumerable<ViewChild> path)
    {
        var tooltipData = path.Select(x => x.View.Tooltip).LastOrDefault(tooltip => tooltip is not null);
        return tooltipData?.ConstrainTextWidth(640);
    }

    /// <summary>
    /// When overridden in a derived class, provides an alternative method to close the menu instead of the default
    /// logic in <see cref="IClickableMenu.exitThisMenu(bool)"/>.
    /// </summary>
    /// <remarks>
    /// The method will only be called when the menu is closed (either programmatically or via the UI) while
    /// <see cref="GetCloseBehavior"/> is returning <see cref="MenuCloseBehavior.Custom"/>.
    /// </remarks>
    protected virtual void CustomClose()
    {
        if (
            this == Game1.activeClickableMenu
            || (Game1.activeClickableMenu is GameMenu gameMenu && gameMenu.GetCurrentPage() == this)
        )
        {
            Game1.exitActiveMenu();
        }
        if (_parentMenu is IClickableMenu parentMenu)
        {
            _parentMenu = null;
            parentMenu.SetChildMenu(null);
            Dispose();
        }
    }

    /// <summary>
    /// Gets the current close behavior for the menu.
    /// </summary>
    /// <remarks>
    /// The default implementation always returns <see cref="MenuCloseBehavior.Default"/>. Subclasses may override this
    /// in order to use <see cref="CustomClose"/>, or disable closure entirely.
    /// </remarks>
    protected virtual MenuCloseBehavior GetCloseBehavior()
    {
        return MenuCloseBehavior.Default;
    }

    /// <summary>
    /// Computes the origin (top left) position of the menu for a given viewport and offset.
    /// </summary>
    /// <param name="viewportSize">The available size of the viewport in which the menu is to be displayed.</param>
    /// <param name="gutterOffset">The offset implied by any asymmetrical <see cref="Gutter"/> setting; for example,
    /// a gutter whose <see cref="Edges.Left"/> edge is <c>100</c> px and whose <see cref="Edges.Right"/> edge is only
    /// <c>50</c> px would have an X offset of <c>25</c> px (half the difference, because centered).</param>
    /// <returns>The origin (top left) position for the menu's root view.</returns>
    protected virtual Point GetOriginPosition(Point viewportSize, Point gutterOffset)
    {
        int x = viewportSize.X / 2 - width / 2 + gutterOffset.X;
        int y = viewportSize.Y / 2 - height / 2 + gutterOffset.Y;
        return new(x, y);
    }

    /// <summary>
    /// Invokes the <see cref="Closed"/> event handler.
    /// </summary>
    /// <param name="e">The event arguments.</param>
    protected virtual void OnClosed(EventArgs e)
    {
        Closed?.Invoke(this, e);
    }

    private bool CanReceiveButton(Buttons b, out SButton button)
    {
        // We don't actually dispatch the button to any capturing overlay, just prevent it from affecting the menu.
        //
        // This is because a capturing overlay doesn't necessarily just need to know about the button "press", it cares
        // about the entire press, hold and release cycle, and can handle these directly through InputState or SMAPI.
        if (IsInputCaptured())
        {
            button = SButton.None;
            return false;
        }
        button = b.ToSButton();
        return !UI.InputHelper.IsSuppressed(b.ToSButton());
    }

    private static void FinishFocusSearch(IView rootView, Point origin, FocusSearchResult found)
    {
        LogFocusSearchResult(found.Target);
        ReleaseCaptureTarget();
        Game1.playSound("shiny4");
        var pathWithTarget = found.Path.Append(found.Target).ToList();
        var nextMousePosition = origin + pathWithTarget.ToGlobalPositions().Last().CenterPoint();
        if (rootView.ScrollIntoView(pathWithTarget, out var distance))
        {
            nextMousePosition -= distance.ToPoint();
        }
        SetMousePosition(nextMousePosition);
    }

    private Vector2 GetCloseButtonTranslation()
    {
        return new(view.OuterSize.X + CloseButtonOffset.X, CloseButtonOffset.Y);
    }

    private static Point GetMousePosition()
    {
        return mousePositionOverride ?? Game1.getMousePosition(ui_scale: true);
    }

    private OverlayLayoutData GetOverlayLayoutData(IOverlay overlay)
    {
        var rootPosition = new Vector2(xPositionOnScreen, yPositionOnScreen);
        return overlayCache.GetValue(overlay, ov => OverlayLayoutData.FromOverlay(view, rootPosition, overlay));
    }

    private Vector2? GetRootViewPosition(IView view)
    {
        if (view.Equals(View))
        {
            return new(xPositionOnScreen, yPositionOnScreen);
        }
        foreach (var overlay in overlayContext.FrontToBack())
        {
            if (overlay.View == view && overlayCache.TryGetValue(overlay, out var layoutData))
            {
                return layoutData.Position;
            }
        }
        return null;
    }

    private bool HandleChildMenuClosed()
    {
        if (!wasChildMenuAttached)
        {
            return false;
        }
        Refocus(allowIfLayoutUnchanged: true);
        wasChildMenuAttached = false;
        return true;
    }

    private void HandleKeyboardCaptureChange()
    {
        if (GetChildMenu() is not null)
        {
            return;
        }
        bool isKeyboardCaptured = Game1.keyboardDispatcher.Subscriber is ICaptureTarget;
        if (isKeyboardCaptured != wasKeyboardCaptured)
        {
            if (isKeyboardCaptured)
            {
                keyboardCaptureActivationPath = focusableHoverPath.Select(child => child.AsWeak()).ToArray();
            }
            else if (Game1.options.gamepadControls)
            {
                RestoreFocusToPath(keyboardCaptureActivationPath);
            }
            wasKeyboardCaptured = isKeyboardCaptured;
        }
    }

    private void InitiateButtonPress(SButton button, bool repeat = false)
    {
        var mousePosition = GetMousePosition();
        OnViewOrOverlay(
            (view, origin) =>
            {
                var localPosition = mousePosition.ToVector2() - origin;
                if (!view.ContainsPoint(localPosition))
                {
                    return;
                }
                var args = new ButtonEventArgs(localPosition, button);
                if (repeat)
                {
                    view.OnButtonRepeat(args);
                }
                else
                {
                    view.OnButtonPress(args);
                }
                if (args.Handled)
                {
                    handledPressedButtons.Add(button);
                }
            }
        );
        RequestRehover();
    }

    private void InitiateClick(SButton button, Point screenPoint)
    {
        if (overlayContext.Front is IOverlay overlay)
        {
            var overlayData = GetOverlayLayoutData(overlay);
            var overlayLocalPosition = screenPoint.ToVector2() - overlayData.Position;
            if (!overlayData.ContainsPoint(overlayLocalPosition))
            {
                overlayContext.Pop();
            }
            else
            {
                var overlayArgs = new ClickEventArgs(overlayLocalPosition, button);
                overlay.View.OnClick(overlayArgs);
            }
            return;
        }
        var origin = new Point(xPositionOnScreen, yPositionOnScreen);
        var localPosition = (screenPoint - origin).ToVector2();
        if (Game1.keyboardDispatcher.Subscriber is ICaptureTarget captureTarget)
        {
            var clickPath = view.GetPathToPosition(localPosition);
            if (!clickPath.Select(child => child.View).Contains(captureTarget.CapturingView))
            {
                captureTarget.ReleaseCapture();
            }
        }
        var args = new ClickEventArgs(localPosition, button);
        view.OnClick(args);
        if (!args.Handled && GetCloseBehavior() != MenuCloseBehavior.Disabled)
        {
            if (closeButton.Sprite is not null)
            {
                var closePosition = GetCloseButtonTranslation();
                var closeBounds = new Bounds(closePosition, closeButton.OuterSize);
                if (closeBounds.ContainsPoint(localPosition))
                {
                    Close();
                    return;
                }
            }
            if (CloseOnOutsideClick && overlayContext.Front is null && !View.IsVisible(localPosition))
            {
                Close();
                return;
            }
        }
        RequestRehover();
    }

    private void InitiateWheel(Direction direction)
    {
        var mousePosition = GetMousePosition();
        OnViewOrOverlay(
            (view, origin) =>
            {
                var localPosition = mousePosition.ToVector2() - origin;
                var pathBeforeScroll = view.GetPathToPosition(localPosition, preferFocusable: true).ToList();
                var args = new WheelEventArgs(localPosition, direction);
                view.OnWheel(args);
                if (!args.Handled)
                {
                    return;
                }
                Game1.playSound("shiny4");
                Refocus(view, origin, localPosition, pathBeforeScroll, direction);
                RequestRehover();
            }
        );
    }

    private bool IsInputCaptured()
    {
        return overlayContext.FrontToBack().Any(overlay => overlay.CapturingInput);
    }

    private bool IsTitleSubmenu()
    {
        return Game1.activeClickableMenu is TitleMenu && TitleMenu.subMenu == this;
    }

    private bool IsTopmost()
    {
        return GetChildMenu() is null
            && !(
                // Hack to work around another hack: TitleMenu isn't capable of displaying or forwarding interactions to
                // children of its subMenu, but it _can_ have an actual ChildMenu, and that ChildMenu essentially works the
                // same as if it were a child of the subMenu. However, we need to detect this scenario explicitly.
                Game1.activeClickableMenu
                    is TitleMenu titleMenu
                && TitleMenu.subMenu == this
                && titleMenu.GetChildMenu() is not null
            );
    }

    [Conditional("DEBUG_FOCUS_SEARCH")]
    private static void LogFocusSearchResult(ViewChild? result)
    {
        Logger.Log($"Found: {result?.View.Name} ({result?.View.GetType().Name}) at {result?.Position}", LogLevel.Info);
    }

    private void MeasureAndCenterView()
    {
        using var _ = Diagnostics.Trace.Begin(this, nameof(MeasureAndCenterView));
        var viewportSize = UiViewport.GetMaxSize();
        var currentGutter = gutter ?? DefaultGutter;
        var availableMenuSize = viewportSize.ToVector2() - currentGutter.Total;
        if (!view.Measure(availableMenuSize))
        {
            return;
        }
        RequestRehover();
        // Make gutters act as margins; otherwise centering could actually place content in the gutter.
        // For example, if there is an asymmetrical gutter with left = 100 and right = 200, and it takes up the full
        // viewport width, then it will actually occupy the horizontal region from 150 to (viewportWidth - 150), which
        // is the centered region with 300px total margin. In this case we need to push the content left by 50px, or
        // half the difference between the left and right edge.
        var gutterOffsetX = (currentGutter.Left - currentGutter.Right) / 2;
        var gutterOffsetY = (currentGutter.Top - currentGutter.Bottom) / 2;
        width = (int)MathF.Round(view.OuterSize.X);
        height = (int)MathF.Round(view.OuterSize.Y);
        (xPositionOnScreen, yPositionOnScreen) = GetOriginPosition(viewportSize, new(gutterOffsetX, gutterOffsetY));
        if (Game1.keyboardDispatcher.Subscriber is not ICaptureTarget)
        {
            Refocus();
        }
    }

    private OverlayLayoutData MeasureAndPositionOverlay(IOverlay overlay)
    {
        var viewportBounds = Game1.graphics.GraphicsDevice.Viewport.Bounds;
        bool isUpdateRequired = overlay.View.OuterSize == Vector2.Zero;
        if (overlay.Parent is null)
        {
            isUpdateRequired = overlay.View.Measure(viewportBounds.Size.ToVector2());
        }
        var overlayData = GetOverlayLayoutData(overlay);
        if (overlay.Parent is not null)
        {
            var availableOverlaySize = viewportBounds.Size.ToVector2() - overlayData.Position;
            isUpdateRequired = overlay.View.Measure(availableOverlaySize);
        }
        if (isUpdateRequired)
        {
            overlayData.Update(overlay);
            RequestRehover();
        }
        return overlayData;
    }

    private void OnOverlayRemoved(IOverlay overlay)
    {
        // A convenience for gamepad users is to try to move the mouse cursor back to whatever triggered the
        // overlay in the first place, e.g. the button on a drop-down list.
        // However, it's unnecessarily distracting to do it for mouse controls.
        if (Game1.options.gamepadControls)
        {
            RestoreFocusToOverlayActivation(overlay);
        }
        RequestRehover();
    }

    private void OnPageable(Action<IPageable> action)
    {
        if (view is IPageable pageable)
        {
            action(pageable);
        }
    }

    private void OnTabbable(Action<ITabbable> action)
    {
        if (view is ITabbable tabbable)
        {
            action(tabbable);
        }
    }

    private void OnViewOrOverlay(Action<IView, Vector2> action)
    {
        if (overlayContext.Front is IOverlay overlay)
        {
            var overlayData = GetOverlayLayoutData(overlay);
            action(overlay.View, overlayData.Position);
        }
        else
        {
            var origin = new Vector2(xPositionOnScreen, yPositionOnScreen);
            action(view, origin);
        }
    }

    private void Overlay_Close(object? sender, EventArgs e)
    {
        if (sender is IOverlay overlay)
        {
            OnOverlayRemoved(overlay);
        }
    }

    private void OverlayContext_Pushed(object? sender, EventArgs e)
    {
        var overlay = overlayContext.Front!;
        overlayActivationPaths.AddOrUpdate(overlay, focusableHoverPath.Select(child => child.AsWeak()).ToArray());
        overlay.Close += Overlay_Close;
        justPushedOverlay = true;
        isLeftClickSuppressed = true;
        isRightClickSuppressed = true;
    }

    private void PerformHoverAction(IView rootView, Vector2 viewPosition)
    {
        var previousLocalPosition = previousHoverPosition.ToVector2() - viewPosition;
        HandleChildMenuClosed();
        var mousePosition = GetMousePosition();
        var localPosition = mousePosition.ToVector2() - viewPosition;
        var previousFocusableHoverPath = focusableHoverPath;
        SetHoverPath(rootView, localPosition);
        if (Game1.options.gamepadControls && focusableHoverPath.Length == 0)
        {
            Refocus(rootView, viewPosition, previousHoverPosition.ToVector2(), previousFocusableHoverPath, null);
            var newMousePosition = GetMousePosition();
            if (newMousePosition != mousePosition)
            {
                mousePosition = newMousePosition;
                localPosition = mousePosition.ToVector2() - viewPosition;
                SetHoverPath(rootView, localPosition);
            }
        }
        previousHoverPosition = mousePosition;
        var args = new PointerMoveEventArgs(previousLocalPosition, localPosition);
        rootView.OnPointerMove(args);
        if (!args.Handled && hoverPath.Length == 0 && view.Equals(rootView) && closeButton.Sprite is not null)
        {
            var closePosition = GetCloseButtonTranslation();
            var closeBounds = new Bounds(closePosition, closeButton.OuterSize);
            if (closeBounds.ContainsPoint(localPosition) || closeBounds.ContainsPoint(previousLocalPosition))
            {
                var closeArgs = args.Offset(-closePosition);
                closeButton.OnPointerMove(closeArgs);
            }
        }
    }

    private void Refocus(Direction? searchDirection = null, bool allowIfLayoutUnchanged = false)
    {
        if (!Game1.options.gamepadControls)
        {
            return;
        }
        var previousLeaf = focusableHoverPath.ToGlobalPositions().LastOrDefault();
        if (previousLeaf is null)
        {
            return;
        }
        OnViewOrOverlay(
            (view, origin) =>
            {
                var newLeaf = view.ResolveChildPath(focusableHoverPath.Select(x => x.View))
                    .ToGlobalPositions()
                    .LastOrDefault();
                if (
                    newLeaf?.View == previousLeaf.View
                    && (
                        allowIfLayoutUnchanged
                        || (
                            newLeaf.Position != previousLeaf.Position
                            || newLeaf.View.OuterSize != previousLeaf.View.OuterSize
                        )
                    )
                )
                {
                    Refocus(view, origin, previousHoverPosition.ToVector2(), focusableHoverPath, searchDirection);
                }
            }
        );
    }

    private static void Refocus(
        IView root,
        Vector2 origin,
        Vector2 previousPosition,
        IReadOnlyList<ViewChild> previousPath,
        Direction? searchDirection
    )
    {
        using var _ = Diagnostics.Trace.Begin(nameof(ViewMenu), nameof(Refocus));
        if (!Game1.options.gamepadControls)
        {
            return;
        }
        var pathAfterScroll = root.ResolveChildPath(previousPath.FocusablePath().Select(child => child.View));
        var (targetView, bounds) = pathAfterScroll.Aggregate(
            (root as IView, bounds: Bounds.Empty),
            (acc, child) => (child.View, new(acc.bounds.Position + child.Position, child.View.OuterSize))
        );
        var focusedViewChild = root.GetPathToPosition(bounds.Center(), preferFocusable: true)
            .FocusablePath()
            .ToGlobalPositions()
            .LastOrDefault();
        if (focusedViewChild?.View == targetView)
        {
            SetMousePosition((origin + focusedViewChild.Center()).ToPoint());
            return;
        }
        if (searchDirection.HasValue)
        {
            // Can happen if the target view is no longer reachable, i.e. outside the scroll bounds.
            //
            // When we try to find a new focus target, we have to accommodate for the fact that previousPosition is the
            // actual cursor position on screen before the scroll, and not the "adjusted" position of the cursor
            // relative to the new view bounds; for example, if we just scrolled up by 64 px and the view now has a
            // negative Y position (which is why we didn't find it with GetPathToPosition), then the value of
            // previousPosition will be 64px lower (still in bounds) and we have to adjust to compensate.
            var focusOffset =
                previousPath.Count > 0
                    ? bounds.Position - previousPath.ToGlobalPositions().Last().Position
                    : Vector2.Zero;
            var validResult = root.FocusSearch(previousPosition + focusOffset, searchDirection.Value);
            if (validResult is not null)
            {
                ReleaseCaptureTarget();
                var pathWithTarget = validResult.Path.Append(validResult.Target);
                var nextMousePosition = origin + pathWithTarget.ToGlobalPositions().Last().Center();
                SetMousePosition(nextMousePosition.ToPoint());
            }
            return;
        }
        // As a last resort, if the focus is now on nothing *and* the focus search turned up nothing (or we didn't do a
        // focus search because there was no direction) then reset to the default focus for the whole menu.
        if (focusedViewChild is null && SetDefaultFocus(root, origin))
        {
            ReleaseCaptureTarget();
        }
    }

    private static void ReleaseCaptureTarget()
    {
        if (Game1.keyboardDispatcher.Subscriber is ICaptureTarget captureTarget)
        {
            captureTarget.ReleaseCapture();
        }
    }

    private void RequestRehover()
    {
        rehoverRequestTick = Game1.ticks;
    }

    private void RestoreFocusToOverlayActivation(IOverlay overlay)
    {
        var overlayData = GetOverlayLayoutData(overlay);
        if (overlayActivationPaths.TryGetValue(overlay, out var activationPath) && RestoreFocusToPath(activationPath))
        {
            return;
        }
        var defaultFocusPosition = overlay.Parent?.GetDefaultFocusPath().ToGlobalPositions().LastOrDefault()?.Center();
        if (defaultFocusPosition.HasValue)
        {
            SetMousePosition((overlayData.Position + defaultFocusPosition.Value).ToPoint());
        }
    }

    private bool RestoreFocusToPath(WeakViewChild[] path)
    {
        if (path.Length == 0)
        {
            return false;
        }
        var strongActivationPath = path.Select(x => x.TryResolve(out var viewChild) ? viewChild : null).ToList();
        if (strongActivationPath.Count > 0 && strongActivationPath.All(child => child is not null))
        {
            var rootPosition = GetRootViewPosition(strongActivationPath[0]!.View);
            if (rootPosition is not null)
            {
                var localPosition = strongActivationPath!.ToGlobalPositions().Last().Center();
                var mousePosition = (rootPosition.Value + localPosition).ToPoint();
                SetMousePosition(mousePosition);
                using var _ = OverlayContext.PushContext(overlayContext);
                OnViewOrOverlay((view, origin) => PerformHoverAction(view, origin));
                return true;
            }
        }
        return false;
    }

    private static bool SetDefaultFocus(IView root, Vector2 origin)
    {
        var focusPosition = root.GetDefaultFocusPath().ToGlobalPositions().LastOrDefault()?.Center();
        if (focusPosition.HasValue)
        {
            var nextMousePosition = origin + focusPosition.Value;
            SetMousePosition(nextMousePosition.ToPoint());
            return true;
        }
        return false;
    }

    private void SetHoverPath(IView rootView, Vector2 localPosition)
    {
        hoverPath = rootView.GetPathToPosition(localPosition).ToArray();
        focusableHoverPath = rootView.GetPathToPosition(localPosition, preferFocusable: true).FocusablePath().ToArray();
    }

    private static void SetMousePosition(Point mousePosition)
    {
        Game1.setMousePosition(mousePosition, ui_scale: true);
        mousePositionOverride = mousePosition;
    }

    private bool ShouldForceCloseOnMenuButton()
    {
        return GetCloseBehavior() switch
        {
            MenuCloseBehavior.Disabled => false,
            MenuCloseBehavior.Custom => true,
            _ => IsTitleSubmenu(),
        };
    }

    class OverlayLayoutData(ViewChild root)
    {
        public Bounds ParentBounds { get; set; } = Bounds.Empty;
        public ViewChild[] ParentPath { get; set; } = [];
        public Vector2 Position { get; set; }

        // Interactable bounds are the individual bounding boxes of all top-level and floating views in the overlay.
        //
        // Union bounds are a single bounding box used to speed up checks that are completely outside any part of the
        // overlay, but a point being inside the union does not guarantee that it actually lands on a real view; that
        // is, there may be gaps between views.
        //
        // As a hybrid of speed and accuracy, we check the union bounds to exclude impossible points, then check the
        // interactable bounds to confirm a positive match.
        private Bounds[] interactableBounds = [];
        private Bounds unionBounds = Bounds.Empty;

        public static OverlayLayoutData FromOverlay(IView rootView, Vector2 rootPosition, IOverlay overlay)
        {
            using var _ = Diagnostics.Trace.Begin(nameof(OverlayLayoutData), nameof(FromOverlay));
            var data = new OverlayLayoutData(new(rootView, rootPosition));
            data.Update(overlay);
            return data;
        }

        public bool ContainsPoint(Vector2 point)
        {
            return unionBounds.ContainsPoint(point) && interactableBounds.Any(bounds => bounds.ContainsPoint(point));
        }

        public void Update(IOverlay overlay)
        {
            using var _ = Diagnostics.Trace.Begin(this, nameof(Update));
            var immediateParent = GetImmediateParent();
            if (overlay.Parent != immediateParent?.View)
            {
                ParentPath =
                    (
                        overlay.Parent is not null
                            ? root.View.GetPathToView(overlay.Parent)?.ToGlobalPositions().ToArray()
                            : null
                    ) ?? [];
            }
            immediateParent = GetImmediateParent();
            ParentBounds = immediateParent is not null
                ? immediateParent.GetContentBounds().Offset(root.Position)
                : GetUiViewportBounds();
            var x = ResolveAlignments(
                overlay.HorizontalParentAlignment,
                ParentBounds.Left,
                ParentBounds.Right,
                overlay.HorizontalAlignment,
                overlay.View.OuterSize.X
            );
            var y = ResolveAlignments(
                overlay.VerticalParentAlignment,
                ParentBounds.Top,
                ParentBounds.Bottom,
                overlay.VerticalAlignment,
                overlay.View.OuterSize.Y
            );
            Position = new Vector2(x, y);

            interactableBounds = overlay.View.FloatingBounds.Prepend(overlay.View.ActualBounds).ToArray();
            unionBounds = interactableBounds.Aggregate(Bounds.Empty, (acc, bounds) => acc.Union(bounds));
        }

        private ViewChild? GetImmediateParent() => ParentPath.Length > 0 ? ParentPath[^1] : null;

        private static Bounds GetUiViewportBounds()
        {
            var deviceViewport = Game1.graphics.GraphicsDevice.Viewport;
            var uiViewport = Game1.uiViewport;
            var viewportWidth = Math.Min(deviceViewport.Width, uiViewport.Width);
            var viewportHeight = Math.Min(deviceViewport.Height, uiViewport.Height);
            return new(new(0, 0), new(viewportWidth, viewportHeight));
        }

        private static float ResolveAlignments(
            Alignment parentAlignment,
            float parentStart,
            float parentEnd,
            Alignment childAlignment,
            float childLength
        )
        {
            var anchor = parentAlignment switch
            {
                Alignment.Start => parentStart,
                Alignment.Middle => (parentEnd - parentStart) / 2,
                Alignment.End => parentEnd,
                _ => throw new ArgumentException(
                    $"Invalid parent alignment: {parentAlignment}",
                    nameof(parentAlignment)
                ),
            };
            return childAlignment switch
            {
                Alignment.Start => anchor,
                Alignment.Middle => anchor - (childLength / 2),
                Alignment.End => anchor - childLength,
                _ => throw new ArgumentException($"Invalid child alignment: {childAlignment}", nameof(childAlignment)),
            };
        }
    }
}
