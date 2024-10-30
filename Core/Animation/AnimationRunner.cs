namespace StardewUI.Animation;

/// <summary>
/// Central registry for view animations, meant to be driven from the game loop.
/// </summary>
internal static class AnimationRunner
{
    private static readonly LinkedList<IAnimator> animators = [];

    internal static void Register(IAnimator animator)
    {
        animators.AddLast(animator);
    }

    /// <summary>
    /// Handles a game tick.
    /// </summary>
    /// <param name="elapsed">Time elapsed since last tick.</param>
    public static void Tick(TimeSpan elapsed)
    {
        var node = animators.First;
        while (node is not null)
        {
            var nextNode = node.Next;
            if (node.Value.IsValid())
            {
                node.Value.Tick(elapsed);
            }
            else
            {
                animators.Remove(node);
            }
            node = nextNode;
        }
    }
}
