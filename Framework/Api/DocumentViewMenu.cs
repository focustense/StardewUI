using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewUI.Framework.Binding;
using StardewUI.Framework.Dom;
using StardewUI.Framework.Sources;
using StardewUI.Graphics;
using StardewValley.Menus;

namespace StardewUI.Framework.Api;

/// <summary>
/// <see cref="ViewMenu"/> implementation based on a StarML <see cref="Document"/>.
/// </summary>
/// <param name="viewNodeFactory">Factory for creating and binding <see cref="IViewNode"/>s.</param>
/// <param name="documentSource">Source providing the StarML document describing the view.</param>
/// <param name="data">Data to be bound to the view.</param>
internal class DocumentViewMenu(IViewNodeFactory viewNodeFactory, IValueSource<Document> documentSource, object? data)
    : ViewMenu,
        IMenuController
{
    /// <inheritdoc />
    public event Action? Closing;

    event Action IMenuController.Closed
    {
        add => ControllerClosed += value;
        remove => ControllerClosed -= value;
    }

    /// <inheritdoc />
    public Func<bool>? CanClose { get; set; }

    /// <inheritdoc />
    public Action? CloseAction { get; set; }

    /// <inheritdoc />
    public IClickableMenu Menu => this;

    /// <inheritdoc />
    public Func<Point>? PositionSelector { get; set; }

    private event Action? ControllerClosed;

    /// <inheritdoc />
    public void EnableCloseButton(Texture2D? texture = null, Rectangle? sourceRect = null, float scale = 4)
    {
        if (texture is null)
        {
            CloseButtonSprite = UiSprites.CloseButton;
        }
        else
        {
            CloseButtonSprite = new(texture, sourceRect, SliceSettings: new(Scale: scale));
        }
    }

    /// <inheritdoc />
    public void SetGutters(int left, int top, int right = -1, int bottom = -1)
    {
        Gutter = new(left, top, right == -1 ? left : right, bottom == -1 ? top : bottom);
    }

    /// <inheritdoc />
    protected override void cleanupBeforeExit()
    {
        Closing?.Invoke();
        base.cleanupBeforeExit();
    }

    /// <inheritdoc />
    protected override DocumentView CreateView()
    {
        var context = data is not null ? BindingContext.Create(data) : null;
        var view = new DocumentView(viewNodeFactory, documentSource) { Context = context };
        view.InitialUpdate();
        return view;
    }

    /// <inheritdoc />
    protected override void CustomClose()
    {
        if (CloseAction is not null)
        {
            CloseAction();
        }
        else
        {
            base.CustomClose();
        }
    }

    /// <inheritdoc />
    protected override MenuCloseBehavior GetCloseBehavior()
    {
        if (CanClose is not null && !CanClose.Invoke())
        {
            return MenuCloseBehavior.Disabled;
        }
        if (CloseAction is not null)
        {
            return MenuCloseBehavior.Custom;
        }
        return MenuCloseBehavior.Default;
    }

    /// <inheritdoc />
    protected override Point GetOriginPosition(Point viewportSize, Point gutterOffset)
    {
        return PositionSelector?.Invoke() ?? base.GetOriginPosition(viewportSize, gutterOffset);
    }

    /// <inheritdoc />
    protected override void OnClosed(EventArgs e)
    {
        base.OnClosed(e);
        ControllerClosed?.Invoke();
    }
}
