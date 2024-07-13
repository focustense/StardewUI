namespace SupplyChain.UI;

/// <summary>
/// Controls the scrolling of a <see cref="ScrollContainer"/>.
/// </summary>
/// <remarks>
/// Must be associated with a <see cref="ScrollContainer"/> in order to work; will not draw if the container is not set
/// or if its <see cref="ScrollContainer.ScrollSize"/> is zero.
/// </remarks>
public class Scrollbar(Sprite upSprite, Sprite downSprite, Sprite trackSprite, Sprite thumbSprite)
    : WrapperView<Lane>
{
    /// <summary>
    /// The scroll container that this <see cref="Scrollbar"/> controls.
    /// </summary>
    public ScrollContainer? Container
    {
        get => container;
        set
        {
            container = value;
            LazyUpdate();
        }
    }

    /// <summary>
    /// Margins for this view. See <see cref="View.Margin"/>.
    /// </summary>
    public Edges Margin
    {
        get => margin;
        set
        {
            margin = value;
            LazyUpdate();
        }
    }

    private ScrollContainer? container;
    private Edges margin = new();

    // Initialized in CreateView
    private Image upButton = null!;
    private Image downButton = null!;
    private Frame track = null!;
    private Image thumb = null!;

    /// <summary>
    /// Moves the thumb position to match the scroll offset of the associated container.
    /// </summary>
    public void SyncPosition()
    {
        if (Container is null || thumb is null)
        {
            return;
        }
        var progress = Container.ScrollSize > 0 ? Container.ScrollOffset / Container.ScrollSize : 0;
        var availableLength = Container.Orientation.Get(track.OuterSize) - Container.Orientation.Get(thumb.ContentSize);
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

    protected override Lane CreateView()
    {
        upButton = CreateButton(upSprite, 48, 48);
        downButton = CreateButton(downSprite, 48, 48);
        thumb = new()
        {
            Layout = LayoutParameters.FitContent(),
            HorizontalAlignment = Alignment.Middle,
            VerticalAlignment = Alignment.Middle,
            Sprite = thumbSprite,
        };
        track = new()
        {
            Margin = new(Left: 2, Top: 2, Bottom: 8),
            Background = trackSprite,
            Content = thumb,
        };
        var lane = new Lane()
        {
            Children = [upButton, track, downButton]
        };
        Update(lane);
        return lane;
    }

    private static Image CreateButton(Sprite sprite, int width, int height)
    {
        return new()
        {
            Layout = LayoutParameters.FixedSize(width, height),
            HorizontalAlignment = Alignment.Middle,
            VerticalAlignment = Alignment.Middle,
            Sprite = sprite,
        };
    }

    private void LazyUpdate()
    {
        if (IsViewCreated)
        {
            Update(Root);
        }
    }

    private void Update(Lane root)
    {
        if (Container is null /* || Container.ScrollSize == 0 */)
        {
            root.Visibility = Visibility.Hidden;
            return;
        }
        root.Visibility = Visibility.Visible;
        root.Margin = margin;
        if (Container.Orientation == Orientation.Vertical)
        {
            root.Orientation = Orientation.Vertical;
            root.HorizontalContentAlignment = track.HorizontalContentAlignment = Alignment.Middle;
            root.VerticalContentAlignment = track.VerticalContentAlignment = Alignment.Start;
            track.Layout = new() { Width = Length.Content(), Height = Length.Stretch() };
            upButton.Rotation = null;
            downButton.Rotation = null;
            thumb.Layout = new() { Width = Length.Px(24), Height = Length.Px(40) };
            thumb.Rotation = null;
        }
        else
        {
            root.Orientation = Orientation.Horizontal;
            root.HorizontalContentAlignment = track.HorizontalContentAlignment = Alignment.Start;
            root.VerticalContentAlignment = track.VerticalContentAlignment = Alignment.Middle;
            track.Layout = new() { Width = Length.Stretch(), Height = Length.Content() };
            upButton.Rotation = SimpleRotation.QuarterCounterclockwise; // Left
            downButton.Rotation = SimpleRotation.QuarterCounterclockwise; // Right
            thumb.Layout = new() { Width = Length.Px(40), Height = Length.Px(24) };
            thumb.Rotation = SimpleRotation.QuarterCounterclockwise;
        }
        SyncPosition();
    }
}
