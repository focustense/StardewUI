namespace StardewUI.Widgets;

/// <summary>
/// Provides a single selection key with change notifications.
/// </summary>
/// <remarks>
/// Can be used to group together UI widgets so that only one at a time can be active, e.g. in a tab or radio group.
/// </remarks>
public class SelectionGroup
{
    /// <summary>
    /// Raised when the <see cref="Key"/> changes.
    /// </summary>
    public event EventHandler? Change;

    /// <summary>
    /// The currently-selected key.
    /// </summary>
    public string? Key
    {
        get => key;
        set
        {
            if (value != key)
            {
                key = value;
                Change?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    private string? key;
}
