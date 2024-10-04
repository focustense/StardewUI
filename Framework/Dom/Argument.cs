using StardewUI.Framework.Grammar;

namespace StardewUI.Framework.Dom;

/// <summary>
/// An argument to a method call, e.g. as used in an <see cref="IEvent"/>.
/// </summary>
public interface IArgument
{
    /// <summary>
    /// Specifies the redirect to use for a context binding, if one was specified and if the <see cref="Type"/> is
    /// <see cref="ArgumentExpressionType.ContextBinding"/>.
    /// </summary>
    ContextRedirect? ContextRedirect { get; }

    /// <summary>
    /// The argument value or binding path, not including punctuation such as quotes or prefixes.
    /// </summary>
    string Expression { get; }

    /// <summary>
    /// The type of argument, indicating how it is to be evaluated in any method calls.
    /// </summary>
    ArgumentExpressionType Type { get; }
}

/// <summary>
/// Record implementation of an <see cref="IArgument"/>.
/// </summary>
/// <param name="Type">The type of argument, indicating how it is to be evaluated in any method calls.</param>
/// <param name="Expression">The argument value or binding path, not including punctuation such as quotes or
/// prefixes.</param>
/// <param name="ContextRedirect">Specifies the redirect to use for a context binding, if one was specified and if the
/// <see cref="Type"/> is <see cref="ArgumentExpressionType.ContextBinding"/>.</param>
public record SArgument(ArgumentExpressionType Type, string Expression, ContextRedirect? ContextRedirect = null)
    : IArgument
{
    /// <summary>
    /// Initializes a new <see cref="SArgument"/> from the data of a parsed argument.
    /// </summary>
    /// <param name="argument">The parsed argument.</param>
    public SArgument(Argument argument)
        : this(
            argument.ExpressionType,
            argument.Expression.ToString(),
            ContextRedirect.FromParts(argument.ParentDepth, argument.ParentType)
        ) { }
}
