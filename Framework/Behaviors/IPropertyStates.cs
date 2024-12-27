using System.Diagnostics.CodeAnalysis;

namespace StardewUI.Framework.Behaviors;

/// <summary>
/// Provides methods for tracking and modifying state-based overrides for a view's property.
/// </summary>
/// <remarks>
/// <para>
/// State overrides provide a clean priority scheme and reversion path for semantic states such as "hover" or "pressed".
/// Instead of behaviors modifying an <see cref="IView"/> directly, they can instead push their override to the view's
/// propert states, and as long as that override remains the topmost state, it is authoritative for that specific view
/// and property. If it is later removed, then whichever other state is subsequently on top takes precedence.
/// </para>
/// <para>
/// Using this abstraction avoids the need for individual behaviors to save the previous value, and more importantly,
/// prevents unintended conflicts between multiple behaviors each trying to act on the same property of the same view.
/// </para>
/// </remarks>
/// <typeparam name="T">The property value type.</typeparam>
public interface IPropertyStates<T> : IEnumerable<KeyValuePair<string, T>>
{
    /// <summary>
    /// Pushes a new state to the top of the stack, making it the active override.
    /// </summary>
    /// <remarks>
    /// If a state with the specified <paramref name="stateName"/> already exists on the stack, then this will remove
    /// the previous instance and add the new instance on top.
    /// </remarks>
    /// <param name="stateName">The name of the new state.</param>
    /// <param name="value">The property value to use when while the state is active.</param>
    void Push(string stateName, T value);

    /// <summary>
    /// Replaces the value associated with a specified state.
    /// </summary>
    /// <remarks>
    /// If no state with the specified <paramref name="stateName"/> is on the stack, then this does nothing. It will not
    /// push a new state.
    /// </remarks>
    /// <param name="stateName">The name of the state on the stack.</param>
    /// <param name="value">The new value to associate with the specified <paramref name="stateName"/>.</param>
    bool Replace(string stateName, T value);

    /// <summary>
    /// Replaces any existing value associated with a specified state, or pushes a new state to the top of the stack if
    /// a previous state does not already exist.
    /// </summary>
    /// <param name="stateName">The name of the new state.</param>
    /// <param name="value">The property value to associate with the specified <paramref name="stateName"/>.</param>
    void ReplaceOrPush(string stateName, T value)
    {
        if (!Replace(stateName, value))
        {
            Push(stateName, value);
        }
    }

    /// <summary>
    /// Removes a specified state override, if one exists.
    /// </summary>
    /// <param name="stateName">The name of the state on the stack.</param>
    /// <param name="value">The value associated with the specified <paramref name="stateName"/>, if there was an
    /// existing override, or <c>null</c> if there was no instance of that state.</param>
    /// <returns><c>true</c> if an override for the specified <paramref name="stateName"/> was removed from the stack;
    /// <c>false</c> if no such state was on the stack.</returns>
    bool TryRemove(string stateName, [MaybeNullWhen(false)] out T value);

    /// <summary>
    /// Gets the value with highest priority, i.e. on top of the stack.
    /// </summary>
    /// <param name="value">The value of the active override, or the default for <typeparamref name="T"/> if the
    /// function returned <c>false</c>.</param>
    /// <returns><c>true</c> if there was at least one active override for this property; <c>false</c> if the stack is
    /// currently empty.</returns>
    bool TryPeek([MaybeNullWhen(false)] out T value);
}
