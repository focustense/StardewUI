namespace StardewUI.Framework.Grammar;

/// <summary>
/// An event handler parsed from StarML.
/// </summary>
/// <param name="eventName">The name of the event to which a handler should be attached.</param>
/// <param name="handlerName">The name of the handler method to invoke.</param>
/// <param name="parentDepth">The depth to walk - i.e. number of parents to traverse - to find the object on which to
/// invoke the handler method.</param>
/// <param name="parentType">The type name of the parent to walk up to for a context redirect. Exclusive with
/// <paramref name="parentDepth"/>.</param>
public readonly ref struct Event(
    ReadOnlySpan<char> eventName,
    ReadOnlySpan<char> handlerName,
    uint parentDepth,
    ReadOnlySpan<char> parentType
)
{
    /// <summary>
    /// The name of the event to which a handler should be attached.
    /// </summary>
    public ReadOnlySpan<char> EventName { get; } = eventName;

    /// <summary>
    /// The name of the handler method to invoke.
    /// </summary>
    public ReadOnlySpan<char> HandlerName { get; } = handlerName;

    /// <summary>
    /// The depth to walk - i.e. number of parents to traverse - to find the object on which to invoke the handler
    /// method.
    /// </summary>
    public uint ParentDepth { get; } = parentDepth;

    /// <summary>
    /// The type name of the parent to walk up to for a context redirect. Exclusive with <see cref="ParentDepth"/>.
    /// </summary>
    public ReadOnlySpan<char> ParentType { get; } = parentType;
}
