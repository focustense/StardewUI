using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Utilities;
using StardewUI.Animation;
using StardewUI.Events;
using StardewUI.Graphics;
using StardewUI.Input;
using StardewUI.Layout;
using StardewUI.Overlays;
using StardewUI.Widgets.Keybinding;
using StardewValley;

namespace StardewUI.Widgets;

/// <summary>
/// An overlay that can be used to edit the position of some arbitrary content.
/// </summary>
/// <remarks>
/// Note that the widget only provides a means to visually/interactively obtain a new position, similar to e.g.
/// obtaining a text string from a modal input query. It is up to the caller to persist the resulting
/// <see cref="ContentPlacement"/> to configuration and determine how to actually position the content in its usual
/// context (e.g. game HUD).
/// </remarks>
/// <param name="buttonSpriteMap">Map of buttons to button prompt sprites.</param>
/// <param name="directionSpriteMap">Map of directions to directional arrow sprites; used to indicate dragging.</param>
public class PositioningOverlay(ISpriteMap<SButton>? buttonSpriteMap, ISpriteMap<Direction>? directionSpriteMap)
    : FullScreenOverlay
{
    /// <summary>
    /// Configures the mapping of buttons to positioning actions in a <see cref="PositioningOverlay"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// For all <see cref="IReadOnlyList{T}"/> properties, <b>any</b> of the buttons can be pressed in order to perform
    /// that function; it is primarily intended to support left-stick/d-pad equivalency and WASD/arrow-key equivalency.
    /// Button combinations are not supported.
    /// </para>
    /// <para>
    /// Keyboard control schemes only specify the fine movements; alignments are always controlled using number/numpad
    /// keys for each of the 9 possibilities.
    /// </para>
    /// </remarks>
    public class ControlScheme
    {
        /// <summary>
        /// Buttons to shift the content down one pixel by modifying the <see cref="NineGridPlacement.Offset"/> of the
        /// <see cref="ContentPlacement"/>.
        /// </summary>
        public IReadOnlyList<SButton> FineDown { get; init; } = [];

        /// <summary>
        /// Buttons to shift the content left one pixel by modifying the <see cref="NineGridPlacement.Offset"/> of the
        /// <see cref="ContentPlacement"/>.
        /// </summary>
        public IReadOnlyList<SButton> FineLeft { get; init; } = [];

        /// <summary>
        /// Buttons to shift the content right one pixel by modifying the <see cref="NineGridPlacement.Offset"/> of the
        /// <see cref="ContentPlacement"/>.
        /// </summary>
        public IReadOnlyList<SButton> FineRight { get; init; } = [];

        /// <summary>
        /// Buttons to shift the content up one pixel by modifying the <see cref="NineGridPlacement.Offset"/> of the
        /// <see cref="ContentPlacement"/>.
        /// </summary>
        public IReadOnlyList<SButton> FineUp { get; init; } = [];
    }

    /// <summary>
    /// Configures the mapping of buttons to positioning actions in a <see cref="PositioningOverlay"/>. Includes the
    /// generic <see cref="ControlScheme"/> settings as well as grid-movement settings specific to gamepads.
    /// </summary>
    public class GamepadControlScheme : ControlScheme
    {
        /// <summary>
        /// Buttons to shift the content down by one grid cell by changing the
        /// <see cref="NineGridPlacement.VerticalAlignment"/> of the <see cref="ContentPlacement"/>.
        /// <see cref="Alignment.Start"/> becomes <see cref="Alignment.Middle"/> and <see cref="Alignment.Middle"/>
        /// becomes <see cref="Alignment.End"/>.
        /// </summary>
        public IReadOnlyList<SButton> GridDown { get; init; } = [];

        /// <summary>
        /// Buttons to shift the content left by one grid cell by changing the
        /// <see cref="NineGridPlacement.HorizontalAlignment"/> of the <see cref="ContentPlacement"/>.
        /// <see cref="Alignment.End"/> becomes <see cref="Alignment.Middle"/> and <see cref="Alignment.Middle"/>
        /// becomes <see cref="Alignment.Start"/>.
        /// </summary>
        public IReadOnlyList<SButton> GridLeft { get; init; } = [];

        /// <summary>
        /// Buttons to shift the content right by one grid cell by changing the
        /// <see cref="NineGridPlacement.HorizontalAlignment"/> of the <see cref="ContentPlacement"/>.
        /// <see cref="Alignment.Start"/> becomes <see cref="Alignment.Middle"/> and <see cref="Alignment.Middle"/>
        /// becomes <see cref="Alignment.End"/>.
        /// </summary>
        public IReadOnlyList<SButton> GridRight { get; init; } = [];

        /// <summary>
        /// Buttons to shift the content up by one grid cell by changing the
        /// <see cref="NineGridPlacement.VerticalAlignment"/> of the <see cref="ContentPlacement"/>.
        /// <see cref="Alignment.End"/> becomes <see cref="Alignment.Middle"/> and <see cref="Alignment.Middle"/>
        /// becomes <see cref="Alignment.Start"/>.
        /// </summary>
        public IReadOnlyList<SButton> GridUp { get; init; } = [];

        /// <summary>
        /// Modifier key to switch between grid- and fine-positioning modes.
        /// </summary>
        /// <remarks>
        /// If specified, the default motion will be fine, and the modifier key must be held in order to move accross
        /// the grid.
        /// </remarks>
        public SButton Modifier { get; init; }
    }

    /// <summary>
    /// The content that is being positioned.
    /// </summary>
    /// <remarks>
    /// This is normally a "representative" version of the real content, as the true HUD widget or other element may not
    /// exist or have its properties known at configuration time.
    /// </remarks>
    public IView? Content { get; set; }

    /// <summary>
    /// Current placement of the <see cref="Content"/> within the viewport.
    /// </summary>
    public NineGridPlacement ContentPlacement { get; set; } = new(Alignment.Start, Alignment.Start);

    /// <summary>
    /// The control scheme to use when positioning with a gamepad.
    /// </summary>
    public GamepadControlScheme GamepadControls { get; init; } =
        new()
        {
            FineDown = [SButton.LeftThumbstickDown, SButton.DPadDown],
            FineLeft = [SButton.LeftThumbstickLeft, SButton.DPadLeft],
            FineRight = [SButton.LeftThumbstickRight, SButton.DPadRight],
            FineUp = [SButton.LeftThumbstickUp, SButton.DPadUp],
            GridDown = [SButton.RightShoulder],
            GridLeft = [SButton.LeftTrigger],
            GridRight = [SButton.RightTrigger],
            GridUp = [SButton.LeftShoulder],
        };

    /// <summary>
    /// The control scheme to use when positioning with keyboard/mouse.
    /// </summary>
    /// <remarks>
    /// </remarks>
    public ControlScheme KeyboardControls { get; init; } =
        new()
        {
            FineDown = [SButton.S, SButton.Down],
            FineLeft = [SButton.A, SButton.Left],
            FineRight = [SButton.D, SButton.Right],
            FineUp = [SButton.W, SButton.Up],
        };

    private readonly ISpriteMap<SButton>? buttonSpriteMap = buttonSpriteMap;
    private readonly ISpriteMap<Direction>? directionSpriteMap = directionSpriteMap;

    /// <inheritdoc />
    protected override IView CreateView()
    {
        var actionState = BuildActionState();
        var view = new PositioningView(this, actionState);
        view.RightClick += View_RightClick;
        return view;
    }

    private ActionState<PositioningAction> BuildActionState()
    {
        return new ActionState<PositioningAction>()
            .Bind([.. KeyboardControls.FineUp, .. GamepadControls.FineUp], Nudge(Direction.North), suppress: false)
            .Bind([.. KeyboardControls.FineDown, .. GamepadControls.FineDown], Nudge(Direction.South), suppress: false)
            .Bind([.. KeyboardControls.FineLeft, .. GamepadControls.FineLeft], Nudge(Direction.West), suppress: false)
            .Bind([.. KeyboardControls.FineRight, .. GamepadControls.FineRight], Nudge(Direction.East), suppress: false)
            .Bind(GamepadControls.GridUp, Snap(Direction.North))
            .Bind(GamepadControls.GridDown, Snap(Direction.South))
            .Bind(GamepadControls.GridLeft, Snap(Direction.West))
            .Bind(GamepadControls.GridRight, Snap(Direction.East))
            .Bind([SButton.D1, SButton.NumPad1], Align(Alignment.Start, Alignment.End))
            .Bind([SButton.D2, SButton.NumPad2], Align(Alignment.Middle, Alignment.End))
            .Bind([SButton.D3, SButton.NumPad3], Align(Alignment.End, Alignment.End))
            .Bind([SButton.D4, SButton.NumPad4], Align(Alignment.Start, Alignment.Middle))
            .Bind([SButton.D5, SButton.NumPad5], Align(Alignment.Middle, Alignment.Middle))
            .Bind([SButton.D6, SButton.NumPad6], Align(Alignment.End, Alignment.Middle))
            .Bind([SButton.D7, SButton.NumPad7], Align(Alignment.Start, Alignment.Start))
            .Bind([SButton.D8, SButton.NumPad8], Align(Alignment.Middle, Alignment.Start))
            .Bind([SButton.D9, SButton.NumPad9], Align(Alignment.End, Alignment.Start));

        PositioningAction.Align Align(Alignment horizontal, Alignment vertical) => new(horizontal, vertical);
        PositioningAction.Nudge Nudge(Direction direction) => new(direction);
        PositioningAction.Snap Snap(Direction direction) => new(direction);
    }

    private void View_RightClick(object? sender, ClickEventArgs e)
    {
        Overlay.Remove(this);
    }

    abstract record PositioningAction
    {
        private PositioningAction() { }

        public abstract NineGridPlacement? Apply(NineGridPlacement placement);

        public record Align(Alignment Horizontal, Alignment Vertical) : PositioningAction
        {
            public override NineGridPlacement? Apply(NineGridPlacement placement)
            {
                return new NineGridPlacement(Horizontal, Vertical, Point.Zero);
            }
        }

        public record Nudge(Direction Direction) : PositioningAction
        {
            public override NineGridPlacement? Apply(NineGridPlacement placement)
            {
                return placement.Nudge(Direction);
            }
        }

        public record Snap(Direction Direction) : PositioningAction, IEquatable<Snap>
        {
            public override NineGridPlacement? Apply(NineGridPlacement placement)
            {
                return placement.Snap(Direction, avoidMiddle: true);
            }
        }
    }

    class PositioningView(PositioningOverlay owner, ActionState<PositioningAction> actionState) : ComponentView<Panel>
    {
        private static readonly Color KeyTint = new(0.5f, 0.5f, 0.5f, 0.5f);
        private static readonly Color MouseTint = new(0.5f, 0.5f, 0.5f, 0.5f);

        private readonly Frame contentFrame =
            new()
            {
                Layout = LayoutParameters.Fill(),
                ZIndex = 2,
                Draggable = true,
            };

        private readonly GhostView dragContent = new() { TintColor = Color.Green, Visibility = Visibility.Hidden };

        private readonly Lane dragPrompt =
            new()
            {
                Layout = LayoutParameters.FitContent(),
                VerticalContentAlignment = Alignment.Middle,
                Children =
                [
                    new Image()
                    {
                        Layout = LayoutParameters.FixedSize(64, 64),
                        Sprite = owner.directionSpriteMap?.Get(Direction.West, out _),
                        Tint = MouseTint,
                    },
                    new Image()
                    {
                        Layout = LayoutParameters.FixedSize(100, 100),
                        Sprite = owner.buttonSpriteMap?.Get(SButton.MouseLeft, out _),
                        Tint = MouseTint,
                    },
                    new Image()
                    {
                        Layout = LayoutParameters.FixedSize(64, 64),
                        Sprite = owner.directionSpriteMap?.Get(Direction.East, out _),
                        Tint = MouseTint,
                    },
                ],
            };

        private bool wasGamepadControls = Game1.options.gamepadControls;

        // Initialized in CreateView
        private Panel alignmentPromptsPanel = null!;
        private Lane controllerMovementPromptsLane = null!;
        private SpriteAnimator dpadAnimator = null!;
        private Image dpadImage = null!;
        private Lane keyboardMovementPromptsLane = null!;
        private Frame movementPromptsFrame = null!;

        public override void OnUpdate(TimeSpan elapsed)
        {
            if (Game1.options.gamepadControls != wasGamepadControls)
            {
                CreateAlignmentPrompts();
                UpdateDirectionalPrompts();
                if (Game1.options.gamepadControls)
                {
                    dpadAnimator.Reset();
                }
                wasGamepadControls = Game1.options.gamepadControls;
            }
            contentFrame.Content = owner.Content;
            contentFrame.HorizontalContentAlignment = owner.ContentPlacement.HorizontalAlignment;
            contentFrame.VerticalContentAlignment = owner.ContentPlacement.VerticalAlignment;
            contentFrame.Margin = owner.ContentPlacement.GetMargin();
            dragContent.RealView = owner.Content;
            actionState.Tick(elapsed);
            base.OnUpdate(elapsed);
            HandleCurrentActions();
        }

        protected override Panel CreateView()
        {
            alignmentPromptsPanel = new Panel() { Layout = LayoutParameters.Fill() };
            CreateAlignmentPrompts();
            movementPromptsFrame = new Frame() { Layout = LayoutParameters.FitContent() };
            var wasdPrompt = CreateDirectionalPrompt();
            dpadImage = new() { Layout = LayoutParameters.FixedSize(100, 100), Tint = KeyTint };
            dpadAnimator = new(dpadImage)
            {
                Frames = owner.buttonSpriteMap is not null
                    ?
                    [
                        // Sleep is used as substitute for (non-existing) "d-pad no press" button.
                        owner.buttonSpriteMap.Get(SButton.Sleep, out _),
                        owner.buttonSpriteMap.Get(SButton.DPadUp, out _),
                        owner.buttonSpriteMap.Get(SButton.DPadRight, out _),
                        owner.buttonSpriteMap.Get(SButton.DPadDown, out _),
                        owner.buttonSpriteMap.Get(SButton.DPadLeft, out _),
                        owner.buttonSpriteMap.Get(SButton.DPadUp, out _),
                    ]
                    : [],
                FrameDuration = TimeSpan.FromMilliseconds(250),
                StartDelay = TimeSpan.FromSeconds(4),
            };
            var stickImage = new Image()
            {
                Layout = LayoutParameters.FixedSize(100, 100),
                Margin = new(Right: 64),
                Sprite = owner.buttonSpriteMap?.Get(SButton.LeftThumbstickUp, out _),
                Tint = KeyTint,
            };
            controllerMovementPromptsLane = new Lane()
            {
                Layout = LayoutParameters.FitContent(),
                Children = [stickImage, dpadImage],
            };
            keyboardMovementPromptsLane = new Lane()
            {
                Layout = LayoutParameters.FitContent(),
                Orientation = Orientation.Vertical,
                HorizontalContentAlignment = Alignment.Middle,
                Children = [dragPrompt, new Spacer() { Layout = LayoutParameters.FixedSize(0, 64) }, wasdPrompt],
            };
            UpdateDirectionalPrompts();
            contentFrame.DragStart += ContentFrame_DragStart;
            contentFrame.Drag += ContentFrame_Drag;
            contentFrame.DragEnd += ContentFrame_DragEnd;
            // Creating a stretched frame for the drag content isn't strictly necessary for moving the content, but
            // makes it easier to calculate position since the content will be aligned to the top left instead of
            // the viewport center.
            var dragFrame = new Frame()
            {
                Layout = LayoutParameters.Fill(),
                Content = dragContent,
                PointerEventsEnabled = false,
                ZIndex = 3,
            };
            return new Panel()
            {
                Layout = LayoutParameters.FixedSize(UiViewport.GetMaxSize()),
                HorizontalContentAlignment = Alignment.Middle,
                VerticalContentAlignment = Alignment.Middle,
                Children = [contentFrame, alignmentPromptsPanel, movementPromptsFrame, dragFrame],
            };
        }

        private Vector2? dragContentOffset;

        private void ContentFrame_Drag(object? sender, PointerEventArgs e)
        {
            if (dragContentOffset is null)
            {
                return;
            }
            var origin = e.Position - dragContentOffset.Value;
            dragContent.Margin = new(Left: (int)origin.X, Top: (int)origin.Y);
            e.Handled = true;
        }

        private void ContentFrame_DragEnd(object? sender, PointerEventArgs e)
        {
            if (dragContentOffset is null)
            {
                return;
            }
            Game1.playSound("stoneStep");
            dragContent.Visibility = Visibility.Hidden;
            alignmentPromptsPanel.Visibility = Visibility.Visible;
            movementPromptsFrame.Visibility = Visibility.Visible;
            var origin = e.Position - dragContentOffset.Value;
            DropAtPosition(origin);
            e.Handled = true;
        }

        private void ContentFrame_DragStart(object? sender, PointerEventArgs e)
        {
            if (contentFrame.Content is null || contentFrame.GetChildAt(e.Position) is not ViewChild contentChild)
            {
                dragContentOffset = null;
                return;
            }
            Game1.playSound("drumkit6");
            dragContentOffset = e.Position - contentChild.Position;
            alignmentPromptsPanel.Visibility = Visibility.Hidden;
            movementPromptsFrame.Visibility = Visibility.Hidden;
            dragContent.Visibility = Visibility.Visible;
            e.Handled = true;
        }

        private static IView CreateAlignmentPrompt(NineGridPlacement placement, IView content)
        {
            return new Frame()
            {
                Layout = LayoutParameters.Fill(),
                HorizontalContentAlignment = placement.HorizontalAlignment,
                VerticalContentAlignment = placement.VerticalAlignment,
                Content = content,
            };
        }

        private void CreateAlignmentPrompts()
        {
            if (Game1.options.gamepadControls)
            {
                alignmentPromptsPanel.Children = owner
                    .ContentPlacement.GetNeighbors(avoidMiddle: true)
                    .Select(p => CreateAlignmentPrompt(p.Placement, CreateControllerButtonPrompt(p.Direction)))
                    .ToList();
            }
            else
            {
                alignmentPromptsPanel.Children = NineGridPlacement
                    .StandardPlacements.Select(
                        (p, i) =>
                            CreateAlignmentPrompt(
                                p,
                                CreateButtonPrompt(
                                    SButton.NumPad1 + i,
                                    visible: !p.IsMiddle() && !p.EqualsIgnoringOffset(owner.ContentPlacement)
                                )
                            )
                    )
                    .ToList();
            }
        }

        private IView CreateButtonPrompt(SButton button, Edges? margin = null, bool visible = true)
        {
            return CreateButtonPrompt(new Keybind(button), margin, visible);
        }

        private IView CreateButtonPrompt(Keybind keybind, Edges? margin = null, bool visible = true)
        {
            return new KeybindView
            {
                Layout = new()
                {
                    Width = Length.Content(),
                    Height = Length.Content(),
                    MinWidth = 100,
                },
                Margin = margin ?? Edges.NONE,
                ButtonHeight = 100,
                ButtonMinWidth = 100,
                Font = Game1.dialogueFont,
                SpriteMap = owner.buttonSpriteMap,
                TextColor = Color.Gray,
                TintColor = KeyTint,
                Keybind = keybind,
                Visibility = visible ? Visibility.Visible : Visibility.Hidden,
            };
        }

        private IView CreateControllerButtonPrompt(Direction snapDirection)
        {
            var action = new PositioningAction.Snap(snapDirection);
            var binding = actionState.GetControllerBindings(action).FirstOrDefault();
            return CreateButtonPrompt(binding ?? new());
        }

        private Lane CreateDirectionalPrompt()
        {
            var northBinding = GetKeyboardNudgeBinding(Direction.North);
            var southBinding = GetKeyboardNudgeBinding(Direction.South);
            var westBinding = GetKeyboardNudgeBinding(Direction.West);
            var eastBinding = GetKeyboardNudgeBinding(Direction.East);
            return new()
            {
                Layout = LayoutParameters.FitContent(),
                HorizontalContentAlignment = Alignment.Middle,
                Orientation = Orientation.Vertical,
                Children =
                [
                    CreateButtonPrompt(northBinding),
                    new Lane()
                    {
                        Layout = LayoutParameters.FitContent(),
                        VerticalContentAlignment = Alignment.Middle,
                        Children =
                        [
                            CreateButtonPrompt(westBinding, new(0, -16)),
                            new Spacer() { Layout = LayoutParameters.FixedSize(48, 0) },
                            CreateButtonPrompt(eastBinding, new(0, -16)),
                        ],
                    },
                    CreateButtonPrompt(southBinding),
                ],
            };
        }

        private void DropAtPosition(Vector2 position)
        {
            // Drops are special because they essentially lose the alignment contents and place the target at an
            // arbitrary position. We have to reconstitute the best/most likely combination of alignments and offset to
            // match the absolute position.
            //
            // A possibly naive but probably still mostly accurate answer is just to set the alignments to whichever
            // ends up closest, and use the offset to adjust from there.
            var centerDistanceX = position.X - OuterSize.X / 2f;
            var horizontalAlignment =
                centerDistanceX < 0
                    ? (position.X < -centerDistanceX)
                        ? Alignment.Start
                        : Alignment.Middle
                    : ((OuterSize.X - position.X) < centerDistanceX)
                        ? Alignment.End
                        : Alignment.Middle;
            var centerDistanceY = position.Y - OuterSize.Y / 2f;
            var verticalAlignment =
                centerDistanceY < 0
                    ? (position.Y < -centerDistanceY)
                        ? Alignment.Start
                        : Alignment.Middle
                    : ((OuterSize.Y - position.Y) < centerDistanceY)
                        ? Alignment.End
                        : Alignment.Middle;
            // The position we get is the top-left position, but the content offset is relative to the aligned edge.
            // To get the correct position we must also account for the content size.
            var contentSize = contentFrame.Content?.OuterSize ?? Vector2.Zero;
            // It might appear wrong that we are adding the content size instead of removing it, but we are actually
            // trying to "decompensate" for the content size since the normal layout/alignment will compensate.
            var placedX = horizontalAlignment switch
            {
                Alignment.Middle => position.X + contentSize.X / 2,
                Alignment.End => position.X + contentSize.X,
                _ => position.X,
            };
            var placedY = verticalAlignment switch
            {
                Alignment.Middle => position.Y + contentSize.Y / 2,
                Alignment.End => position.Y + contentSize.Y,
                _ => position.Y,
            };
            var placedPosition = new Vector2(placedX, placedY);
            owner.ContentPlacement = NineGridPlacement.AtPosition(
                placedPosition,
                OuterSize,
                horizontalAlignment,
                verticalAlignment
            );
            CreateAlignmentPrompts();
        }

        private Keybind GetKeyboardNudgeBinding(Direction direction)
        {
            var action = new PositioningAction.Nudge(direction);
            return actionState.GetKeyboardBindings(action).FirstOrDefault() ?? new();
        }

        private void HandleCurrentActions()
        {
            bool wasPlacementChanged = false;
            foreach (var action in actionState.GetCurrentActions())
            {
                var nextPlacement = action.Apply(owner.ContentPlacement);
                if (nextPlacement is not null)
                {
                    owner.ContentPlacement = nextPlacement;
                    wasPlacementChanged = true;
                }
            }
            if (wasPlacementChanged)
            {
                Game1.playSound("drumkit6");
                CreateAlignmentPrompts();
            }
        }

        private void UpdateDirectionalPrompts()
        {
            if (movementPromptsFrame is null)
            {
                return;
            }
            movementPromptsFrame.Content = Game1.options.gamepadControls
                ? controllerMovementPromptsLane
                : keyboardMovementPromptsLane;
        }
    }
}
