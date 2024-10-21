using StardewUI.Animation;
using StardewUI.Events;
using StardewUI.Graphics;
using StardewUI.Layout;
using StardewValley;

namespace StardewUI.Widgets;

/// <summary>
/// Controls the scrolling of a <see cref="ScrollContainer"/>.
/// </summary>
/// <remarks>
/// Must be associated with a <see cref="ScrollContainer"/> in order to work; will not draw if the container is not set
/// or if its <see cref="ScrollContainer.ScrollSize"/> is zero.
/// </remarks>
public class Scrollbar : ComponentView<Lane>
{
    /// <summary>
    /// The scroll container that this <see cref="Scrollbar"/> controls.
    /// </summary>
    public ScrollContainer? Container
    {
        get => container;
        set => SetContainer(value);
    }

    /// <summary>
    /// Sprite to draw for the down arrow, or right arrow in horizontal orientation.
    /// </summary>
    public Sprite? DownSprite
    {
        get => downButton.Sprite;
        set => downButton.Sprite = value;
    }

    /// <summary>
    /// Margins for this view. See <see cref="View.Margin"/>.
    /// </summary>
    public Edges Margin
    {
        get => margin;
        set
        {
            // No OnPropertyChanged here because the margin field is just a lazy initializer for Root.Margin which is
            // already propagated.
            margin = value;
            LazyUpdate();
        }
    }

    /// <summary>
    /// Sprite to draw for the thumb, which moves within the track and indicates the current scroll position and can be
    /// dragged to scroll.
    /// </summary>
    public Sprite? ThumbSprite
    {
        get => thumb.Sprite;
        set => thumb.Sprite = value;
    }

    /// <summary>
    /// Sprite to draw for the track area, within which the thumb can move.
    /// </summary>
    public Sprite? TrackSprite
    {
        get => track.Background;
        set => track.Background = value;
    }

    /// <summary>
    /// Sprite to draw for the up arrow, or left arrow in horizontal orientation.
    /// </summary>
    public Sprite? UpSprite
    {
        get => upButton.Sprite;
        set => upButton.Sprite = value;
    }

    private ScrollContainer? container;
    private Edges margin = new();

    // Initialized in CreateView
    private Image upButton = null!;
    private Image downButton = null!;
    private Frame track = null!;
    private Image thumb = null!;

    // To avoid the common-but-annoying problem where the initial drag motion causes the thumb to suddenly jump to an
    // arbitrary point - typically the result of auto-centering - we track the initial (local) position of the cursor
    // within the thumb, and calculate the intended thumb position based on that.
    //
    // We only need the position along the orientation axis, since dragging in the perpendicular direction should do
    // nothing.
    private float? initialThumbDragCursorOffset;

    /// <summary>
    /// Forces an immediate sync of the thumb position with the associated container.
    /// </summary>
    /// <remarks>
    /// This is typically automatic and should only need to be called in rare situations.
    /// </remarks>
    public void SyncPosition()
    {
        if (Container is null || thumb is null)
        {
            return;
        }
        var progress = Container.ScrollSize > 0 ? Container.ScrollOffset / Container.ScrollSize : 0;
        var availableLength = Container.Orientation.Get(track.InnerSize) - Container.Orientation.Get(thumb.ContentSize);
        var position = availableLength * progress;
        if (Container.Orientation == Orientation.Vertical)
        {
            thumb.Margin = new(Top: (int)position);
        }
        else
        {
            thumb.Margin = new(Left: (int)position);
        }
    }

    /// <inheritdoc />
    protected override Lane CreateView()
    {
        upButton = CreateButton("ScrollBackButton", UiSprites.SmallUpArrow, 48, 48);
        upButton.LeftClick += UpButton_LeftClick;
        downButton = CreateButton("ScrollForwardButton", UiSprites.SmallDownArrow, 48, 48);
        downButton.LeftClick += DownButton_LeftClick;
        thumb = new()
        {
            Name = "ScrollbarThumb",
            Layout = LayoutParameters.FitContent(),
            HorizontalAlignment = Alignment.Middle,
            VerticalAlignment = Alignment.Middle,
            Sprite = UiSprites.VerticalScrollThumb,
            Draggable = true,
        };
        thumb.DragStart += Thumb_DragStart;
        thumb.Drag += Thumb_Drag;
        thumb.DragEnd += Thumb_DragEnd;
        thumb.LeftClick += Thumb_LeftClick;
        track = new()
        {
            Name = "ScrollbarTrack",
            Margin = new(Left: 2, Top: 2, Bottom: 8),
            Background = UiSprites.ScrollBarTrack,
            Content = thumb,
            ShadowAlpha = 0.4f,
            ShadowCount = 2,
            ShadowOffset = new(-5, 5),
        };
        track.LeftClick += Track_LeftClick;
        var lane = new Lane() { Children = [upButton, track, downButton] };
        Update(lane);
        return lane;
    }

    // Events

    private void Container_ScrollChanged(object? sender, EventArgs e)
    {
        SyncPosition();
        SyncVisibility(View);
    }

    private void DownButton_LeftClick(object? sender, ClickEventArgs e)
    {
        if (Container?.ScrollForward() == true)
        {
            Game1.playSound("shwip");
        }
    }

    private void Thumb_Drag(object? sender, PointerEventArgs e)
    {
        if (Container is null || !initialThumbDragCursorOffset.HasValue)
        {
            return;
        }

        var availableLength = Container.Orientation.Get(track.InnerSize) - Container.Orientation.Get(thumb.ContentSize);
        if (availableLength == 0)
        {
            // Shouldn't get here. If we do, there's no way to compute the actual scroll offset based on thumb position.
            return;
        }

        // Because the thumb technically never changes its _position_ (only its margin), the event position is actually
        // also the position in the track, which simplifies the remaining calculations considerably.
        var targetDistance = Container.Orientation.Get(e.Position) - initialThumbDragCursorOffset.Value;
        var targetThumbStart = Math.Clamp(targetDistance, 0, availableLength);
        Container.ScrollOffset = targetThumbStart / availableLength * Container.ScrollSize;
        // Force immediate sync so that we don't get "feedback" from the cursor still being out of sync with the thumb
        // on next frame.
        SyncPosition();
    }

    private void Thumb_DragEnd(object? sender, PointerEventArgs e)
    {
        initialThumbDragCursorOffset = null;
    }

    private void Thumb_DragStart(object? sender, PointerEventArgs e)
    {
        if (Container is null)
        {
            initialThumbDragCursorOffset = null;
            return;
        }
        // The same simplification used in the Drag handler gives us a bit of a wrinkle here; we need to know the cursor
        // offset relative to the actual visible part of the thumb, not the entire view range including margin.
        var orientationPosition = Container.Orientation.Get(e.Position);
        var orientationStart = Container.Orientation == Orientation.Vertical ? thumb.Margin.Top : thumb.Margin.Left;
        var cursorOffset = orientationPosition - orientationStart;
        // Negative offset means the "drag" is not actually on the thumb itself, but in the preceding margin.
        initialThumbDragCursorOffset = cursorOffset >= 0 ? cursorOffset : null;
    }

    private void Thumb_LeftClick(object? sender, ClickEventArgs e)
    {
        // Prevent clicks on the thumb from being treated as clicks on the track.
        if (Container is not null)
        {
            var orientationStart = Container.Orientation == Orientation.Vertical ? thumb.Margin.Top : thumb.Margin.Left;
            if (Container.Orientation.Get(e.Position) >= orientationStart)
            {
                e.Handled = true;
            }
        }
    }

    private void Track_LeftClick(object? sender, ClickEventArgs e)
    {
        if (Container is null)
        {
            return;
        }
        // The simple (and subtly wrong) way to calculate this is to use the exact cursor position within the track as
        // a percentage of the scroll size. However, this won't line up consistently with the new thumb position,
        // because the amount by which the thumb can move is smaller than the track size (by exactly the size of the
        // thumb itself). We have to compensate for the thumb size.
        var cursorDistance = Container.Orientation.Get(e.Position);
        var trackLength = Container.Orientation.Get(track.InnerSize);
        var thumbLength = Container.Orientation.Get(thumb.ContentSize);
        var progress = Math.Clamp((cursorDistance - thumbLength / 2) / (trackLength - thumbLength), 0, 1);
        Container.ScrollOffset = progress * Container.ScrollSize;
    }

    private void UpButton_LeftClick(object? sender, ClickEventArgs e)
    {
        if (Container?.ScrollBackward() == true)
        {
            Game1.playSound("shwip");
        }
    }

    // Other UI

    private static Image CreateButton(string name, Sprite sprite, int width, int height)
    {
        var button = new Image()
        {
            Name = name,
            Layout = LayoutParameters.FixedSize(width, height),
            HorizontalAlignment = Alignment.Middle,
            VerticalAlignment = Alignment.Middle,
            Sprite = sprite,
        };
        HoverScale.Attach(button, 1.1f);
        return button;
    }

    private void LazyUpdate()
    {
        if (View is not null)
        {
            Update(View);
        }
    }

    private void SetContainer(ScrollContainer? container)
    {
        if (container == this.container)
        {
            return;
        }
        if (this.container is not null)
        {
            this.container.ScrollChanged -= Container_ScrollChanged;
        }
        this.container = container;
        if (container is not null)
        {
            container.ScrollChanged += Container_ScrollChanged;
        }
        LazyUpdate();
        OnPropertyChanged(nameof(Container));
    }

    private void SyncVisibility(Lane root)
    {
        root.Visibility = Container?.ScrollSize > 0 ? Visibility.Visible : Visibility.Hidden;
    }

    private void Update(Lane root)
    {
        SyncVisibility(root);
        if (Container is null)
        {
            return;
        }
        root.Margin = margin;
        if (Container.Orientation == Orientation.Vertical)
        {
            root.Orientation = Orientation.Vertical;
            track.Layout = new() { Width = Length.Content(), Height = Length.Stretch() };
            track.Margin = new(Left: 14, Top: 2, Bottom: 8);
            upButton.Rotation = null;
            downButton.Rotation = null;
            thumb.Layout = new() { Width = Length.Px(24), Height = Length.Px(40) };
            thumb.Rotation = null;
        }
        else
        {
            root.Orientation = Orientation.Horizontal;
            track.Layout = new() { Width = Length.Stretch(), Height = Length.Content() };
            track.Margin = new(Left: 2, Top: 14, Bottom: 8);
            upButton.Rotation = SimpleRotation.QuarterCounterclockwise; // Left
            downButton.Rotation = SimpleRotation.QuarterCounterclockwise; // Right
            thumb.Layout = new() { Width = Length.Px(40), Height = Length.Px(24) };
            thumb.Rotation = SimpleRotation.QuarterCounterclockwise;
        }
        SyncPosition();
        SyncVisibility(root);
    }
}
