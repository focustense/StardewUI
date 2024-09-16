using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewValley;

namespace StardewUI.Widgets;

/// <summary>
/// An overlay that can be used to edit the position of some arbitrary content.
/// </summary>
/// <remarks>
/// <para>
/// The current position is controlled by the <see cref="HorizontalContentAlignment"/>,
/// <see cref="VerticalContentAlignment"/> and <see cref="ContentOffset"/>.
/// </para>
/// <para>
/// Note that the widget only provides a means to visually/interactively obtain a new position, similar to e.g.
/// obtaining a text string from a modal input query. It is up to the caller to persist these values to configuration
/// and determine how to actually position the content in its usual context (e.g. game HUD).
/// </para>
/// </remarks>
/// <param name="buttonSpriteMap">Map of buttons to button prompt sprites.</param>
/// <param name="directionSpriteMap">Map of directions to directional arrow sprites; used to indicate dragging.</param>
public class PositioningOverlay(ISpriteMap<SButton> buttonSpriteMap, ISpriteMap<Direction> directionSpriteMap)
    : FullScreenOverlay
{
    /// <summary>
    /// Configures the mapping of buttons to positioning actions in a <see cref="PositioningOverlay"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// For all <see cref="IReadOnlyList{T}"/> properties, <b>any</b> of the buttons can be pressed in order to perform
    /// that function; it is primarily intended to support left-stick/d-pad equivalency and WASD/arrow-key equivalency.
    /// Button combinations are not supported except as part of <see cref="Modifier"/>.
    /// </para>
    /// <para>
    /// Keyboard control schemes only specify the fine movements; alignments are always controlled using number/numpad
    /// keys for each of the 9 possibilities.
    /// </para>
    /// </remarks>
    public class ControlScheme
    {
        /// <summary>
        /// Buttons to shift the content down one pixel by modifying <see cref="ContentOffset"/>.
        /// </summary>
        public IReadOnlyList<SButton> FineDown { get; init; } = [];

        /// <summary>
        /// Buttons to shift the content left one pixel by modifying <see cref="ContentOffset"/>.
        /// </summary>
        public IReadOnlyList<SButton> FineLeft { get; init; } = [];

        /// <summary>
        /// Buttons to shift the content right one pixel by modifying <see cref="ContentOffset"/>.
        /// </summary>
        public IReadOnlyList<SButton> FineRight { get; init; } = [];

        /// <summary>
        /// Buttons to shift the content up one pixel by modifying <see cref="ContentOffset"/>.
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
        /// Buttons to shift the content down by one grid cell by changing <see cref="VerticalContentAlignment"/>.
        /// <see cref="Alignment.Start"/> becomes <see cref="Alignment.Middle"/> and <see cref="Alignment.Middle"/>
        /// becomes <see cref="Alignment.End"/>.
        /// </summary>
        public IReadOnlyList<SButton> GridDown { get; init; } = [];

        /// <summary>
        /// Buttons to shift the content left by one grid cell by changing <see cref="HorizontalContentAlignment"/>.
        /// <see cref="Alignment.End"/> becomes <see cref="Alignment.Middle"/> and <see cref="Alignment.Middle"/>
        /// becomes <see cref="Alignment.Start"/>.
        /// </summary>
        public IReadOnlyList<SButton> GridLeft { get; init; } = [];

        /// <summary>
        /// Buttons to shift the content right by one grid cell by changing <see cref="HorizontalContentAlignment"/>.
        /// <see cref="Alignment.Start"/> becomes <see cref="Alignment.Middle"/> and <see cref="Alignment.Middle"/>
        /// becomes <see cref="Alignment.End"/>.
        /// </summary>
        public IReadOnlyList<SButton> GridRight { get; init; } = [];

        /// <summary>
        /// Buttons to shift the content up by one grid cell by changing <see cref="VerticalContentAlignment"/>.
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

    private readonly ISpriteMap<SButton> buttonSpriteMap = buttonSpriteMap;
    private readonly ISpriteMap<Direction> directionSpriteMap = directionSpriteMap;

    protected override IView CreateView()
    {
        var actionState = BuildActionState();
        return new PositioningView(this, actionState);
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

    abstract class PositioningAction
    {
        private PositioningAction() { }

        public abstract NineGridPlacement? Apply(NineGridPlacement placement);

        public class Align(Alignment horizontal, Alignment vertical) : PositioningAction
        {
            public override NineGridPlacement? Apply(NineGridPlacement placement)
            {
                return new NineGridPlacement(horizontal, vertical, Point.Zero);
            }
        }

        public class Nudge(Direction direction) : PositioningAction
        {
            public override NineGridPlacement? Apply(NineGridPlacement placement)
            {
                return placement.Nudge(direction);
            }
        }

        public class Snap(Direction direction) : PositioningAction
        {
            public override NineGridPlacement? Apply(NineGridPlacement placement)
            {
                return placement.Snap(direction, avoidMiddle: true);
            }
        }
    }

    class PositioningView(PositioningOverlay owner, ActionState<PositioningAction> actionState) : WrapperView<Panel>
    {
        private record LabeledButton(SButton Button, string Label);

        private static readonly Color KeyTint = new(0.5f, 0.5f, 0.5f, 0.5f);
        private static readonly Color MouseTint = new(0.5f, 0.5f, 0.5f, 0.5f);

        private readonly Frame contentFrame = new() { Layout = LayoutParameters.Fill(), ZIndex = 2 };

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
                        Sprite = owner.directionSpriteMap.Get(Direction.West, out _),
                        Tint = MouseTint,
                    },
                    new Image()
                    {
                        Layout = LayoutParameters.FixedSize(100, 100),
                        Sprite = owner.buttonSpriteMap.Get(SButton.MouseLeft, out _),
                        Tint = MouseTint,
                    },
                    new Image()
                    {
                        Layout = LayoutParameters.FixedSize(64, 64),
                        Sprite = owner.directionSpriteMap.Get(Direction.East, out _),
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
            actionState.Tick(elapsed);
            base.OnUpdate(elapsed);
            HandleCurrentActions();
        }

        protected override Panel CreateView()
        {
            alignmentPromptsPanel = new Panel() { Layout = LayoutParameters.Fill() };
            CreateAlignmentPrompts();
            movementPromptsFrame = new Frame() { Layout = LayoutParameters.FitContent() };
            var wasdPrompt = CreateDirectionalPrompt(
                new(SButton.W, "W"),
                new(SButton.A, "A"),
                new(SButton.S, "S"),
                new(SButton.D, "D")
            );
            dpadImage = new() { Layout = LayoutParameters.FixedSize(100, 100), Tint = KeyTint };
            dpadAnimator = new(dpadImage)
            {
                Frames =
                [
                    // Sleep is used as substitute for (non-existing) "d-pad no press" button.
                    owner.buttonSpriteMap.Get(SButton.Sleep, out _),
                    owner.buttonSpriteMap.Get(SButton.DPadUp, out _),
                    owner.buttonSpriteMap.Get(SButton.DPadRight, out _),
                    owner.buttonSpriteMap.Get(SButton.DPadDown, out _),
                    owner.buttonSpriteMap.Get(SButton.DPadLeft, out _),
                    owner.buttonSpriteMap.Get(SButton.DPadUp, out _),
                ],
                FrameDuration = TimeSpan.FromMilliseconds(250),
                StartDelay = TimeSpan.FromSeconds(4),
            };
            var stickImage = new Image()
            {
                Layout = LayoutParameters.FixedSize(100, 100),
                Margin = new(Right: 64),
                Sprite = owner.buttonSpriteMap.Get(SButton.LeftThumbstickUp, out _),
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
            return new Panel()
            {
                Layout = LayoutParameters.FixedSize(GetViewportSize()),
                HorizontalContentAlignment = Alignment.Middle,
                VerticalContentAlignment = Alignment.Middle,
                Children = [contentFrame, alignmentPromptsPanel, movementPromptsFrame],
            };
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
                    .Select(p => CreateAlignmentPrompt(p.Placement, CreateButtonPrompt(p.Direction)))
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
                                    (i + 1).ToString(),
                                    visible: !p.IsMiddle() && !p.EqualsIgnoringOffset(owner.ContentPlacement)
                                )
                            )
                    )
                    .ToList();
            }
        }

        private IView CreateButtonPrompt(Direction direction)
        {
            return direction switch
            {
                Direction.North => CreateButtonPrompt(SButton.LeftShoulder, "LB"),
                Direction.South => CreateButtonPrompt(SButton.RightShoulder, "RB"),
                Direction.West => CreateButtonPrompt(SButton.LeftTrigger, "LT"),
                Direction.East => CreateButtonPrompt(SButton.RightTrigger, "RT"),
                _ => throw new ArgumentException($"Invalid direction: {direction}", nameof(direction)),
            };
        }

        private IView CreateButtonPrompt(SButton button, string text, Edges? margin = null, bool visible = true)
        {
            var sprite = owner.buttonSpriteMap.Get(button, out var isPlaceholder);
            return new Frame()
            {
                Layout = LayoutParameters.FixedSize(100, 100),
                Margin = margin ?? Edges.NONE,
                Background = sprite,
                BackgroundTint = KeyTint,
                HorizontalContentAlignment = Alignment.Middle,
                VerticalContentAlignment = Alignment.Middle,
                Content = isPlaceholder ? Label.Simple(text, Game1.dialogueFont, Color.Gray) : null,
                Visibility = visible ? Visibility.Visible : Visibility.Hidden,
            };
        }

        private Lane CreateDirectionalPrompt(
            LabeledButton up,
            LabeledButton left,
            LabeledButton down,
            LabeledButton right
        )
        {
            return new()
            {
                Layout = LayoutParameters.FitContent(),
                HorizontalContentAlignment = Alignment.Middle,
                Orientation = Orientation.Vertical,
                Children =
                [
                    CreateButtonPrompt(up.Button, up.Label),
                    new Lane()
                    {
                        Layout = LayoutParameters.FitContent(),
                        VerticalContentAlignment = Alignment.Middle,
                        Children =
                        [
                            CreateButtonPrompt(left.Button, left.Label, new(0, -16)),
                            new Spacer() { Layout = LayoutParameters.FixedSize(48, 0) },
                            CreateButtonPrompt(right.Button, right.Label, new(0, -16)),
                        ],
                    },
                    CreateButtonPrompt(down.Button, down.Label),
                ],
            };
        }

        private static Point GetViewportSize()
        {
            var maxViewport = Game1.graphics.GraphicsDevice.Viewport;
            return Game1.uiViewport.Width <= maxViewport.Width
                ? new(Game1.uiViewport.Width, Game1.uiViewport.Height)
                : new(maxViewport.Width, maxViewport.Height);
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
