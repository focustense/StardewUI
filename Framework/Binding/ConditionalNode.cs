using StardewModdingAPI;

namespace StardewUI.Framework.Binding;

/// <summary>
/// A structural node that only passes through its child node when some condition passes.
/// </summary>
/// <param name="innerNode">The node to conditionally render.</param>
/// <param name="condition">The condition to evaluate.</param>
public class ConditionalNode(IViewNode innerNode, ICondition condition) : IViewNode
{
    public IReadOnlyList<IViewNode> ChildNodes => innerNode.ChildNodes;

    public BindingContext? Context
    {
        get => condition.Context;
        set => innerNode.Context = condition.Context = value;
    }

    // In general, child nodes shouldn't have any views when the condition didn't match, because we'll reset them when
    // that happens, but in case of a buggy reset, rechecking here guarantees we won't accidentally show any descendant
    // views if they do still exist.
    public IReadOnlyList<IView> Views => wasMatched ? innerNode.Views : [];

    private bool wasMatched;

    public void Dispose()
    {
        condition.Dispose();
        innerNode.Dispose();
        wasMatched = false;
        GC.SuppressFinalize(this);
    }

    public void Reset()
    {
        innerNode.Reset();
        wasMatched = false;
    }

    public bool Update()
    {
        condition.Update();
        bool conditionChanged = condition.Passed != wasMatched;
        wasMatched = condition.Passed;
        // If the inner node is rendering, we need to update it regardless of whether it was rendering on the previous
        // update, and the final outcome is a change in either the match state OR the inner node.
        if (condition.Passed)
        {
            return innerNode.Update() || conditionChanged;
        }
        // However, we only need to clear the inner node after an actual transition from matched to unmatched, and
        // definitely shouldn't run the update in this case (otherwise it would recreate the child views).
        else if (conditionChanged)
        {
            innerNode.Reset();
        }
        return conditionChanged;
    }
}
