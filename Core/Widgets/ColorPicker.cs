using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewUI.Events;
using StardewUI.Graphics;
using StardewUI.Layout;
using StardewUI.Overlays;

namespace StardewUI.Widgets;

/// <summary>
/// Drop-down style widget that opens a detailed color wheel/slider overlay for choosing a precise color.
/// </summary>
public partial class ColorPicker : ComponentView
{
    /// <summary>
    /// Event raised when the selected color changes.
    /// </summary>
    public event EventHandler<EventArgs>? Change;

    /// <summary>
    /// The current color.
    /// </summary>
    public Color Color
    {
        get => color;
        set
        {
            if (value == color)
            {
                return;
            }
            color = value;
            if (overlayView.IsValueCreated)
            {
                overlayView.Value.Color = value;
            }
            if (!textLocked)
            {
                textInput.Text = FormatColor(Color);
            }
            previewImage.Tint = value;
            OnPropertyChanged(nameof(Color));
            Change?.Invoke(this, EventArgs.Empty);
        }
    }

    /// <summary>
    /// Preset colors to show at the bottom, providing single click convenience for the most common colors.
    /// </summary>
    public IReadOnlyList<Color> Presets
    {
        get => presets;
        set
        {
            if (!value.SequenceEqual(presets))
            {
                presets = value;
                if (overlayView.IsValueCreated)
                {
                    overlayView.Value.Presets = value;
                }
                OnPropertyChanged(nameof(Presets));
            }
        }
    }

    /// <summary>
    /// Sprite to display for the Hue-Saturation color wheel.
    /// </summary>
    public Sprite? WheelSprite
    {
        get => wheelSprite;
        set
        {
            if (value != wheelSprite)
            {
                wheelSprite = value;
                if (overlayView.IsValueCreated)
                {
                    overlayView.Value.WheelSprite = value;
                }
                OnPropertyChanged(nameof(WheelSprite));
            }
        }
    }

    private static readonly Lazy<Texture2D> AlphaAdjustTexture = new(() =>
    {
        var texture = new Texture2D(Game1.graphics.GraphicsDevice, 1, 256);
        texture.SetData(Enumerable.Range(0, 256).Select(i => new Color(255 - i, 255 - i, 255 - i, 255 - i)).ToArray());
        return texture;
    });

    private static readonly IReadOnlyList<Color> DefaultPresets =
    [
        new(0x00, 0x0c, 0x16),
        new(0xa0, 0x27, 0x38),
        new(0x82, 0x32, 0x19),
        new(0x62, 0x40, 0x0c),
        new(0x54, 0x45, 0x0a),
        new(0x1d, 0x5d, 0x24),
        new(0x17, 0x5d, 0x4d),
        new(0x25, 0x4a, 0x87),
        new(0x3e, 0x39, 0xb6),
        new(0x57, 0x33, 0xa6),
        new(0xa2, 0x26, 0x52),
        new(0xec, 0xf9, 0xff),
        new(0xf8, 0xcf, 0xda),
        new(0xf5, 0xd7, 0xb1),
        new(0xf1, 0xdd, 0x81),
        new(0xee, 0xdf, 0x76),
        new(0xb5, 0xe6, 0xc5),
        new(0x9b, 0xe6, 0xe6),
        new(0xaf, 0xe0, 0xf5),
        new(0xc5, 0xd9, 0xfa),
        new(0xda, 0xd4, 0xf8),
        new(0xf6, 0xcb, 0xe6),
    ];

    private static readonly Lazy<Texture2D> LightnessAdjustTexture = new(() =>
    {
        var texture = new Texture2D(Game1.graphics.GraphicsDevice, 1, 256);
        texture.SetData(Enumerable.Range(0, 256).Select(i => new Color(255 - i, 255 - i, 255 - i)).ToArray());
        return texture;
    });

    // Initialized in CreateView
    private Lazy<ColorPickerOverlayView> overlayView = null!;
    private Image previewImage = null!;
    private TextInput textInput = null!;

    // Other data
    private Color color;
    private IReadOnlyList<Color> presets = [];
    private bool textLocked; // See comments in equivalent field in ColorPickerOverlayView
    private Sprite? wheelSprite;

    /// <inheritdoc />
    protected override IView CreateView()
    {
        overlayView = new(
            () =>
                new(this)
                {
                    WheelSprite = WheelSprite,
                    Presets = Presets,
                    Color = Color,
                }
        );
        previewImage = new()
        {
            Layout = LayoutParameters.Fill(),
            Fit = ImageFit.Stretch,
            Sprite = UiSprites.White,
            Tint = Color,
            Focusable = true,
        };
        previewImage.LeftClick += PreviewImage_LeftClick;
        textInput = new()
        {
            Layout = LayoutParameters.Fill(),
            Margin = new(Left: 8),
            MaxLength = 9,
            Text = FormatColor(color),
            // It's handy for users to be able to type directly in the box without bringing up the full overlay.
            // However, it's generally not going to be what a controller user wants, since using the sliders is almost
            // always faster than typing on a virtual keyboard.
            // Worst-case scenario, they can still bring up the virtual keyboard from within the overlay.
            Focusable = false,
        };
        textInput.TextChanged += TextInput_TextChanged;
        return new Lane()
        {
            Padding = new(8),
            VerticalContentAlignment = Alignment.Middle,
            Children =
            [
                new Frame()
                {
                    Layout = LayoutParameters.FixedSize(36, 36),
                    Padding = new(4),
                    Background = UiSprites.MenuSlotTransparent,
                    Content = previewImage,
                },
                textInput,
            ],
        };
    }

    private void PreviewImage_LeftClick(object? sender, ClickEventArgs e)
    {
        ShowOverlay();
    }

    private void TextInput_TextChanged(object? sender, EventArgs e)
    {
        var colorText = textInput.Text;
        if (
            Parsers.TryParseColor(colorText, out var color)
            || (colorText.Length > 0 && !colorText.StartsWith('#') && Parsers.TryParseColor("#" + colorText, out color))
        )
        {
            textLocked = true;
            try
            {
                Color = color;
            }
            finally
            {
                textLocked = false;
            }
        }
    }

    private static string FormatColor(Color color)
    {
        var sb = new StringBuilder("#")
            .Append(color.R.ToString("x2"))
            .Append(color.G.ToString("x2"))
            .Append(color.B.ToString("x2"));
        if (color.A < 255)
        {
            sb.Append(color.A.ToString("x2"));
        }
        return sb.ToString();
    }

    private void ShowOverlay()
    {
        Game1.playSound("bigSelect");
        var overlay = new ColorPickerOverlay(overlayView);
        overlay.Close += Overlay_Close;
        Overlay.Push(overlay);
    }

    private void Overlay_Close(object? sender, EventArgs e)
    {
        if (sender is not ColorPickerOverlay overlay)
        {
            return;
        }
        Color = overlay.Color;
    }

    class ColorPickerOverlay(Lazy<ColorPickerOverlayView> lazyView) : FullScreenOverlay
    {
        public Color Color => lazyView.Value.Color;

        protected override IView CreateView()
        {
            return lazyView.Value;
        }
    }

    class ColorPickerOverlayView(ColorPicker owner) : ComponentView
    {
        public Color Color
        {
            get => color;
            set
            {
                if (value != color)
                {
                    color = value;
                    if (!hsvLocked)
                    {
                        hsv = Hsv.FromRgb(color);
                    }
                    UpdateColor();
                }
            }
        }

        public Hsv Hsv
        {
            get => hsv;
            set
            {
                if (value != hsv)
                {
                    hsv = value;
                    hsvLocked = true;
                    try
                    {
                        Color = hsv.ToRgb(Color.A / 255f);
                    }
                    finally
                    {
                        hsvLocked = false;
                    }
                }
            }
        }

        public IReadOnlyList<Color> Presets
        {
            get => presets;
            set
            {
                if (!value.SequenceEqual(presets))
                {
                    presets = value;
                    UpdatePresets();
                }
            }
        }

        public Sprite? WheelSprite
        {
            get => wheelImage.Sprite;
            set => wheelImage.Sprite = value;
        }

        // Initialized in CreateView
        private IView alphaAdjustFrame = null!;
        private Image alphaAdjustImage = null!;
        private Slider alphaSlider = null!;
        private Slider blueSlider = null!;
        private Slider greenSlider = null!;
        private IView lightnessAdjustFrame = null!;
        private Image lightnessAdjustImage = null!;
        private Grid presetGrid = null!;
        private Image previewImage = null!;
        private Slider redSlider = null!;
        private SelectionCircleView selectionCircle = null!;
        private TextInput textInput = null!;
        private Image wheelImage = null!;

        private Color color = owner.Color;
        private IReadOnlyList<Color> presets = DefaultPresets;

        // Round-trip conversion from RGB > HSV > RGB is lossy and can cause strange effects such as stuck R component
        // when playing with the HSV wheel or slider. To mitigate, we track HSV as a separate field, and update it
        // whenever the RGB color updates, but prevent it from updating when the color is specifically being updated
        // from one of the HSV controls.
        private Hsv hsv = Hsv.FromRgb(owner.Color);
        private bool hsvLocked;

        // Various controls need to sync with each other; this helps prevent conflicting updates.
        private bool isUpdating;

        // Typing the hex color doesn't have lossy conversion, but it is distracting and even unusable if e.g. a 4-digit
        // hex suddenly turns into an 8-digit, making it impossible to delete all the text.
        // So, for a different reason, but in a similar fashion to the HSV color, we lock the text when the color is
        // being updated from the text box.
        private bool textLocked;

        public override void Dispose()
        {
            base.Dispose();
            alphaAdjustImage.Sprite?.Texture.Dispose();
            lightnessAdjustImage.Sprite?.Texture.Dispose();
            GC.SuppressFinalize(this);
        }

        protected override IView CreateView()
        {
            // Top row for color preview and/or direct text entry
            previewImage = new()
            {
                Layout = LayoutParameters.Fill(),
                Fit = ImageFit.Stretch,
                Sprite = UiSprites.White,
            };
            textInput = new()
            {
                Layout = LayoutParameters.Fill(),
                Margin = new(Left: 8),
                MaxLength = 9,
                Text = FormatColor(color),
            };
            textInput.TextChanged += TextInput_TextChanged;
            var resultLane = new Lane()
            {
                Padding = new(8),
                VerticalContentAlignment = Alignment.Middle,
                Children =
                [
                    new Frame()
                    {
                        Layout = LayoutParameters.FixedSize(48, 48),
                        Padding = new(4),
                        Background = UiSprites.MenuSlotTransparent,
                        Content = previewImage,
                    },
                    textInput,
                ],
            };

            // Color wheel and lightness/alpha sliders, for more intuitive selection than RGB sliders, but generally
            // only accessible using a mouse.
            wheelImage = new()
            {
                Layout = LayoutParameters.Fill(),
                ShadowAlpha = 0.3f,
                ShadowOffset = new(-3, 3),
                Draggable = true,
            };
            wheelImage.Drag += WheelImage_Drag;
            selectionCircle = new SelectionCircleView();
            var wheelPanel = new Panel()
            {
                Layout = LayoutParameters.FixedSize(240, 240),
                Margin = new(Right: 8),
                HorizontalContentAlignment = Alignment.Middle,
                VerticalContentAlignment = Alignment.Middle,
                Children = [wheelImage, selectionCircle],
            };
            lightnessAdjustFrame = CreateVerticalAdjustmentFrame(ref lightnessAdjustImage);
            lightnessAdjustImage.Sprite = new(LightnessAdjustTexture.Value);
            lightnessAdjustImage.Drag += LightnessAdjustImage_Drag;
            alphaAdjustFrame = CreateVerticalAdjustmentFrame(ref alphaAdjustImage);
            alphaAdjustImage.Sprite = new(AlphaAdjustTexture.Value);
            alphaAdjustImage.Drag += AlphaAdjustImage_Drag;
            var wheelLane = new Lane()
            {
                Layout = LayoutParameters.AutoRow(),
                Margin = new(0, 16),
                Children = [wheelPanel, lightnessAdjustFrame, alphaAdjustFrame],
            };

            var redSliderLane = CreateSliderRow(ref redSlider, "R", (c, r) => new(r, c.G, c.B, c.A));
            var greenSliderLane = CreateSliderRow(ref greenSlider, "G", (c, g) => new(c.R, g, c.B, c.A));
            var blueSliderLane = CreateSliderRow(ref blueSlider, "B", (c, b) => new(c.R, c.G, b, c.A));
            var alphaSliderLane = CreateSliderRow(ref alphaSlider, "A", (c, a) => new(c.R, c.G, c.B, a));

            UpdateColor();

            // Final row is the clickable presets.
            presetGrid = new()
            {
                Layout = LayoutParameters.AutoRow(),
                Margin = new(Top: 16, Right: 24),
                ItemLayout = new GridItemLayout.Length(32, Expandable: true),
                ItemSpacing = new(4, 4),
                GridAlignment = Alignment.Middle,
                HorizontalItemAlignment = Alignment.Middle,
                VerticalItemAlignment = Alignment.Middle,
            };
            UpdatePresets();

            return new Frame()
            {
                Layout = new() { Width = Length.Px(450), Height = Length.Content() },
                Padding = new(32, 24),
                Background = UiSprites.ControlBorder,
                Content = new Lane()
                {
                    Layout = LayoutParameters.AutoRow(),
                    Orientation = Orientation.Vertical,
                    Children =
                    [
                        resultLane,
                        wheelLane,
                        redSliderLane,
                        greenSliderLane,
                        blueSliderLane,
                        alphaSliderLane,
                        presetGrid,
                    ],
                },
            };
        }

        private void AlphaAdjustImage_Drag(object? sender, PointerEventArgs e)
        {
            byte alpha = (byte)
                MathF.Round(MathHelper.Clamp(1 - e.Position.Y / alphaAdjustImage.OuterSize.Y, 0, 1) * 255);
            Color = new(Color.R, Color.G, Color.B, alpha);
        }

        private IView CreatePresetSelector(Color color)
        {
            var image = new Image()
            {
                Layout = LayoutParameters.Fill(),
                Fit = ImageFit.Stretch,
                Sprite = UiSprites.White,
                Tint = color,
                Tags = Tags.Create(color),
                Focusable = true,
            };
            image.LeftClick += PresetImage_LeftClick;
            var value = Math.Max(color.R, Math.Max(color.G, color.B));
            Color borderTint = value < 185 ? new(204, 204, 204) : new(170, 170, 170);
            return new Frame()
            {
                Layout = new() { Width = Length.Stretch(), Height = Length.Px(32) },
                Padding = new(2),
                Background = UiSprites.White,
                BackgroundTint = borderTint,
                Content = image,
            };
        }

        private Lane CreateSliderRow(ref Slider slider, string labelText, Func<Color, byte, Color> getNewColor)
        {
            var label = new Label()
            {
                Layout = new() { Width = Length.Px(24), Height = Length.Content() },
                Margin = new(Right: 16),
                Text = labelText,
            };
            // Need a local ref to attach the event handler below; C# compiler will not allow the ref parameter to be
            // used in the lambda function.
            var localSlider = new Slider()
            {
                TrackWidth = 340,
                Min = 0,
                Max = 255,
                Interval = 1,
            };
            localSlider.ValueChange += (_, _) => Color = getNewColor(Color, (byte)localSlider.Value);
            slider = localSlider;
            return new()
            {
                Margin = new(0, 2),
                VerticalContentAlignment = Alignment.Middle,
                Children = [label, slider],
            };
        }

        private static IView CreateVerticalAdjustmentFrame(ref Image image)
        {
            image = new Image()
            {
                Layout = LayoutParameters.Fill(),
                Fit = ImageFit.Stretch,
                Draggable = true,
            };
            var leftCaretLane = CreateCaretLane(UiSprites.CaretRight, Alignment.Start, new(Left: -20));
            var rightCaretLane = CreateCaretLane(UiSprites.CaretLeft, Alignment.End, new(Right: -22));
            var panel = new Panel()
            {
                Layout = LayoutParameters.Fill(),
                Children = [leftCaretLane, rightCaretLane, image],
            };
            return new Frame()
            {
                Layout = new() { Width = Length.Px(48), Height = Length.Stretch() },
                Margin = new(24, 0),
                Padding = new(4),
                Background = UiSprites.WhiteBorder,
                BackgroundTint = new(120, 120, 120),
                ShadowAlpha = 0.2f,
                ShadowOffset = new(-2, 2),
                Content = panel,
            };

            static IView CreateCaretLane(Sprite sprite, Alignment horizontalAlignment, Edges margin)
            {
                return new Lane()
                {
                    Layout = LayoutParameters.Fill(),
                    Margin = margin + new Edges(Top: -6, Bottom: 8),
                    Orientation = Orientation.Vertical,
                    HorizontalContentAlignment = horizontalAlignment,
                    Children =
                    [
                        new Spacer(),
                        new Image() { Layout = LayoutParameters.FixedSize(16, 16), Sprite = sprite },
                    ],
                };
            }
        }

        private void LightnessAdjustImage_Drag(object? sender, PointerEventArgs e)
        {
            float newValue = MathHelper.Clamp(1 - e.Position.Y / lightnessAdjustImage.OuterSize.Y, 0, 1);
            Hsv = new Hsv(Hsv.Hue, Hsv.Saturation, newValue);
        }

        private static Color NormalizeColor(Color color)
        {
            float max = Math.Max(color.R, Math.Max(color.G, color.B));
            if (max == 0)
            {
                return Color.White;
            }
            float rf = color.R / max;
            float gf = color.G / max;
            float bf = color.B / max;
            return new(rf, gf, bf);
        }

        private void PresetImage_LeftClick(object? sender, ClickEventArgs e)
        {
            if (sender is not IView view)
            {
                return;
            }
            var taggedColor = view.Tags.Get<Color>();
            if (taggedColor != Color)
            {
                Game1.playSound("drumkit6");
                Color = taggedColor;
            }
        }

        private void TextInput_TextChanged(object? sender, EventArgs e)
        {
            var colorText = textInput.Text;
            if (
                Parsers.TryParseColor(colorText, out var color)
                || (
                    colorText.Length > 0
                    && !colorText.StartsWith('#')
                    && Parsers.TryParseColor("#" + colorText, out color)
                )
            )
            {
                textLocked = true;
                try
                {
                    Color = color;
                }
                finally
                {
                    textLocked = false;
                }
            }
        }

        private static void UpdateCaretPositions(IView view, float position)
        {
            if (view.GetChildren().FirstOrDefault()?.View is not Panel panel)
            {
                return;
            }
            foreach (var caretLane in panel.Children.Take(2).OfType<Lane>())
            {
                if (caretLane.Children.FirstOrDefault() is not { } spacer)
                {
                    continue;
                }
                spacer.Layout = new() { Width = Length.Px(0), Height = Length.Percent(position * 100) };
            }
        }

        private void UpdateColor()
        {
            if (isUpdating)
            {
                return;
            }
            isUpdating = true;
            try
            {
                previewImage.Tint = Color;
                if (!textLocked)
                {
                    textInput.Text = FormatColor(Color);
                }
                UpdateSelectionCirclePosition();
                alphaAdjustImage.Tint = new(Color.R, Color.G, Color.B, (byte)255);
                UpdateCaretPositions(alphaAdjustFrame, 1 - Color.A / 255f);
                lightnessAdjustImage.Tint = NormalizeColor(Color) * (Color.A / 255f);
                UpdateCaretPositions(lightnessAdjustFrame, 1 - Hsv.Value);
                redSlider.Value = Color.R;
                greenSlider.Value = Color.G;
                blueSlider.Value = Color.B;
                alphaSlider.Value = Color.A;
            }
            finally
            {
                isUpdating = false;
            }
        }

        private void UpdatePresets()
        {
            presetGrid.Children = Presets.Select(CreatePresetSelector).ToArray();
        }

        private void UpdateSelectionCirclePosition()
        {
            var r = Hsv.Saturation * wheelImage.OuterSize.X / 2;
            var t = MathHelper.ToRadians((Hsv.Hue - 90) % 360);
            var x = r * MathF.Cos(t);
            var y = r * MathF.Sin(t);
            selectionCircle.Position = new(x, y);
        }

        private void WheelImage_Drag(object? sender, PointerEventArgs e)
        {
            // The wheel is an HSV selector, with hue on the rotational axis and saturation along the radius.
            // This is exactly how we'll translate positions to color values and vice versa; assuming the image is the
            // one we expect, then the red hue will be at 0°, green at 120° and blue at 240°. Essentially we are just
            // taking the polar coordinate and converting to H and S.
            var center = wheelImage.ContentSize / 2;
            var ray = e.Position - center;
            var distance = ray.Length();
            // Ignore points outside the circular area. X and Y should be the same so it doesn't matter which is used.
            if (distance > center.X)
            {
                return;
            }
            var saturation = distance / center.X;
            var angle = MathF.Atan2(ray.Y, ray.X);
            var hue = ((int)MathF.Round(MathHelper.ToDegrees(angle + MathHelper.PiOver2)) + 360) % 360;
            Hsv = new(hue, saturation, Hsv.Value);
        }
    }

    class SelectionCircleView : View
    {
        public Vector2 Position { get; set; }

        private static Texture2D Texture => LazyTexture.Value;

        private static readonly Vector2 DrawingOffset = new(-4.5f, -4.5f);
        private static readonly Lazy<Texture2D> LazyTexture = new(() =>
        {
            var texture = new Texture2D(Game1.graphics.GraphicsDevice, 9, 9);
            var data = new Color[81];
            data[0 * 9 + 2] = Color.White;
            data[0 * 9 + 3] = Color.White;
            data[0 * 9 + 4] = Color.White;
            data[0 * 9 + 5] = Color.White;
            data[0 * 9 + 6] = Color.White;
            data[1 * 9 + 1] = Color.White;
            data[1 * 9 + 7] = Color.White;
            data[2 * 9 + 0] = Color.White;
            data[2 * 9 + 8] = Color.White;
            data[3 * 9 + 0] = Color.White;
            data[3 * 9 + 8] = Color.White;
            data[4 * 9 + 0] = Color.White;
            data[4 * 9 + 8] = Color.White;
            data[5 * 9 + 0] = Color.White;
            data[5 * 9 + 8] = Color.White;
            data[6 * 9 + 0] = Color.White;
            data[6 * 9 + 8] = Color.White;
            data[7 * 9 + 1] = Color.White;
            data[7 * 9 + 7] = Color.White;
            data[8 * 9 + 2] = Color.White;
            data[8 * 9 + 3] = Color.White;
            data[8 * 9 + 4] = Color.White;
            data[8 * 9 + 5] = Color.White;
            data[8 * 9 + 6] = Color.White;
            texture.SetData(data);
            return texture;
        });
        private static readonly Color shadowColor = new(20, 20, 20);

        protected override void OnDrawContent(ISpriteBatch b)
        {
            var pos = Position + DrawingOffset;
            b.Draw(Texture, pos + new Vector2(0, 1), null, shadowColor, scale: 2);
            b.Draw(Texture, pos + new Vector2(1, 0), null, shadowColor, scale: 2);
            b.Draw(Texture, pos + Vector2.One, null, shadowColor, scale: 2);
            b.Draw(Texture, pos, null, Color.Black, scale: 2);
        }

        protected override void OnMeasure(Vector2 availableSize)
        {
            ContentSize = new(Texture.Width, Texture.Height);
        }
    }
}
