namespace StardewUI.Framework.Descriptors;

/// <summary>
/// Marks some type as being dependent on the descriptor cache, and provides a method to invalidate its contents so that
/// they will be updated after a descriptor change (i.e. due to code hot reload).
/// </summary>
internal interface IDescriptorDependent
{
    /// <summary>
    /// Forces the content to invalidate its descriptors so that they will be reloaded either immediately or on the next
    /// update tick.
    /// </summary>
    /// <remarks>
    /// In most cases this will simply invalidate the entire object graph, e.g. removing a constructed view or context.
    /// </remarks>
    void InvalidateDescriptors();
}
