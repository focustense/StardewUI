using Microsoft.Xna.Framework;
using StardewUI.Framework.Binding;
using StardewUI.Framework.Dom;
using StardewUI.Framework.Sources;
using StardewValley.Menus;

namespace StardewUI.Framework.Api;

/// <summary>
///
/// </summary>
/// <param name="viewNodeFactory">Factory for creating and binding <see cref="IViewNode"/>s.</param>
/// <param name="documentSource">Source providing the StarML document describing the view.</param>
/// <param name="data">Data to be bound to the view.</param>
internal class DocumentViewMenu(IViewNodeFactory viewNodeFactory, IValueSource<Document> documentSource, object? data)
    : ViewMenu<DocumentView>,
        IMenuController
{
    /// <inheritdoc />
    public Func<bool>? CanClose { get; set; }

    /// <inheritdoc />
    public Action<IClickableMenu>? OnClosing { get; set; }

    /// <inheritdoc />
    public IClickableMenu Menu => this;

    /// <inheritdoc />
    public Func<Point>? PositionSelector { get; set; }

    /// <inheritdoc />
    public override bool readyToClose()
    {
        return CanClose?.Invoke() ?? base.readyToClose();
    }

    /// <inheritdoc />
    protected override void cleanupBeforeExit()
    {
        OnClosing?.Invoke(this);
        base.cleanupBeforeExit();
    }

    /// <inheritdoc />
    public void SetGutters(int left, int top, int right = -1, int bottom = -1)
    {
        Gutter = new(left, top, right == -1 ? left : right, bottom == -1 ? top : bottom);
    }

    /// <inheritdoc />
    protected override DocumentView CreateView()
    {
        var context = data is not null ? BindingContext.Create(data) : null;
        return new(viewNodeFactory, documentSource) { Context = context };
    }

    /// <inheritdoc />
    protected override Point GetOriginPosition(Point viewportSize, Point gutterOffset)
    {
        return PositionSelector?.Invoke() ?? base.GetOriginPosition(viewportSize, gutterOffset);
    }
}
