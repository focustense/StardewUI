using System.Text;
using StardewUI.Layout;

namespace StardewUI.Framework.Binding;

/// <summary>
/// A transparent binding node whose purpose is to throttle failed updates and log any errors.
/// </summary>
/// <param name="node">The owned node.</param>
/// <param name="backoffRule">Configures the backoff duration and scaling when an update fails.</param>
public class BackoffNodeDecorator(IViewNode node, BackoffRule backoffRule) : IViewNode
{
    private BackoffState? backoffState;

    /// <inheritdoc />
    public IReadOnlyList<IViewNode.Child> Children => node.Children;

    /// <inheritdoc />
    public BindingContext? Context
    {
        get => node.Context;
        set => node.Context = value;
    }

    /// <inheritdoc />
    public IReadOnlyList<FloatingElement> FloatingElements => node.FloatingElements;

    /// <inheritdoc />
    public IReadOnlyList<IView> Views => node.Views;

    /// <inheritdoc />
    public void Dispose()
    {
        node.Dispose();
        GC.SuppressFinalize(this);
    }

    /// <inheritdoc />
    public void Print(StringBuilder sb, bool includeChildren)
    {
        node.Print(sb, includeChildren);
    }

    /// <inheritdoc />
    public void Reset()
    {
        node.Reset();
    }

    /// <inheritdoc />
    public bool Update(TimeSpan elapsed)
    {
        if (backoffState is not null && backoffState.Elapsed < backoffState.Duration)
        {
            backoffState.Elapsed += elapsed;
            return false;
        }
        try
        {
            var result = node.Update(elapsed);
            backoffState = null;
            return result;
        }
        catch (Exception ex)
        {
            if (backoffState is not null)
            {
                backoffState.Duration = backoffRule.GetNextDuration(backoffState.Duration);
                backoffState.Elapsed = TimeSpan.Zero;
            }
            else
            {
                backoffState = new(backoffRule.InitialDuration);
            }
            var nodeFlatContent = new StringBuilder();
            node.Print(nodeFlatContent, false);
            Logger.Log($"Failed to update node:\n\n  {nodeFlatContent}\n\n{ex}", LogLevel.Error);
            return false;
        }
    }
}
