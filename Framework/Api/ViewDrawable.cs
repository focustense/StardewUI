using Microsoft.Xna.Framework.Graphics;
using StardewUI.Framework.Binding;
using StardewUI.Graphics;

namespace StardewUI.Framework.Api;

/// <summary>
/// An <see cref="IViewDrawable"/> based on a <see cref="DocumentView"/>.
/// </summary>
/// <param name="view">The view to draw.</param>
internal class ViewDrawable(DocumentView view) : IViewDrawable, IUpdatable
{
    /// <inheritdoc />
    public Vector2 ActualSize => view.OuterSize;

    /// <inheritdoc />
    public object? Context
    {
        get => view.Context?.Data;
        set
        {
            if (value != view.Context?.Data)
            {
                view.Context = value is not null ? BindingContext.Create(value) : null;
            }
        }
    }

    /// <inheritdoc />
    public bool IsDisposed { get; private set; }

    /// <inheritdoc />
    public Vector2? MaxSize { get; set; }

    private readonly RenderTargetPool renderTargetPool = new(Game1.graphics.GraphicsDevice);

    /// <inheritdoc />
    public void Dispose()
    {
        if (IsDisposed)
        {
            return;
        }
        view.Dispose();
        renderTargetPool.Dispose();
        IsDisposed = true;
        GC.SuppressFinalize(this);
    }

    /// <inheritdoc />
    public void Draw(SpriteBatch b, Vector2 position)
    {
        using ISpriteBatch drawableBatch = new PropagatedSpriteBatch(b, GlobalTransform.Default, renderTargetPool);
        drawableBatch.Translate(position);
        view.Draw(drawableBatch);
    }

    /// <inheritdoc />
    public void Update(TimeSpan elapsed)
    {
        view.OnUpdate(elapsed);
        var availableSize = MaxSize ?? UiViewport.GetMaxSize().ToVector2();
        view.Measure(availableSize);
    }
}
