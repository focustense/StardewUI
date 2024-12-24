using Microsoft.Xna.Framework.Graphics;
using static StardewValley.Minigames.TargetGame;

namespace StardewUI.Graphics;

/// <summary>
/// Pools <see cref="RenderTarget2D"/> instances so they can be reused across multiple frames.
/// </summary>
/// <remarks>
/// <para>
/// Targets are pooled by their size, since render targets have a fixed size and changing the size requires recreating
/// the target. The pooled targets are considered to be managed by the pool, and are disposed along with the pool
/// itself; typically a pool is associated with some long-lived UI object such as a menu, and then assigned to a
/// transient instance like <see cref="PropagatedSpriteBatch"/>.
/// </para>
/// <para>
/// Pools can be configured with a <paramref name="slack"/> in order to increase long-term reuse at the expense of
/// higher transient memory and/or VRAM usage due to the extra targets; these may be several megabytes if the areas to
/// be captured are large. Slack can help accommodate dynamic views, e.g. different tabs with different scroll sizes,
/// but should be used conservatively to avoid keeping long-dead targets.
/// </para>
/// <para>
/// The pool is effectively unbounded in the number of instances it can create, but once slack is exceeded, it will
/// dispose an old instance before creating a new one, starting with the instance having the largest pixel size (i.e.
/// taking up the most memory).
/// </para>
/// </remarks>
/// <param name="graphicsDevice">The graphics device used for rendering.</param>
/// <param name="slack">Specifies the maximum number of unused pooled render targets to keep, when requesting a new
/// target whose size is not in the pool, before disposing an older target.</param>
public class RenderTargetPool(GraphicsDevice graphicsDevice, int slack = 0) : IDisposable
{
    private readonly Dictionary<(int, int), Stack<RenderTarget2D>> instances = [];
    private readonly SortedDictionary<int, (int, int)> sizes = new(DescendingIntComparer.Instance);

    private int count;
    private bool isDisposed;

    /// <summary>
    /// Obtains a pooled target with the specified dimensions, or creates a new target if there is no usable pooled
    /// instance.
    /// </summary>
    /// <param name="width">The target's pixel width.</param>
    /// <param name="height">The target's pixel height.</param>
    /// <param name="target">Receives the pooled or created <see cref="RenderTarget2D"/> which has the specified
    /// <paramref name="width"/> and <paramref name="height"/>.</param>
    /// <returns>An <see cref="IDisposable"/> instance which, when disposed, will release the <paramref name="target"/>
    /// back to the pool, without disposing the target itself.</returns>
    public IDisposable Acquire(int width, int height, out RenderTarget2D target)
    {
        if (isDisposed)
        {
            throw new ObjectDisposedException(nameof(RenderTargetPool));
        }
        var key = (width, height);
        if (instances.TryGetValue(key, out var targets) && targets.TryPop(out var acquired))
        {
            count--;
        }
        else
        {
            if (count >= slack)
            {
                Trim(count - slack + 1);
            }
            acquired = CreateTarget(width, height);
        }
        target = acquired;
        return new TargetReleaser(this, acquired);
    }

    /// <inheritdoc />
    public void Dispose()
    {
        if (isDisposed)
        {
            return;
        }
        isDisposed = true;
        foreach (var targets in instances.Values)
        {
            foreach (var target in targets)
            {
                target.Dispose();
            }
        }
        instances.Clear();
        sizes.Clear();
        count = 0;
        GC.SuppressFinalize(this);
    }

    private RenderTarget2D CreateTarget(int width, int height)
    {
        return new(
            graphicsDevice,
            width,
            height,
            false,
            SurfaceFormat.Color,
            DepthFormat.None,
            0,
            RenderTargetUsage.PreserveContents
        );
    }

    private void Release(RenderTarget2D target)
    {
        var key = (target.Width, target.Height);
        if (!instances.TryGetValue(key, out var targets))
        {
            targets = new();
            instances.Add(key, targets);
        }
        targets.Push(target);
        count++;
    }

    private void Trim(int count)
    {
        int remaining = count;
        foreach (var size in sizes.Values)
        {
            if (!instances.TryGetValue(size, out var targets))
            {
                continue;
            }
            while (targets.TryPop(out var target))
            {
                target.Dispose();
                if (--remaining <= 0)
                {
                    return;
                }
            }
        }
    }

    private class DescendingIntComparer : IComparer<int>
    {
        public static readonly DescendingIntComparer Instance = new();

        public int Compare(int x, int y)
        {
            return y.CompareTo(x);
        }
    }

    private class TargetReleaser(RenderTargetPool owner, RenderTarget2D target) : IDisposable
    {
        private bool isDisposed;

        public void Dispose()
        {
            if (isDisposed)
            {
                return;
            }
            isDisposed = true;
            owner.Release(target);
            GC.SuppressFinalize(this);
        }
    }
}
