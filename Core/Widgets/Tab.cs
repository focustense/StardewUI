using Microsoft.Xna.Framework;
using StardewUI.Events;
using StardewUI.Graphics;
using StardewUI.Layout;
using StardewValley;

namespace StardewUI.Widgets;

/// <summary>
/// A view with tab appearance, used to navigate sections of a larger complex menu.
/// </summary>
/// <remarks>
/// Tabs activate when clicked; multiple tabs can be assigned to the same <see cref="Group"/>, each with a unique
/// <see cref="GroupKey"/>, in order to deactivate other tabs when any one tab is activated.
/// </remarks>
public class Tab : ComponentView<Panel>
{
    /// <summary>
    /// Event raised when <see cref="Active"/> becomes <c>true</c>.
    /// </summary>
    public event EventHandler? Activate;

    /// <summary>
    /// Event raised when <see cref="Active"/> becomes <c>false</c>.
    /// </summary>
    public event EventHandler? Deactivate;

    /// <summary>
    /// Whether or not the tab is considered active (selected).
    /// </summary>
    /// <remarks>
    /// Active tabs have an offset appearance, normally used to indicate their "pressed" status in relation to other,
    /// neighboring tabs. The offset can be adjusted with <see cref="ActiveOffset"/>.
    /// </remarks>
    public bool Active
    {
        get => isActive;
        set
        {
            if (value != isActive)
            {
                isActive = value;
                if (Group is not null && !string.IsNullOrEmpty(GroupKey))
                {
                    Group.Key = GroupKey;
                }
                if (value)
                {
                    Activate?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    Deactivate?.Invoke(this, EventArgs.Empty);
                }
                OnPropertyChanged(nameof(Active));
            }
        }
    }

    /// <summary>
    /// The drawing offset to apply when the tab is <see cref="Active"/>.
    /// </summary>
    public Vector2 ActiveOffset
    {
        get => activeOffset;
        set
        {
            if (value != activeOffset)
            {
                activeOffset = value;
                OnPropertyChanged(nameof(ActiveOffset));
            }
        }
    }

    /// <summary>
    /// Background image to draw behind the tab's <see cref="Content"/>, which provides the tab appearance.
    /// </summary>
    public Sprite? Background
    {
        get => backgroundImage.Sprite;
        set
        {
            if (value != backgroundImage.Sprite)
            {
                backgroundImage.Sprite = value;
                OnPropertyChanged(nameof(Background));
            }
        }
    }

    /// <summary>
    /// Rotation of the <see cref="Background"/>; does not apply to <see cref="Content"/>.
    /// </summary>
    public SimpleRotation? BackgroundRotation
    {
        get => backgroundImage.Rotation;
        set
        {
            if (backgroundImage.Rotation != value)
            {
                backgroundImage.Rotation = value;
                OnPropertyChanged(nameof(BackgroundRotation));
            }
        }
    }

    /// <summary>
    /// Content to draw inside the tab's border.
    /// </summary>
    public IView? Content
    {
        get => contentFrame.Content;
        set
        {
            if (contentFrame.Content != value)
            {
                contentFrame.Content = value;
                OnPropertyChanged(nameof(Content));
            }
        }
    }

    /// <summary>
    /// Margin to apply to the frame containing the <see cref="Content"/>, i.e. distance between the tab's visual border
    /// and the inner image, text, etc.
    /// </summary>
    /// <remarks>
    /// When using the default <see cref="Background"/>, this is automatically set up to match its border size. If a
    /// different background is used, the margin may need to be adjusted.
    /// </remarks>
    public Edges ContentMargin
    {
        get => contentFrame.Margin;
        set
        {
            if (contentFrame.Margin != value)
            {
                contentFrame.Margin = value;
                OnPropertyChanged(nameof(ContentMargin));
            }
        }
    }

    /// <summary>
    /// The selection group, if any, to which this tab belongs.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Assigning multiple tabs to the same group guarantees that only one can be <see cref="Active"/> at a time. If
    /// this tab becomes active, any previously-active tab will become inactive.
    /// </para>
    /// <para>
    /// To participate in the group, a non-empty <see cref="GroupKey"/> must also be specified.
    /// </para>
    /// </remarks>
    public SelectionGroup? Group
    {
        get => group;
        set
        {
            if (value == group)
            {
                return;
            }
            if (group is not null)
            {
                group.Change -= Group_Change;
            }
            group = value;
            if (value is not null)
            {
                value.Change += Group_Change;
            }
            OnPropertyChanged(nameof(Group));
        }
    }

    /// <summary>
    /// The unique key per <see cref="Group"/> that identifies this tab.
    /// </summary>
    public string GroupKey
    {
        get => groupKey;
        set
        {
            if (value != groupKey)
            {
                groupKey = value;
                UpdateGroupDefault();
                OnPropertyChanged(nameof(GroupKey));
            }
        }
    }

    private readonly Image backgroundImage = new() { Layout = LayoutParameters.Fill(), Sprite = UiSprites.TabTopEmpty };
    private readonly Frame contentFrame =
        new()
        {
            Layout = LayoutParameters.Fill(),
            Margin =
                (UiSprites.TabTopEmpty.FixedEdges ?? Edges.NONE) * (UiSprites.TabTopEmpty.SliceSettings?.Scale ?? 1),
            HorizontalContentAlignment = Alignment.Middle,
            VerticalContentAlignment = Alignment.Middle,
        };

    private Vector2 activeOffset = new(0, 8);
    private bool isActive;
    private SelectionGroup? group;
    private string groupKey = "";

    /// <inheritdoc />
    public override void Draw(ISpriteBatch b)
    {
        if (Active)
        {
            b.Translate(ActiveOffset);
        }
        base.Draw(b);
    }

    /// <inheritdoc />
    public override void Dispose()
    {
        if (group is not null)
        {
            group.Change -= Group_Change;
            group = null;
        }
        base.Dispose();
        GC.SuppressFinalize(this);
    }

    /// <inheritdoc />
    public override void OnClick(ClickEventArgs e)
    {
        if (e.IsPrimaryButton())
        {
            if (!Active)
            {
                Active = true;
                Game1.playSound("smallSelect");
            }
            e.Handled = true;
        }
        base.OnClick(e);
    }

    /// <inheritdoc />
    protected override Panel CreateView()
    {
        return new() { Focusable = true, Children = [backgroundImage, contentFrame] };
    }

    private void Group_Change(object? sender, EventArgs e)
    {
        if (sender != Group)
        {
            return;
        }
        if (Group is not null && !string.IsNullOrEmpty(GroupKey))
        {
            Active = Group.Key == GroupKey;
        }
    }

    private void UpdateGroupDefault()
    {
        if (Group is not null && string.IsNullOrEmpty(Group.Key) && !string.IsNullOrEmpty(GroupKey))
        {
            Group.Key = GroupKey;
        }
    }
}
