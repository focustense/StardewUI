using Microsoft.Xna.Framework;
using StardewUI.Events;
using StardewUI.Graphics;
using StardewUI.Input;
using StardewUI.Layout;
using StardewValley;

namespace StardewUI.Widgets;

/// <summary>
/// A horizontal track with draggable thumb (button) for choosing a numeric value in a range.
/// </summary>
public class Slider : ComponentView
{
    private const int DEFAULT_TRACK_WIDTH = 120;
    private const int TRACK_HEIGHT = 24;

    /// <summary>
    /// Event raised when the <see cref="Value"/> changes.
    /// </summary>
    public event EventHandler<EventArgs>? ValueChange;

    /// <summary>
    /// Background or track sprite, if not using the default.
    /// </summary>
    public Sprite? BackgroundSprite
    {
        get => backgroundImage.Sprite;
        set => backgroundImage.Sprite = value;
    }

    /// <summary>
    /// Sprite for the thumb/button, if not using the default.
    /// </summary>
    public Sprite? ThumbSprite
    {
        get => thumbImage.Sprite;
        set => thumbImage.Sprite = value;
    }

    /// <summary>
    /// The interval of which <see cref="Value"/> should be a multiple. Affects which values will be hit while dragging.
    /// </summary>
    public float Interval
    {
        get => interval;
        set
        {
            if (value != interval)
            {
                interval = value;
                OnPropertyChanged(nameof(Interval));
            }
        }
    }

    /// <summary>
    /// The maximum value allowed for <see cref="Value"/>.
    /// </summary>
    public float Max
    {
        get => max;
        set
        {
            if (value == max)
            {
                return;
            }
            if (Min > value)
            {
                Min = value;
            }
            max = value;
            OnPropertyChanged(nameof(Max));
            if (Value > max)
            {
                Value = max;
            }
        }
    }

    /// <summary>
    /// The minimum value allowed for <see cref="Value"/>.
    /// </summary>
    public float Min
    {
        get => min;
        set
        {
            if (value == min)
            {
                return;
            }
            if (Max < value)
            {
                Max = value;
            }
            min = value;
            OnPropertyChanged(nameof(Min));
            if (Value < min)
            {
                Value = min;
            }
        }
    }

    /// <summary>
    /// Override for the thumb/button size, recommended when using a custom <see cref="ThumbSprite"/>.
    /// </summary>
    public Vector2? ThumbSize
    {
        get => thumbSize;
        set
        {
            if (value != thumbSize)
            {
                return;
            }
            thumbSize = value;
            thumbImage.Layout = GetThumbImageLayout(value);
            OnPropertyChanged(nameof(ThumbSize));
        }
    }

    /// <summary>
    /// Width of the track bar.
    /// </summary>
    public float TrackWidth
    {
        get => sliderPanel.Layout.Width.Value;
        set
        {
            if (sliderPanel.Layout.Width.Type != LengthType.Px || sliderPanel.Layout.Width.Value != value)
            {
                sliderPanel.Layout = LayoutParameters.FixedSize(value, TRACK_HEIGHT);
                OnPropertyChanged(nameof(TrackWidth));
            }
        }
    }

    /// <summary>
    /// The current value.
    /// </summary>
    public float Value
    {
        get => value;
        set
        {
            var clamped = Math.Clamp(value, min, max);
            if (clamped == this.value)
            {
                return;
            }
            this.value = clamped;
            UpdatePosition();
            UpdateValueLabel();
            ValueChange?.Invoke(this, EventArgs.Empty);
            OnPropertyChanged(nameof(Value));
        }
    }

    /// <summary>
    /// Color of the value text to render, if overriding the default text color.
    /// </summary>
    public Color? ValueColor
    {
        get => valueLabel.Color;
        set
        {
            var color = value ?? Game1.textColor;
            if (color != valueLabel.Color)
            {
                valueLabel.Color = color;
                OnPropertyChanged(nameof(ValueColor));
            }
        }
    }

    /// <summary>
    /// Specifies how to format the <see cref="Value"/> in the label text.
    /// </summary>
    public Func<float, string> ValueFormat
    {
        get => valueFormat;
        set
        {
            if (value != valueFormat)
            {
                valueFormat = value;
                UpdateValueLabel();
                OnPropertyChanged(nameof(ValueFormat));
            }
        }
    }

    // Initialized in CreateView
    private Image backgroundImage = null!;
    private Panel sliderPanel = null!;
    private Image thumbImage = null!;
    private Label valueLabel = null!;

    // Refer to the similar implementation in Scrollbar for explanation of the drag cursor offset.
    private float? initialThumbDragCursorOffset;
    private float interval = 0.01f;
    private float max = 1;
    private float min = 0;
    private Vector2? thumbSize;
    private float value = 0;
    private Func<float, string> valueFormat = v => v.ToString();

    /// <inheritdoc />
    public override FocusSearchResult? FocusSearch(Vector2 position, Direction direction)
    {
        if (ContentBounds.ContainsPoint(position) && TryMoveValue(direction))
        {
            // Fake a focus search "into" the slider so that we keep focus on the thumb.
            return base.FocusSearch(new(0, -1), Direction.South);
        }
        return base.FocusSearch(position, direction);
    }

    /// <inheritdoc />
    protected override IView CreateView()
    {
        backgroundImage = new()
        {
            Layout = LayoutParameters.Fill(),
            Sprite = UiSprites.SliderBackground,
            Fit = ImageFit.Stretch,
        };
        thumbImage = new()
        {
            Layout = GetThumbImageLayout(thumbSize),
            Sprite = UiSprites.SliderButton,
            Fit = ImageFit.Stretch,
            Focusable = true,
            Draggable = true,
            ZIndex = 1,
        };
        thumbImage.DragStart += Thumb_DragStart;
        thumbImage.Drag += Thumb_Drag;
        thumbImage.DragEnd += Thumb_DragEnd;
        thumbImage.LeftClick += Thumb_LeftClick;
        sliderPanel = new Panel()
        {
            Layout = LayoutParameters.FixedSize(DEFAULT_TRACK_WIDTH, TRACK_HEIGHT),
            Children = [backgroundImage, thumbImage],
        };
        sliderPanel.LeftClick += Track_LeftClick;
        valueLabel = Label.Simple("");
        valueLabel.Margin = new(Left: 8);
        UpdateValueLabel();
        return new Lane()
        {
            Layout = LayoutParameters.FitContent(),
            VerticalContentAlignment = Alignment.Middle,
            Children = [sliderPanel, valueLabel],
        };
    }

    /// <inheritdoc />
    protected override void OnLayout()
    {
        UpdatePosition();
    }

    private static LayoutParameters GetThumbImageLayout(Vector2? thumbSize)
    {
        return thumbSize.HasValue
            ? LayoutParameters.FixedSize(thumbSize.Value.X, thumbSize.Value.Y)
            : LayoutParameters.FitContent();
    }

    private void SetValueFromProgress(float progress)
    {
        var exactValueAboveMin = progress * (Max - Min);
        var intervalCount = MathF.Round(exactValueAboveMin / Interval);
        var newValue = Min + intervalCount * Interval;
        if (newValue != Value)
        {
            Game1.playSound("stoneStep");
            Value = newValue;
        }
    }

    // Thumb-related methods are all a simplification of the Scrollbar thumb. Refer to that implementation for comments
    // and explanations.

    private void Thumb_Drag(object? sender, PointerEventArgs e)
    {
        if (!initialThumbDragCursorOffset.HasValue)
        {
            return;
        }

        var trackWidth = sliderPanel.InnerSize.X;
        var availableWidth = trackWidth - thumbImage.ContentSize.X;
        if (availableWidth <= 0)
        {
            return;
        }

        var targetDistance = e.Position.X - initialThumbDragCursorOffset.Value;
        var targetThumbStart = Math.Clamp(targetDistance, 0, availableWidth);
        var progress = targetThumbStart / availableWidth;
        SetValueFromProgress(progress);
    }

    private void Thumb_DragEnd(object? sender, PointerEventArgs e)
    {
        initialThumbDragCursorOffset = null;
    }

    private void Thumb_DragStart(object? sender, PointerEventArgs e)
    {
        var cursorOffset = e.Position.X - thumbImage.Margin.Left;
        initialThumbDragCursorOffset = cursorOffset >= 0 ? cursorOffset : null;
    }

    private void Thumb_LeftClick(object? sender, ClickEventArgs e)
    {
        // Prevent clicks on the button from being treated as clicks on the track.
        if (e.Position.X >= thumbImage.Margin.Left)
        {
            e.Handled = true;
        }
    }

    private void Track_LeftClick(object? sender, ClickEventArgs e)
    {
        if (!e.IsPrimaryButton())
        {
            return;
        }
        var trackWidth = sliderPanel.InnerSize.X;
        var thumbWidth = thumbImage.ContentSize.X;
        var progress = Math.Clamp((e.Position.X - thumbWidth / 2) / (trackWidth - thumbWidth), 0, 1);
        SetValueFromProgress(progress);
    }

    private bool TryMoveValue(Direction direction)
    {
        var interval = direction switch
        {
            Direction.East => Interval,
            Direction.West => -Interval,
            _ => 0,
        };
        if (interval == 0)
        {
            return false;
        }
        var nextValue = Math.Clamp(Value + interval, Min, Max);
        if (nextValue != Value)
        {
            Game1.playSound("stoneStep");
            Value = nextValue;
            return true;
        }
        return false;
    }

    private void UpdatePosition()
    {
        if (sliderPanel is null)
        {
            return;
        }
        if (Max == Min)
        {
            thumbImage.Margin = new(0);
            return;
        }
        var trackWidth = sliderPanel.ContentSize.X;
        var availableWidth = trackWidth - thumbImage.ContentSize.X;
        var progress = (Value - Min) / (Max - Min);
        thumbImage.Margin = new(Left: (int)Math.Round(availableWidth * progress));
    }

    private void UpdateValueLabel()
    {
        if (valueLabel is null)
        {
            return;
        }
        valueLabel.Text = ValueFormat(Value);
    }
}
