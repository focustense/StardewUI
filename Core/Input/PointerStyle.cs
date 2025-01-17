namespace StardewUI.Input;

/// <summary>
/// The vanilla pointer styles that can be drawn as a mouse cursor.
/// </summary>
/// <remarks>
/// These are the values supported and expected in vanilla menus via
/// <see cref="StardewValley.Menus.IClickableMenu.drawMouse"/>. Unlike <see cref="Graphics.Cursor"/>, the available
/// options are limited to standard 16x16 rectangles on the main Cursors tilesheet.
/// </remarks>
public enum PointerStyle
{
    /// <summary>
    /// The default pointer for the given input device. Automatically switches between <see cref="Arrow"/> and
    /// <see cref="Finger"/> depending on whether gamepad controls are active.
    /// </summary>
    Default = -1,

    /// <summary>
    /// The default arrow cursor, used for mouse input when no special circumstances are present.
    /// </summary>
    Arrow = 0,

    /// <summary>
    /// Wait cursor, used e.g. when loading a save game.
    /// </summary>
    Hourglass,

    /// <summary>
    /// An open hand. Often used to indicate picking up items, opening doors or chests, petting animals, etc. In a UI,
    /// can also be used to indicate panning or dragging.
    /// </summary>
    Hand,

    /// <summary>
    /// Arrow tip with a wrapped gift box. Used to indicate gifting an NPC.
    /// </summary>
    Gift,

    /// <summary>
    /// Arrow tip with a speech bubble. Used to indicate the initiation of dialogue with an NPC.
    /// </summary>
    Dialogue,

    /// <summary>
    /// Small magnifying glass denoting some type of search function.
    /// </summary>
    Search,

    /// <summary>
    /// Standard arrow pointer next to a plus, used for harvesting. The same icon is also used for stamina.
    /// </summary>
    Harvest,

    /// <summary>
    /// Standard arrow pointer next to a heart.
    /// </summary>
    Health,

    /// <summary>
    /// Closed hand with pointed index finger, used for gamepad focus.
    /// </summary>
    Finger = 44,

    /// <summary>
    /// The "A" button on a gamepad controller, without an explicit pointer.
    /// </summary>
    ControllerA,

    /// <summary>
    /// The "X" button on a gamepad controller, without an explicit pointer.
    /// </summary>
    ControllerX,

    /// <summary>
    /// The "B" button on a gamepad controller, without an explicit pointer.
    /// </summary>
    ControllerB,

    /// <summary>
    /// The "Y" button on a gamepad controller, without an explicit pointer.
    /// </summary>
    ControllerY,
}
