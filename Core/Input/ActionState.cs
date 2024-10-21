using StardewModdingAPI;
using StardewModdingAPI.Utilities;

namespace StardewUI.Input;

/// <summary>
/// Translates raw input to semantic actions.
/// </summary>
/// <typeparam name="T">The semantic action type.</typeparam>
/// <param name="defaultRepeat">Default repetition settings for registered actions that do not specify one.</param>
/// <param name="defaultSuppress">Whether registered actions should, by default, suppress the normal game behavior for
/// any buttons in their active keybinds. Individual registrations can override this setting.</param>
public class ActionState<T>(ActionRepeat? defaultRepeat = null, bool defaultSuppress = true)
{
    private readonly HashSet<string> readyActions = [];
    private readonly Dictionary<string, RegisteredAction> registeredActions = [];

    /// <summary>
    /// Binds an action to a single button.
    /// </summary>
    /// <param name="button">The bound button.</param>
    /// <param name="action">The action to activate.</param>
    /// <param name="repeat">Repeat behavior for this binding, if not using the default setting.</param>
    /// <param name="suppress">Input suppression behavior for this binding, if not using the default setting.</param>
    /// <returns>The current instance.</returns>
    public ActionState<T> Bind(SButton button, T action, ActionRepeat? repeat = null, bool? suppress = null)
    {
        return Bind(new Keybind(button), action, repeat, suppress);
    }

    /// <summary>
    /// Binds an action to several individual buttons.
    /// </summary>
    /// <remarks>
    /// The <paramref name="buttons"/> are not treated as a single keybind; each individual <see cref="SButton"/> will
    /// independently trigger the <paramref name="action"/>.
    /// </remarks>
    /// <param name="buttons">List of buttons which will each trigger the associated <paramref name="action"/>.</param>
    /// <param name="action">The action to activate.</param>
    /// <param name="repeat">Repeat behavior for this binding, if not using the default setting.</param>
    /// <param name="suppress">Input suppression behavior for this binding, if not using the default setting.</param>
    /// <returns>The current instance.</returns>
    public ActionState<T> Bind(
        IReadOnlyList<SButton> buttons,
        T action,
        ActionRepeat? repeat = null,
        bool suppress = true
    )
    {
        foreach (var button in buttons)
        {
            Bind(button, action, repeat, suppress);
        }
        return this;
    }

    /// <summary>
    /// Binds an action to a button combination.
    /// </summary>
    /// <param name="keybind">Keybind containing buttons that must be simultaneously pressed.</param>
    /// <param name="action">The action to activate.</param>
    /// <param name="repeat">Repeat behavior for this binding, if not using the default setting.</param>
    /// <param name="suppress">Input suppression behavior for this binding, if not using the default setting.</param>
    /// <returns>The current instance.</returns>
    public ActionState<T> Bind(Keybind keybind, T action, ActionRepeat? repeat = null, bool? suppress = null)
    {
        var id = keybind.ToString();
        var registeredAction = new RegisteredAction(
            keybind,
            action,
            repeat ?? defaultRepeat ?? ActionRepeat.Default,
            suppress ?? defaultSuppress
        );
        if (!registeredActions.TryAdd(id, registeredAction))
        {
            throw new InvalidOperationException($"The keybind {id} has already been registered.");
        }
        return this;
    }

    /// <summary>
    /// Binds an action to several button combinations.
    /// </summary>
    /// <param name="keybinds">List of keybinds each of whose button combinations will trigger the associated
    /// <paramref name="action"/>.</param>
    /// <param name="action">The action to activate.</param>
    /// <param name="repeat">Repeat behavior for this binding, if not using the default setting.</param>
    /// <param name="suppress">Input suppression behavior for this binding, if not using the default setting.</param>
    /// <returns>The current instance.</returns>
    public ActionState<T> Bind(
        IReadOnlyList<Keybind> keybinds,
        T action,
        ActionRepeat? repeat = null,
        bool? suppress = null
    )
    {
        foreach (var keybind in keybinds)
        {
            Bind(keybind, action, repeat, suppress);
        }
        return this;
    }

    /// <summary>
    /// Binds an action to all keybinds in a <see cref="KeybindList"/>.
    /// </summary>
    /// <param name="keybindList">List of all keybinds that should trigger the <paramref name="action"/>.</param>
    /// <param name="action">The action to activate.</param>
    /// <param name="repeat">Repeat behavior for this binding, if not using the default setting.</param>
    /// <param name="suppress">Input suppression behavior for this binding, if not using the default setting.</param>
    /// <returns>The current instance.</returns>
    public ActionState<T> Bind(KeybindList keybindList, T action, ActionRepeat? repeat = null, bool? suppress = null)
    {
        foreach (var keybind in keybindList.Keybinds)
        {
            Bind(keybind, action, repeat, suppress);
        }
        return this;
    }

    /// <summary>
    /// Gets all controller bindings associated with a given action.
    /// </summary>
    /// <param name="action">The action to look up.</param>
    /// <returns>A sequence of <see cref="Keybind"/> elements that perform the specified <paramref name="action"/> and
    /// use at least one controller button.</returns>
    public IEnumerable<Keybind> GetControllerBindings(T action)
    {
        return GetBindings(action, keybind => keybind.Buttons.Any(button => button.TryGetController(out _)));
    }

    /// <summary>
    /// Gets the actions that should be run right now, either because one of the triggering buttons/combinations was
    /// just pressed, or because it was held and is due to repeat.
    /// </summary>
    /// <remarks>
    /// Current actions reset on every frame regardless of whether the actions were "handled". Code that reads from
    /// <c>GetCurrentActions</c> should generally do so at the end of an update tick, e.g. in SMAPI's
    /// <see cref="StardewModdingAPI.Events.IGameLoopEvents.UpdateTicked"/> event.
    /// </remarks>
    /// <returns>Sequence of actions that should be handled this frame.</returns>
    public IEnumerable<T> GetCurrentActions()
    {
        foreach (var id in readyActions)
        {
            if (registeredActions.TryGetValue(id, out var registeredAction))
            {
                yield return registeredAction.Action;
            }
        }
    }

    /// <summary>
    /// Gets all keyboard bindings associated with a given action.
    /// </summary>
    /// <param name="action">The action to look up.</param>
    /// <returns>A sequence of <see cref="Keybind"/> elements that perform the specified <paramref name="action"/> and
    /// use at least one keyboard key.</returns>
    public IEnumerable<Keybind> GetKeyboardBindings(T action)
    {
        return GetBindings(action, keybind => keybind.Buttons.Any(button => button.TryGetKeyboard(out _)));
    }

    /// <summary>
    /// Runs on game ticks; updates the state of each binding/action combination.
    /// </summary>
    /// <param name="elapsed">Time elapsed since last game tick.</param>
    internal void Tick(TimeSpan elapsed)
    {
        foreach (var (id, registeredAction) in registeredActions)
        {
            var wasReady = registeredAction.IsReady;
            registeredAction.Tick(elapsed);
            if (registeredAction.IsReady && !wasReady)
            {
                readyActions.Add(id);
            }
            else if (!registeredAction.IsReady && wasReady)
            {
                readyActions.Remove(id);
            }
        }
    }

    private IEnumerable<Keybind> GetBindings(T action, Predicate<Keybind> predicate)
    {
        foreach (var registeredAction in registeredActions.Values)
        {
            if (Equals(registeredAction.Action, action) && predicate(registeredAction.Keybind))
            {
                yield return registeredAction.Keybind;
            }
        }
    }

    private class RegisteredAction(Keybind keybind, T action, ActionRepeat repeat, bool suppress)
    {
        public T Action { get; } = action;

        public bool IsActive { get; private set; }

        public bool IsReady { get; private set; }

        public Keybind Keybind { get; } = keybind;

        private TimeSpan timeSinceFirst;
        private TimeSpan timeSinceLast;

        public void Tick(TimeSpan elapsed)
        {
            if (Keybind.GetState() != SButtonState.Pressed && Keybind.GetState() != SButtonState.Held)
            {
                IsActive = false;
                IsReady = false;
                timeSinceFirst = TimeSpan.Zero;
                timeSinceLast = TimeSpan.Zero;
                return;
            }
            if (IsActive)
            {
                timeSinceFirst += elapsed;
                timeSinceLast += elapsed;
                IsReady = timeSinceFirst >= repeat.InitialDelay && timeSinceLast >= repeat.RepeatInterval;
                if (IsReady)
                {
                    timeSinceLast = TimeSpan.Zero;
                }
            }
            else
            {
                IsActive = true;
                IsReady = true;
                if (suppress)
                {
                    foreach (var button in Keybind.Buttons)
                    {
                        UI.InputHelper.Suppress(button);
                    }
                }
            }
        }
    }
}
