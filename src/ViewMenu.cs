using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Menus;
using System.Diagnostics;

namespace StardewUI;

/// <summary>
/// Generic menu implementation based on a root <see cref="IView"/>.
/// </summary>
public abstract class ViewMenu<T> : IClickableMenu, IDisposable where T : IView
{
    /// <summary>
    /// Amount of dimming between 0 and 1; i.e. opacity of the background underlay.
    /// </summary>
    /// <remarks>
    /// Underlay is only drawn when game options do not force clear backgrounds.
    /// </remarks>
    public float DimmingAmount { get; set; } = 0.75f;

    /// <summary>
    /// The view to display with this menu.
    /// </summary>
    public T View => view;

    private static readonly Edges DefaultGutter = new(100, 50);

    private readonly T view;
    private readonly bool wasHudDisplayed;

    private ViewChild[] hoverPath = [];
    private Point previousHoverPosition;
    private Point previousDragPosition;

    public ViewMenu(Edges? gutter = null)
    {
        Game1.playSound("bigSelect");

        view = CreateView();
        var viewportSize = GetViewportSize();
        var availableMenuSize = viewportSize.ToVector2() - (gutter ?? DefaultGutter).Total;
        view.Measure(availableMenuSize);
        width = (int)MathF.Round(view.OuterSize.X);
        height = (int)MathF.Round(view.OuterSize.Y);
        xPositionOnScreen = viewportSize.X / 2 - width / 2;
        yPositionOnScreen = viewportSize.Y / 2 - height / 2;

        var initialFocus = GetDefaultFocus(new(view, Vector2.Zero));
        if (initialFocus is not null)
        {
            var focusPosition = initialFocus.Center();
            Game1.setMousePosition(new Point(xPositionOnScreen, yPositionOnScreen) + focusPosition, true);
        }

        wasHudDisplayed = Game1.displayHUD;
        Game1.displayHUD = false;
    }

    /// <summary>
    /// Creates the view.
    /// </summary>
    /// <remarks>
    /// Subclasses will generally create an entire tree in this method and store references to any views that might
    /// require content updates.
    /// </remarks>
    /// <returns>The created view.</returns>
    protected abstract T CreateView();

    public override void applyMovementKey(int directionValue)
    {
        var direction = (Direction)directionValue;
        var origin = new Point(xPositionOnScreen, yPositionOnScreen);
        var mousePosition = Game1.input.GetMouseState().Position;
        var found = view.FocusSearch((mousePosition - origin).ToVector2(), direction);
        if (found is not null)
        {
            LogFocusSearchResult(found.Target);
            ReleaseCaptureTarget();
            Game1.playSound("shiny4");
            var nextMousePosition = origin + found.Target.Center();
            if (view.ScrollIntoView(found.Path, out var distance))
            {
                nextMousePosition -= distance.ToPoint();
            }
            Game1.setMousePosition(nextMousePosition, true);
        }
    }

    public override bool areGamePadControlsImplemented()
    {
        return false;
    }

    public void Dispose()
    {
        Game1.displayHUD = wasHudDisplayed;
        GC.SuppressFinalize(this);
    }

    public override void draw(SpriteBatch b)
    {
        if (!Game1.options.showClearBackgrounds)
        {
            b.Draw(Game1.fadeToBlackRect, Game1.graphics.GraphicsDevice.Viewport.Bounds, Color.Black * DimmingAmount);
        }

        view.Measure(new(width, height));

        var origin = new Point(xPositionOnScreen, yPositionOnScreen);
        var viewBatch = new PropagatedSpriteBatch(b, Transform.FromTranslation(origin.ToVector2()));
        view.Draw(viewBatch);

        var tooltip = hoverPath.Select(x => x.View.Tooltip).LastOrDefault(tooltip => !string.IsNullOrEmpty(tooltip));
        if (!string.IsNullOrEmpty(tooltip))
        {
            drawToolTip(b, tooltip, null, null);
        }

        Game1.mouseCursorTransparency = 1.0f;
        drawMouse(b);
    }

    public override void leftClickHeld(int x, int y)
    {
        var dragPosition = new Point(x, y);
        if (dragPosition == previousDragPosition)
        {
            return;
        }
        previousDragPosition = dragPosition;
        var origin = new Point(xPositionOnScreen, yPositionOnScreen);
        var localPosition = (dragPosition - origin).ToVector2();
        view.OnDrag(new(localPosition));
    }

    public override void performHoverAction(int x, int y)
    {
        if (previousHoverPosition.X == x && previousHoverPosition.Y == y)
        {
            return;
        }
        var origin = new Point(xPositionOnScreen, yPositionOnScreen);
        var mousePosition = new Point(x, y);
        var localPosition = (mousePosition - origin).ToVector2();
        var previousLocalPosition = (previousHoverPosition - origin).ToVector2();
        previousHoverPosition = new(x, y);
        hoverPath = view.GetPathToPosition(localPosition).ToArray();
        view.OnPointerMove(new PointerMoveEventArgs(previousLocalPosition, localPosition));
    }

    public override void populateClickableComponentList()
    {
        // The base class does a bunch of nasty reflection to populate the list, none of which is compatible with how
        // this menu works. To save time, we can simply do nothing here.
    }

    public override void receiveGamePadButton(Buttons b)
    {
        // When the game performs updateActiveMenu, it checks areGamePadControlsImplemented(), and if false, translates
        // those buttons into clicks.
        //
        // We still receive this event regardless of areGamePadControlsImplemented(), but letting updateActiveMenu
        // convert the A and X button presses into clicks makes receiveLeftClick and receiveRightClick fire repeatedly
        // as the button is held, which is generally the behavior that users will be accustomed to. If we override the
        // gamepad controls then we'd have to reimplement the repeating-click logic.

        switch (b.ToSButton())
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
        }
    }

    public override void receiveKeyPress(Keys key)
    {
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
        if (key == Keys.Escape
            || Game1.keyboardDispatcher.Subscriber is not ICaptureTarget
            || (Game1.options.gamepadControls && Utility.getHeldButtons(Game1.input.GetGamePadState()).Count > 0))
        {
            base.receiveKeyPress(key);
        }
    }

    public override void receiveLeftClick(int x, int y, bool playSound = true)
    {
        InitiateClick(SButton.MouseLeft, new(x, y));
    }

    public override void receiveRightClick(int x, int y, bool playSound = true)
    {
        InitiateClick(SButton.MouseRight, new(x, y));
    }

    public override void receiveScrollWheelAction(int value)
    {
        // IClickableMenu calls the value "direction" but it is actually a magnitude, and always in the Y direction
        // (negative is down).
        var direction = value > 0 ? Direction.North : Direction.South;
        InitiateWheel(direction);
    }

    public override void releaseLeftClick(int x, int y)
    {
        var mousePosition = new Point(x, y);
        previousDragPosition = mousePosition;
        var origin = new Point(xPositionOnScreen, yPositionOnScreen);
        var localPosition = (mousePosition - origin).ToVector2();
        view.OnDrop(new(localPosition));
    }

    private static ViewChild? GetDefaultFocus(ViewChild parent)
    {
        if (parent.View.IsFocusable)
        {
            return parent;
        }
        foreach (var child in parent.View.GetChildren())
        {
            var childFocus = GetDefaultFocus(child.Offset(parent.Position));
            if (childFocus is not null)
            {
                return childFocus;
            }
        }
        return null;
    }

    private static Point GetViewportSize()
    {
        var maxViewport = Game1.graphics.GraphicsDevice.Viewport;
        return Game1.uiViewport.Width <= maxViewport.Width
            ? new(Game1.uiViewport.Width, Game1.uiViewport.Height)
            : new(maxViewport.Width, maxViewport.Height);
    }

    private void InitiateClick(SButton button, Point screenPoint)
    {
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
    }

    private void InitiateWheel(Direction direction)
    {
        var mousePosition = Game1.input.GetMouseState().Position;
        var origin = new Point(xPositionOnScreen, yPositionOnScreen);
        var localPosition = (mousePosition - origin).ToVector2();
        var pathBeforeScroll = view.GetPathToPosition(localPosition).Select(child => child.View).ToList();
        var args = new WheelEventArgs(localPosition, direction);
        view.OnWheel(args);
        if (!args.Handled)
        {
            return;
        }
        Game1.playSound("shiny4");
        if (Game1.options.gamepadControls && !Game1.lastCursorMotionWasMouse)
        {
            var pathAfterScroll = view.ResolveChildPath(pathBeforeScroll);
            var (targetView, bounds) = pathAfterScroll.Aggregate(
                (view as IView, bounds: Bounds.Empty),
                (acc, child) => (child.View, new(acc.bounds.Position + child.Position, child.View.OuterSize)));
            if (view.GetPathToPosition(bounds.Center()).LastOrDefault()?.View == targetView)
            {
                Game1.setMousePosition(origin + bounds.Center().ToPoint(), true);
            }
            else
            {
                // Can happen if the target view is no longer reachable, i.e. outside the scroll bounds.
                var validResult = view.FocusSearch((mousePosition - origin).ToVector2(), direction);
                if (validResult is not null)
                {
                    ReleaseCaptureTarget();
                    Game1.setMousePosition(origin + validResult.Target.Center(), true);
                }
            }
        }
    }

    [Conditional("DEBUG_FOCUS_SEARCH")]
    private static void LogFocusSearchResult(ViewChild? result)
    {
        Logger.Log($"Found: {result?.View.Name} ({result?.View.GetType().Name}) at {result?.Position}", LogLevel.Info);
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

    private static void ReleaseCaptureTarget()
    {
        if (Game1.keyboardDispatcher.Subscriber is ICaptureTarget captureTarget)
        {
            captureTarget.ReleaseCapture();
        }
    }
}
