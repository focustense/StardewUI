namespace StardewUI.Framework.Dom;

using Event = Grammar.Event;

/// <summary>
/// Event wire-up in a StarML element.
/// </summary>
public interface IEvent
{
    /// <summary>
    /// The arguments to the event handler.
    /// </summary>
    IReadOnlyList<IArgument> Arguments { get; }

    /// <summary>
    /// Specifies the redirect to use for the context on which the method named <see cref="HandlerName"/> should exist.
    /// </summary>
    /// <remarks>
    /// Applies to the handler method itself but <b>not</b> any of the <see cref="Arguments"/>, which specify their own
    /// redirects when applicable.
    /// </remarks>
    ContextRedirect? ContextRedirect { get; }

    /// <summary>
    /// The name of the event handler (method) to run on the bound or redirected context.
    /// </summary>
    string HandlerName { get; }

    /// <summary>
    /// The event name, i.e. name of the <c>event</c> field on the target <see cref="IView"/>.
    /// </summary>
    string Name { get; }
}

/// <summary>
///
/// </summary>
/// <param name="Name">The event name, i.e. name of the <c>event</c> field on the target <see cref="IView"/>.</param>
/// <param name="HandlerName">The name of the event handler (method) to run on the bound or redirected context.</param>
/// <param name="Arguments">The arguments to the event handler.</param>
/// <param name="ContextRedirect">The redirect to use for the context on which the method named
/// <paramref name="HandlerName"/> should exist.</param>
public record SEvent(
    string Name,
    string HandlerName,
    IReadOnlyList<SArgument> Arguments,
    ContextRedirect? ContextRedirect = null
) : IEvent
{
    IReadOnlyList<IArgument> IEvent.Arguments => Arguments;

    /// <summary>
    /// Initializes a new <see cref="SArgument"/> from the data of a parsed argument.
    /// </summary>
    /// <param name="e">The parsed event.</param>
    /// <param name="arguments">The event arguments.</param>
    public SEvent(Event e, IReadOnlyList<SArgument> arguments)
        : this(
            e.EventName.ToString(),
            e.HandlerName.ToString(),
            arguments,
            ContextRedirect.FromParts(e.ParentDepth, e.ParentType)
        ) { }
}
