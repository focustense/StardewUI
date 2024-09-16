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
    /// Pixel offset from the aligned position.
    /// </summary>
    /// <remarks>
    /// The offset is applied after <see cref="HorizontalContentAlignment"/> and <see cref="VerticalContentAlignment"/>;
    /// i.e. it is not an exact position on screen.
    /// </remarks>
    public Point ContentOffset { get; set; }

    /// <summary>
    /// The control scheme to use when positioning with a gamepad.
    /// </summary>
    public GamepadControlScheme GamepadControls { get; set; } =
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
    /// Horizontal alignment of the <see cref="Content"/> within the viewport.
    /// </summary>
    public Alignment HorizontalContentAlignment { get; set; }

    /// <summary>
    /// The control scheme to use when positioning with keyboard/mouse.
    /// </summary>
    /// <remarks>
    /// </remarks>
    public ControlScheme KeyboardControls { get; set; } =
        new()
        {
            FineDown = [SButton.S, SButton.Down],
            FineLeft = [SButton.A, SButton.Left],
            FineRight = [SButton.D, SButton.Right],
            FineUp = [SButton.W, SButton.Up],
        };

    /// <summary>
    /// Vertical alignment of the <see cref="Content"/> within the viewport.
    /// </summary>
    public Alignment VerticalContentAlignment { get; set; }

    private readonly ISpriteMap<SButton> buttonSpriteMap = buttonSpriteMap;
    private readonly ISpriteMap<Direction> directionSpriteMap = directionSpriteMap;

    protected override IView CreateView()
    {
        return new PositioningView(this);
    }

    class GhostView(IView realView, Color tintColor) : View
    {
        protected override void OnDrawContent(ISpriteBatch b)
        {
            using var _ = b.Blend(
                new BlendState()
                {
                    AlphaSourceBlend = Blend.One,
                    ColorSourceBlend = Blend.BlendFactor,
                    BlendFactor = tintColor,
                    AlphaDestinationBlend = Blend.InverseSourceAlpha,
                    ColorDestinationBlend = Blend.InverseSourceAlpha,
                }
            );
            realView.Draw(b);
        }

        protected override void OnMeasure(Vector2 availableSize)
        {
            ContentSize = realView.ContentBounds.Size;
        }
    }

    class PositioningView(PositioningOverlay owner) : WrapperView<Panel>
    {
        private record LabeledButton(SButton Button, string Label);

        // All possible horizontal/vertical alignment combinations.
        // There are "only" 9, so it's better to hardcode than to use reflection.
        // Ordered in the same order that the numpad/number buttons should refer to them.
        private static readonly IReadOnlyList<(Alignment horizontal, Alignment vertical)> alignments =
        [
            (Alignment.Start, Alignment.End),
            (Alignment.Middle, Alignment.End),
            (Alignment.End, Alignment.End),
            (Alignment.Start, Alignment.Middle),
            (Alignment.Middle, Alignment.Middle),
            (Alignment.End, Alignment.Middle),
            (Alignment.Start, Alignment.Start),
            (Alignment.Middle, Alignment.Start),
            (Alignment.End, Alignment.Start),
        ];

        private static readonly TimeSpan ButtonRepeatDelay = TimeSpan.FromMilliseconds(500);
        private static readonly TimeSpan ButtonRepeatInterval = TimeSpan.FromMilliseconds(50);

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

        private readonly Dictionary<SButton, Action> keybindings = [];

        private TimeSpan? timeSinceFirstInput;
        private TimeSpan? timeSinceLastInput;
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
            contentFrame.HorizontalContentAlignment = owner.HorizontalContentAlignment;
            contentFrame.VerticalContentAlignment = owner.VerticalContentAlignment;
            UpdateContentOffset();
            if (timeSinceFirstInput.HasValue)
            {
                timeSinceFirstInput += elapsed;
            }
            if (timeSinceLastInput.HasValue)
            {
                timeSinceLastInput += elapsed;
            }
            base.OnUpdate(elapsed);
            HandleInput();
        }

        private void UpdateContentOffset()
        {
            var (x, y) = owner.ContentOffset;
            var (marginLeft, marginRight) = contentFrame.HorizontalContentAlignment switch
            {
                Alignment.Start => (x, 0),
                Alignment.Middle => (x, -x),
                Alignment.End => (0, -x),
                _ => (0, 0),
            };
            var (marginTop, marginBottom) = contentFrame.VerticalContentAlignment switch
            {
                Alignment.Start => (y, 0),
                Alignment.Middle => (y, -y),
                Alignment.End => (0, -y),
                _ => (0, 0),
            };
            contentFrame.Margin = new(marginLeft, marginTop, marginRight, marginBottom);
        }

        protected override Panel CreateView()
        {
            alignmentPromptsPanel = new Panel()
            {
                Layout = LayoutParameters.Fill(),
                Children = alignments
                    .Select(a => new Frame()
                    {
                        Layout = LayoutParameters.Fill(),
                        HorizontalContentAlignment = a.horizontal,
                        VerticalContentAlignment = a.vertical,
                    })
                    .Cast<IView>()
                    .ToList(),
            };
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
            RegisterAllBindings();
            return new Panel()
            {
                Layout = LayoutParameters.FixedSize(GetViewportSize()),
                HorizontalContentAlignment = Alignment.Middle,
                VerticalContentAlignment = Alignment.Middle,
                Children = [contentFrame, alignmentPromptsPanel, movementPromptsFrame],
            };
        }

        private IView? CreateAlignmentPrompt(Frame frame, int numpadIndex)
        {
            if (!Game1.options.gamepadControls)
            {
                return CreateButtonPrompt(SButton.NumPad0 + numpadIndex, numpadIndex.ToString());
            }
            if (frame.VerticalContentAlignment == owner.VerticalContentAlignment)
            {
                bool skipHorizontalMiddle = frame.VerticalContentAlignment == Alignment.Middle;
                var configuredIndex = GetAlignmentIndex(owner.HorizontalContentAlignment, skipHorizontalMiddle);
                var frameIndex = GetAlignmentIndex(frame.HorizontalContentAlignment, skipHorizontalMiddle);
                var comparison = frameIndex - configuredIndex;
                return comparison switch
                {
                    -1 => CreateButtonPrompt(SButton.LeftTrigger, "LT"),
                    1 => CreateButtonPrompt(SButton.RightTrigger, "RT"),
                    _ => null,
                };
            }
            else if (frame.HorizontalContentAlignment == owner.HorizontalContentAlignment)
            {
                bool skipHorizontalMiddle = frame.HorizontalContentAlignment == Alignment.Middle;
                var configuredIndex = GetAlignmentIndex(owner.VerticalContentAlignment, skipHorizontalMiddle);
                var frameIndex = GetAlignmentIndex(frame.VerticalContentAlignment, skipHorizontalMiddle);
                var comparison = frameIndex - configuredIndex;
                return comparison switch
                {
                    -1 => CreateButtonPrompt(SButton.LeftShoulder, "LB"),
                    1 => CreateButtonPrompt(SButton.RightShoulder, "RB"),
                    _ => null,
                };
            }
            return null;
        }

        private void CreateAlignmentPrompts()
        {
            int index = 0;
            foreach (var frame in alignmentPromptsPanel.Children.OfType<Frame>())
            {
                frame.Content = CreateAlignmentPrompt(frame, ++index);
                UpdateAlignmentPromptVisibility(frame);
            }
        }

        private IView CreateButtonPrompt(SButton button, string text, Edges? margin = null)
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

        // Since we don't really intend to support both horizontal and vertical centering, we don't want to simply use
        // the enum ordinal as the "value". Instead we skip the middle when the other axis is already middle.
        private static int GetAlignmentIndex(Alignment alignment, bool skipMiddle)
        {
            return alignment switch
            {
                Alignment.Start => 0,
                Alignment.Middle => skipMiddle ? -1000 : 1,
                Alignment.End => skipMiddle ? 1 : 2,
                _ => throw new ArgumentException($"Unrecognized alignment {alignment}", nameof(alignment)),
            };
        }

        private static Point GetViewportSize()
        {
            var maxViewport = Game1.graphics.GraphicsDevice.Viewport;
            return Game1.uiViewport.Width <= maxViewport.Width
                ? new(Game1.uiViewport.Width, Game1.uiViewport.Height)
                : new(maxViewport.Width, maxViewport.Height);
        }

        private void HandleInput()
        {
            bool anyPressed = false;
            var canRepeat =
                (!timeSinceFirstInput.HasValue || timeSinceFirstInput >= ButtonRepeatDelay)
                && (!timeSinceLastInput.HasValue || timeSinceLastInput >= ButtonRepeatInterval);
            foreach (var (button, action) in keybindings)
            {
                if (!UI.InputHelper.IsDown(button) || UI.InputHelper.IsSuppressed(button))
                {
                    continue;
                }
                anyPressed = true;
                if (canRepeat)
                {
                    action();
                }
            }
            if (anyPressed)
            {
                // Initialize these to zero but non-null values so they'll get incremented on Update.
                timeSinceFirstInput ??= TimeSpan.Zero;
                timeSinceLastInput ??= TimeSpan.Zero;
            }
            else
            {
                timeSinceFirstInput = null;
                timeSinceLastInput = null;
            }
        }

        private void Nudge(Direction direction)
        {
            Game1.playSound("drumkit6");
            owner.ContentOffset += direction switch
            {
                Direction.West => new(-1, 0),
                Direction.East => new(1, 0),
                Direction.North => new(0, -1),
                Direction.South => new(0, 1),
                _ => Point.Zero,
            };
        }

        private void RegisterAllBindings()
        {
            RegisterBinding(
                [SButton.W, SButton.Up, SButton.DPadUp, SButton.LeftThumbstickUp],
                () => Nudge(Direction.North),
                suppress: false
            );
            RegisterBinding(
                [SButton.A, SButton.Left, SButton.DPadLeft, SButton.LeftThumbstickLeft],
                () => Nudge(Direction.West),
                suppress: false
            );
            RegisterBinding(
                [SButton.S, SButton.Down, SButton.DPadDown, SButton.LeftThumbstickDown],
                () => Nudge(Direction.South),
                suppress: false
            );
            RegisterBinding(
                [SButton.D, SButton.Right, SButton.DPadRight, SButton.LeftThumbstickRight],
                () => Nudge(Direction.East),
                suppress: false
            );
            RegisterBinding(SButton.LeftTrigger, () => Snap(Direction.West));
            RegisterBinding(SButton.RightTrigger, () => Snap(Direction.East));
            RegisterBinding(SButton.LeftShoulder, () => Snap(Direction.North));
            RegisterBinding(SButton.RightShoulder, () => Snap(Direction.South));
            RegisterBinding([SButton.D1, SButton.NumPad1], () => Snap(Alignment.Start, Alignment.End));
            RegisterBinding([SButton.D2, SButton.NumPad2], () => Snap(Alignment.Middle, Alignment.End));
            RegisterBinding([SButton.D3, SButton.NumPad3], () => Snap(Alignment.End, Alignment.End));
            RegisterBinding([SButton.D4, SButton.NumPad4], () => Snap(Alignment.Start, Alignment.Middle));
            RegisterBinding([SButton.D5, SButton.NumPad5], () => Snap(Alignment.Middle, Alignment.Middle));
            RegisterBinding([SButton.D6, SButton.NumPad6], () => Snap(Alignment.End, Alignment.Middle));
            RegisterBinding([SButton.D7, SButton.NumPad7], () => Snap(Alignment.Start, Alignment.Start));
            RegisterBinding([SButton.D8, SButton.NumPad8], () => Snap(Alignment.Middle, Alignment.Start));
            RegisterBinding([SButton.D9, SButton.NumPad9], () => Snap(Alignment.End, Alignment.Start));
        }

        private void RegisterBinding(SButton button, Action action, bool suppress = true)
        {
            var wrappedAction = suppress
                ? () =>
                {
                    UI.InputHelper.Suppress(button);
                    action();
                }
                : action;
            keybindings.Add(button, wrappedAction);
        }

        private void RegisterBinding(IReadOnlyList<SButton> buttons, Action action, bool suppress = true)
        {
            foreach (var button in buttons)
            {
                RegisterBinding(button, action, suppress);
            }
        }

        private void Snap(Alignment horizontal, Alignment vertical)
        {
            if (horizontal == owner.HorizontalContentAlignment && vertical == owner.VerticalContentAlignment)
            {
                return;
            }
            Game1.playSound("drumkit6");
            owner.HorizontalContentAlignment = horizontal;
            owner.VerticalContentAlignment = vertical;
            owner.ContentOffset = Point.Zero;
            UpdateAlignmentPromptVisibilities();
        }

        private void Snap(Direction direction)
        {
            Alignment? horizontal = owner.HorizontalContentAlignment;
            Alignment? vertical = owner.VerticalContentAlignment;
            switch (direction)
            {
                case Direction.West:
                    horizontal = horizontal switch
                    {
                        Alignment.End => vertical == Alignment.Middle ? Alignment.Start : Alignment.Middle,
                        Alignment.Middle => Alignment.Start,
                        _ => null,
                    };
                    break;
                case Direction.East:
                    horizontal = horizontal switch
                    {
                        Alignment.Start => vertical == Alignment.Middle ? Alignment.End : Alignment.Middle,
                        Alignment.Middle => Alignment.End,
                        _ => null,
                    };
                    break;
                case Direction.North:
                    vertical = vertical switch
                    {
                        Alignment.End => horizontal == Alignment.Middle ? Alignment.Start : Alignment.Middle,
                        Alignment.Middle => Alignment.Start,
                        _ => null,
                    };
                    break;
                case Direction.South:
                    vertical = vertical switch
                    {
                        Alignment.Start => horizontal == Alignment.Middle ? Alignment.End : Alignment.Middle,
                        Alignment.Middle => Alignment.End,
                        _ => null,
                    };
                    break;
                default:
                    throw new ArgumentException($"Invalid direction: {direction}", nameof(direction));
            }
            if (horizontal.HasValue && vertical.HasValue)
            {
                Snap(horizontal.Value, vertical.Value);
                // This path is always invoked with gamepad so we need to recreate the (dynamic) prompts.
                CreateAlignmentPrompts();
            }
        }

        private void UpdateAlignmentPromptVisibilities()
        {
            // We only need to update visibility for keyboard prompts, since there are prompts for all 9 spots.
            // Gamepad prompts are already dynamic and there won't be prompts for spots that aren't reachable with a
            // single press.
            if (alignmentPromptsPanel is null || !Game1.options.gamepadControls)
            {
                return;
            }
            foreach (var frame in alignmentPromptsPanel.Children.OfType<Frame>())
            {
                UpdateAlignmentPromptVisibility(frame);
            }
        }

        private void UpdateAlignmentPromptVisibility(Frame frame)
        {
            bool isCenter =
                frame.HorizontalContentAlignment == Alignment.Middle
                && frame.VerticalContentAlignment == Alignment.Middle;
            bool isCurrentAlignment =
                frame.HorizontalContentAlignment == owner.HorizontalContentAlignment
                && frame.VerticalContentAlignment == owner.VerticalContentAlignment;
            frame.Visibility = (isCurrentAlignment || isCenter) ? Visibility.Hidden : Visibility.Visible;
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
