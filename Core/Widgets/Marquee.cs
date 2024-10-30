using Microsoft.Xna.Framework;
using StardewUI.Animation;
using StardewUI.Graphics;
using StardewUI.Input;
using StardewUI.Layout;

namespace StardewUI.Widgets;

/// <summary>
/// A scrolling marquee supporting any inner content.
/// </summary>
/// <remarks>
/// Works by doubling and shifting the drawing the portion and applying a clipping rectangle, and therefore should be
/// used only for non-interactive content; clicks and focus searches will not be correct inside the content area.
/// </remarks>
public class Marquee : View
{
    /// <summary>
    /// Content to scroll inside the marquee.
    /// </summary>
    public IView? Content
    {
        get => content.Value;
        set
        {
            if (content.SetIfChanged(value))
            {
                OnPropertyChanged(nameof(Content));
            }
        }
    }

    /// <summary>
    /// Distance in pixels between the copy of the content being scrolled "out" of the marquee, and the second copy of
    /// the content being scrolled "in".
    /// </summary>
    /// <remarks>
    /// For example, a marquee scrolling the text "Hello World" might at any given moment look like:
    /// <example>
    /// <code>
    /// +----------------+
    /// | ld   Hello wor |
    /// +----------------+
    /// </code>
    /// and in the above scenario the value refers to the distance between the ending "d" and the starting "H".
    /// </example>
    /// </remarks>
    public float ExtraDistance
    {
        get => extraDistance.Value;
        set
        {
            if (extraDistance.SetIfChanged(value))
            {
                OnPropertyChanged(nameof(ExtraDistance));
            }
        }
    }

    /// <summary>
    /// Scrolling speed, in pixels per second.
    /// </summary>
    public float Speed
    {
        get => speed.Value;
        set
        {
            if (!speed.SetIfChanged(value))
            {
                OnPropertyChanged(nameof(Speed));
            }
        }
    }

    private readonly Animator<Marquee, float> animator;
    private readonly DirtyTracker<IView?> content = new(null);
    private readonly DirtyTracker<float> extraDistance = new(100);
    private readonly DirtyTracker<float> speed = new(1);

    private float progress;

    /// <summary>
    /// Initializes a new instance of <see cref="Marquee"/>.
    /// </summary>
    public Marquee()
    {
        animator = Animator.On(this, x => x.progress, (x, v) => x.progress = v);
        animator.Loop = true;
    }

    /// <inheritdoc />
    protected override FocusSearchResult? FindFocusableDescendant(Vector2 contentPosition, Direction direction)
    {
        return Content?.FocusSearch(contentPosition, direction);
    }

    /// <inheritdoc />
    protected override IEnumerable<ViewChild> GetLocalChildren()
    {
        return Content is not null ? [new(Content, Vector2.Zero)] : [];
    }

    /// <inheritdoc />
    protected override bool IsContentDirty()
    {
        return extraDistance.IsDirty || speed.IsDirty || content.IsDirty || (Content?.IsDirty() ?? false);
    }

    /// <inheritdoc />
    protected override void OnDrawContent(ISpriteBatch b)
    {
        if (Content is null)
        {
            return;
        }
        var contentLength = Content.OuterSize.X;
        using var _ = b.Clip(new(0, 0, (int)OuterSize.X, (int)OuterSize.Y));
        b.Translate(-progress, 0);
        Content.Draw(b);
        b.Translate(contentLength + ExtraDistance, 0);
        Content.Draw(b);
    }

    /// <inheritdoc />
    protected override void OnMeasure(Vector2 availableSize)
    {
        var containerLimits = Layout.GetLimits(availableSize);
        var contentLimits = containerLimits;
        contentLimits.X = float.PositiveInfinity;
        Content?.Measure(contentLimits);
        ContentSize = Layout.Resolve(availableSize, () => Content?.OuterSize ?? Vector2.Zero);
        UpdateAnimation();
    }

    /// <inheritdoc />
    protected override void ResetDirty()
    {
        content.ResetDirty();
        extraDistance.ResetDirty();
        speed.ResetDirty();
    }

    private void UpdateAnimation()
    {
        if (animator is null)
        {
            return;
        }
        if (Content is null || Speed == 0)
        {
            animator.Stop();
            return;
        }
        var scrollWidth = Content.OuterSize.X + ExtraDistance;
        var duration = TimeSpan.FromSeconds(scrollWidth / Speed);
        var animation = new Animation<float>(0, scrollWidth, duration);
        // Try to save current progress and restore it after restarting animation.
        var restoredTimeProgress = TimeSpan.FromSeconds(progress / Speed);
        // This will reset the progress to 0.
        animator.Start(animation);
        // Advance back to the adjusted saved position.
        animator.Tick(restoredTimeProgress);
    }
}
