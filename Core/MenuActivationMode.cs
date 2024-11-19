namespace StardewUI;

/// <summary>
/// Available behaviors for opening a <see cref="ViewMenu{T}"/>.
/// </summary>
public enum MenuActivationMode
{
    /// <summary>
    /// Opens the menu as standalone, replacing any previously-open menu and all of its descendants.
    /// </summary>
    Standalone,

    /// <summary>
    /// Opens the menu as a child of the frontmost game menu that is already active.
    /// </summary>
    /// <remarks>
    /// If no other menu is active, has the same behavior as <see cref="Standalone"/>.
    /// </remarks>
    Child,

    /// <summary>
    /// Replaces the frontmost game menu that is already active, but keeps all of its ancestors.
    /// </summary>
    /// <remarks>
    /// If no other menu is active, or if the active menu is also the only menu (has no parent), then this has the
    /// same behavior as <see cref="Standalone"/>.
    /// </remarks>
    Sibling,
}
