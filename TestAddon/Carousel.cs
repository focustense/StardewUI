using Microsoft.Xna.Framework;
using PropertyChanged.SourceGenerator;
using StardewUI;
using StardewUI.Graphics;
using StardewUI.Input;
using StardewUI.Layout;

namespace StardewUITestAddon;

internal partial class Carousel : View
{
    public IList<IView> Children
    {
        get => children;
        set
        {
            if (children.SetItems(value))
            {
                OnPropertyChanged(nameof(Children));
                SelectedIndex = value.Count > 0 ? Math.Clamp(SelectedIndex, 0, value.Count - 1) : -1;
            }
        }
    }

    public float Gap
    {
        get => gap.Value;
        set
        {
            if (gap.SetIfChanged(value))
            {
                OnPropertyChanged(nameof(Gap));
            }
        }
    }

    public LayoutParameters SelectionLayout
    {
        get => selectionLayout.Value;
        set
        {
            if (selectionLayout.SetIfChanged(value))
            {
                OnPropertyChanged(nameof(SelectionLayout));
            }
        }
    }

    protected override Vector2 LayoutOffset => new(-selectedOffset, 0);

    [Notify]
    private KeySpline easing = KeySpline.Linear;

    [Notify]
    private int selectedIndex;

    [Notify]
    private float transitionDuration = 500; // milliseconds

    private readonly List<ViewChild> childPositions = [];
    private readonly DirtyTrackingList<IView> children = [];
    private readonly DirtyTracker<float> gap = new(0);
    private readonly DirtyTracker<LayoutParameters> selectionLayout = new(new());

    private float drawingOffset;
    private float selectedOffset;
    private float transitionProgress;
    private float transitionStartOffset;

    public override void OnUpdate(TimeSpan elapsed)
    {
        if (drawingOffset == selectedOffset)
        {
            return;
        }
        transitionProgress += (float)elapsed.TotalMilliseconds;
        if (transitionProgress >= TransitionDuration)
        {
            drawingOffset = selectedOffset;
            transitionProgress = 0;
            return;
        }
        float progressRatio = transitionProgress / TransitionDuration;
        float offsetRatio = Easing.Get(progressRatio);
        drawingOffset = transitionStartOffset + (selectedOffset - transitionStartOffset) * offsetRatio;
    }

    protected override FocusSearchResult? FindFocusableDescendant(Vector2 contentPosition, Direction direction)
    {
        if (Children.Count == 0)
        {
            return null;
        }
        var selectedChild =
            (SelectedIndex >= 0 && SelectedIndex < Children.Count) ? childPositions[SelectedIndex] : childPositions[0];
        return selectedChild.FocusSearch(contentPosition, direction);
    }

    protected override IEnumerable<ViewChild> GetLocalChildren()
    {
        return childPositions;
    }

    protected override bool IsContentDirty()
    {
        return gap.IsDirty || selectionLayout.IsDirty || children.IsDirty || children.Any(c => c.IsDirty());
    }

    protected override void OnDrawContent(ISpriteBatch b)
    {
        using var _clip = b.Clip(new(0, 0, (int)OuterSize.X, (int)OuterSize.Y));
        var clipBounds = new Bounds(Vector2.Zero, OuterSize);
        foreach (var child in childPositions)
        {
            if (!child.GetActualBounds().Offset(new(-drawingOffset, 0)).IntersectsWith(clipBounds))
            {
                continue;
            }
            using var _ = b.SaveTransform();
            b.Translate(child.Position.X - drawingOffset, child.Position.Y);
            child.View.Draw(b);
        }
    }

    protected override void OnMeasure(Vector2 availableSize)
    {
        ContentSize = Layout.GetLimits(availableSize);
        var selectionLimits = SelectionLayout.GetLimits(availableSize);
        childPositions.Clear();
        float x = (ContentSize.X - selectionLimits.X) / 2;
        foreach (var childView in children)
        {
            childView.Measure(selectionLimits);
            float top = (ContentSize.Y - childView.OuterSize.Y) / 2;
            childPositions.Add(new(childView, new(x, top)));
            x += childView.OuterSize.X + Gap;
        }
        selectedOffset = GetStartOffset(SelectedIndex);
    }

    protected override void ResetDirty()
    {
        gap.ResetDirty();
        selectionLayout.ResetDirty();
        children.ResetDirty();
    }

    private void BeginTransition()
    {
        selectedOffset = GetStartOffset(SelectedIndex);
        transitionStartOffset = drawingOffset;
        transitionProgress = 0;
    }

    private float GetStartOffset(int index)
    {
        if (childPositions.Count == 0 || index < 0 || index >= childPositions.Count)
        {
            return 0;
        }
        var child = childPositions[index];
        var localOffset = (ContentSize.X - child.View.OuterSize.X) / 2;
        return child.Position.X - localOffset;
    }

    // This method has a special name recognized by PropertyChanged.SourceGenerator, and will run whenever the
    // SelectedIndex property changes to a new value.
    private void OnSelectedIndexChanged()
    {
        BeginTransition();
    }
}
