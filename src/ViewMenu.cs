using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Menus;

namespace StardewUI;

/// <summary>
/// Generic menu implementation based on a root <see cref="IView"/>.
/// </summary>
public abstract class ViewMenu<T> : IClickableMenu, IDisposable
    where T : IView
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

    private readonly Edges? gutter;
    private readonly OverlayContext overlayContext = new();
    private readonly ConditionalWeakTable<IOverlay, OverlayLayoutData> overlayCache = [];
    private readonly T view;
    private readonly bool wasHudDisplayed;

    private ViewChild[] hoverPath = [];
    private Point previousHoverPosition;
    private Point previousDragPosition;

    public ViewMenu(Edges? gutter = null, bool forceDefaultFocus = false)
    {
        Game1.playSound("bigSelect");

        this.gutter = gutter;
        overlayContext.Pushed += OverlayContext_Pushed;
        view = CreateView();
        MeasureAndCenter();

        if (forceDefaultFocus || !Game1.lastCursorMotionWasMouse)
        {
            var initialFocus = GetDefaultFocus(new(view, Vector2.Zero));
            if (initialFocus is not null)
            {
                var focusPosition = initialFocus.CenterPoint();
                Game1.setMousePosition(new Point(xPositionOnScreen, yPositionOnScreen) + focusPosition, true);
            }
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
        using var _ = OverlayContext.PushContext(overlayContext);
        var direction = (Direction)directionValue;
        var mousePosition = Game1.input.GetMouseState().Position;
        OnViewOrOverlay(
            (view, origin) =>
            {
                var found = view.FocusSearch(mousePosition.ToVector2() - origin, direction);
                if (found is not null)
                {
                    FinishFocusSearch(view, origin.ToPoint(), found);
                }
            }
        );
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
        var viewportBounds = Game1.graphics.GraphicsDevice.Viewport.Bounds;
        if (!Game1.options.showClearBackgrounds)
        {
            b.Draw(Game1.fadeToBlackRect, viewportBounds, Color.Black * DimmingAmount);
        }

        using var _ = OverlayContext.PushContext(overlayContext);

        MeasureAndCenter();

        var origin = new Point(xPositionOnScreen, yPositionOnScreen);
        var viewBatch = new PropagatedSpriteBatch(b, Transform.FromTranslation(origin.ToVector2()));
        view.Draw(viewBatch);

        foreach (var overlay in overlayContext.BackToFront())
        {
            var overlayData = GetOverlayLayoutData(overlay);
            var availableOverlaySize = viewportBounds.Size.ToVector2() - overlayData.Position;
            overlay.View.Measure(availableOverlaySize);
            var overlayBatch = new PropagatedSpriteBatch(b, Transform.FromTranslation(overlayData.Position));
            overlay.View.Draw(overlayBatch);
        }

        var tooltip = FormatTooltip(hoverPath);
        if (!string.IsNullOrEmpty(tooltip))
        {
            drawToolTip(b, tooltip, null, null);
        }

        Game1.mouseCursorTransparency = 1.0f;
        drawMouse(b);
    }

    public override void leftClickHeld(int x, int y)
    {
        if (!Game1.lastCursorMotionWasMouse)
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

    public override void performHoverAction(int x, int y)
    {
        if (previousHoverPosition.X == x && previousHoverPosition.Y == y)
        {
            return;
        }
        using var _ = OverlayContext.PushContext(overlayContext);
        OnViewOrOverlay((view, origin) => PerformHoverAction(view, origin, x, y));
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

        using var _ = OverlayContext.PushContext(overlayContext);

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

    private void OnOverlayRemoved(IOverlay overlay)
    {
        // A convenience for gamepad users is to try to move the mouse cursor back to whatever triggered the
        // overlay in the first place, e.g. the button on a drop-down list.
        // However, it's unnecessarily distracting to do it for mouse controls.
        if (Game1.lastCursorMotionWasMouse || overlay.Parent is null)
        {
            return;
        }
        var overlayData = GetOverlayLayoutData(overlay);
        var defaultFocus = GetDefaultFocus(new(overlay.Parent, overlayData.ParentBounds.Position));
        if (defaultFocus is not null)
        {
            Game1.setMousePosition(defaultFocus.CenterPoint(), true);
        }
    }

    public override void receiveKeyPress(Keys key)
    {
        // Note: Key "E" is the gamepad version of "Escape", apparently.
        if ((key == Keys.Escape || key == Keys.E) && overlayContext.Pop() is not null)
        {
            return;
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
            || (Game1.options.gamepadControls && Utility.getHeldButtons(Game1.input.GetGamePadState()).Count > 0)
        )
        {
            base.receiveKeyPress(key);
        }
    }

    public override void receiveLeftClick(int x, int y, bool playSound = true)
    {
        using var _ = OverlayContext.PushContext(overlayContext);
        InitiateClick(SButton.MouseLeft, new(x, y));
    }

    public override void receiveRightClick(int x, int y, bool playSound = true)
    {
        using var _ = OverlayContext.PushContext(overlayContext);
        InitiateClick(SButton.MouseRight, new(x, y));
    }

    public override void receiveScrollWheelAction(int value)
    {
        using var _ = OverlayContext.PushContext(overlayContext);
        // IClickableMenu calls the value "direction" but it is actually a magnitude, and always in the Y direction
        // (negative is down).
        var direction = value > 0 ? Direction.North : Direction.South;
        InitiateWheel(direction);
    }

    public override void releaseLeftClick(int x, int y)
    {
        using var _ = OverlayContext.PushContext(overlayContext);
        var mousePosition = new Point(x, y);
        previousDragPosition = mousePosition;
        OnViewOrOverlay((view, origin) => view.OnDrop(new(mousePosition.ToVector2() - origin)));
    }

    protected virtual string? FormatTooltip(IEnumerable<ViewChild> path)
    {
        var tooltip = hoverPath.Select(x => x.View.Tooltip).LastOrDefault(tooltip => !string.IsNullOrEmpty(tooltip));
        return Game1.parseText(tooltip, Game1.smallFont, 640);
    }

    private static void FinishFocusSearch(IView rootView, Point origin, FocusSearchResult found)
    {
        LogFocusSearchResult(found.Target);
        ReleaseCaptureTarget();
        Game1.playSound("shiny4");
        var nextMousePosition = origin + found.Target.CenterPoint();
        if (rootView.ScrollIntoView(found.Path, out var distance))
        {
            nextMousePosition -= distance.ToPoint();
        }
        Game1.setMousePosition(nextMousePosition, true);
    }

    private OverlayLayoutData GetOverlayLayoutData(IOverlay overlay)
    {
        var rootPosition = new Vector2(xPositionOnScreen, yPositionOnScreen);
        return overlayCache.GetValue(overlay, ov => OverlayLayoutData.FromOverlay(view, rootPosition, overlay));
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
        if (overlayContext.Front is IOverlay overlay)
        {
            var overlayData = GetOverlayLayoutData(overlay);
            var overlayLocalPosition = screenPoint.ToVector2() - overlayData.Position;
            if (
                overlayLocalPosition.X < 0
                || overlayLocalPosition.Y < 0
                || overlayLocalPosition.X >= overlay.View.OuterSize.X
                || overlayLocalPosition.Y >= overlay.View.OuterSize.Y
            )
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
    }

    private void InitiateWheel(Direction direction)
    {
        var mousePosition = Game1.input.GetMouseState().Position;
        OnViewOrOverlay(
            (view, origin) =>
            {
                var localPosition = mousePosition.ToVector2() - origin;
                var pathBeforeScroll = view.GetPathToPosition(localPosition).Select(child => child.View).ToList();
                var args = new WheelEventArgs(localPosition, direction);
                view.OnWheel(args);
                if (!args.Handled)
                {
                    return;
                }
                Game1.playSound("shiny4");
                Refocus(view, origin, localPosition, pathBeforeScroll, direction);
            }
        );
    }

    [Conditional("DEBUG_FOCUS_SEARCH")]
    private static void LogFocusSearchResult(ViewChild? result)
    {
        Logger.Log($"Found: {result?.View.Name} ({result?.View.GetType().Name}) at {result?.Position}", LogLevel.Info);
    }

    private void MeasureAndCenter()
    {
        var viewportSize = GetViewportSize();
        var availableMenuSize = viewportSize.ToVector2() - (gutter ?? DefaultGutter).Total;
        if (!view.Measure(availableMenuSize))
        {
            return;
        }
        width = (int)MathF.Round(view.OuterSize.X);
        height = (int)MathF.Round(view.OuterSize.Y);
        xPositionOnScreen = viewportSize.X / 2 - width / 2;
        yPositionOnScreen = viewportSize.Y / 2 - height / 2;
        Refocus();
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
        overlayContext.Front!.Close += Overlay_Close;
    }

    private void PerformHoverAction(IView rootView, Vector2 viewPosition, int mouseX, int mouseY)
    {
        var mousePosition = new Vector2(mouseX, mouseY);
        var localPosition = mousePosition - viewPosition;
        var previousLocalPosition = previousHoverPosition.ToVector2() - viewPosition;
        previousHoverPosition = new(mouseX, mouseY);
        hoverPath = rootView.GetPathToPosition(localPosition).ToArray();
        rootView.OnPointerMove(new PointerMoveEventArgs(previousLocalPosition, localPosition));
    }

    private void Refocus(Direction searchDirection = Direction.South)
    {
        if (hoverPath.Length == 0 || !Game1.options.gamepadControls || Game1.lastCursorMotionWasMouse)
        {
            return;
        }
        var previousLeaf = hoverPath.ToGlobalPositions().Last();
        OnViewOrOverlay(
            (view, origin) =>
            {
                var newLeaf = view.ResolveChildPath(hoverPath.Select(x => x.View)).ToGlobalPositions().LastOrDefault();
                if (
                    newLeaf?.View == previousLeaf.View
                    && (
                        newLeaf.Position != previousLeaf.Position
                        || newLeaf.View.OuterSize != previousLeaf.View.OuterSize
                    )
                )
                {
                    Refocus(
                        view,
                        origin,
                        previousHoverPosition.ToVector2(),
                        hoverPath.Select(x => x.View),
                        searchDirection
                    );
                }
            }
        );
    }

    private static void Refocus(
        IView root,
        Vector2 origin,
        Vector2 previousPosition,
        IEnumerable<IView> previousPath,
        Direction searchDirection
    )
    {
        if (!Game1.options.gamepadControls || Game1.lastCursorMotionWasMouse)
        {
            return;
        }
        var pathAfterScroll = root.ResolveChildPath(previousPath);
        var (targetView, bounds) = pathAfterScroll.Aggregate(
            (root as IView, bounds: Bounds.Empty),
            (acc, child) => (child.View, new(acc.bounds.Position + child.Position, child.View.OuterSize))
        );
        if (root.GetPathToPosition(bounds.Center()).LastOrDefault()?.View == targetView)
        {
            Game1.setMousePosition((origin + bounds.Center()).ToPoint(), true);
        }
        else
        {
            // Can happen if the target view is no longer reachable, i.e. outside the scroll bounds.
            var validResult = root.FocusSearch(previousPosition, searchDirection);
            if (validResult is not null)
            {
                ReleaseCaptureTarget();
                Game1.setMousePosition((origin + validResult.Target.Center()).ToPoint(), true);
            }
        }
    }

    private static void ReleaseCaptureTarget()
    {
        if (Game1.keyboardDispatcher.Subscriber is ICaptureTarget captureTarget)
        {
            captureTarget.ReleaseCapture();
        }
    }

    class OverlayLayoutData(ViewChild root)
    {
        public Bounds ParentBounds { get; set; } = Bounds.Empty;
        public ViewChild[] ParentPath { get; set; } = [];
        public Vector2 Position { get; set; }

        public static OverlayLayoutData FromOverlay(IView rootView, Vector2 rootPosition, IOverlay overlay)
        {
            var data = new OverlayLayoutData(new(rootView, rootPosition));
            data.Update(overlay);
            return data;
        }

        public void Update(IOverlay overlay)
        {
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
