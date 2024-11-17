using Microsoft.Xna.Framework;
using StardewValley;

namespace StardewUI.Data;

/// <summary>
/// Provides data for all known variants of a menu tooltip.
/// </summary>
/// <param name="Text">The primary description text to display. Tooltips converted from a simple <see cref="string"/>
/// will have this field populated.</param>
/// <param name="Title">Bolded title to display above the <see cref="Text"/>, with a separator in between.</param>
/// <param name="Item">The specific game item, if any, that is the "topic" of this tooltip, used to show additional
/// item-specific information such as buffs, durations and recovery values.</param>
/// <param name="CurrencyAmount">Amount of money associated with the tooltip action, generally a buy or sell
/// price.</param>
/// <param name="CurrencySymbol">
/// <para>
/// The currency associated with any <see cref="CurrencyAmount"/>; has no effect unless <see cref="CurrencyAmount"/>
/// is also specified.
/// </para>
/// <para>
/// The meaning of each value is dependent on game implementation, but at the time of writing the available options are:
/// <c>0</c> = coins, <c>1</c> = star tokens (silver star), <c>2</c> = casino tokens, and <c>4</c> = Qi gems.
/// </para>
/// </param>
/// <param name="RequiredItemId">Item ID to show as a required item, usually used as an alternative to
/// <see cref="CurrencySymbol"/> for non-currency trades, such as the Desert Trader.</param>
/// <param name="RequiredItemAmount">The number of items required, e.g. for trade, when <see cref="RequiredItemId"/> is
/// specified.</param>
/// <param name="CraftingRecipe">Crafting recipe to show, if the tooltip is for a craftable item.</param>
/// <param name="AdditionalCraftingMaterials">List of additional items required for crafting that are not included in
/// the <see cref="CraftingRecipe"/>.</param>
public record TooltipData(
    string Text,
    string? Title = null,
    Item? Item = null,
    int? CurrencyAmount = null,
    int CurrencySymbol = 0,
    string? RequiredItemId = null,
    int RequiredItemAmount = 0,
    CraftingRecipe? CraftingRecipe = null,
    IList<Item>? AdditionalCraftingMaterials = null
)
{
    /// <summary>
    /// Validates an item ID for use as the <see cref="RequiredItemId"/>, or else returns <c>null</c> if the ID cannot
    /// be used.
    /// </summary>
    /// <remarks>
    /// Currently, the game only supports unqualified object IDs in this context, so the method will return the
    /// unqualified ID if the <paramref name="itemId"/> is either unqualified or qualified as an object, otherwise
    /// <c>null</c>.
    /// </remarks>
    /// <param name="itemId">The item ID, qualified or unqualified. Unqualified IDs are assumed to be objects.</param>
    internal static string? ValidateItemId(string? itemId)
    {
        if (string.IsNullOrEmpty(itemId))
        {
            return null;
        }
        if (ItemRegistry.IsQualifiedItemId(itemId))
        {
            return itemId.StartsWith("(O)") ? itemId[3..] : null;
        }
        return itemId;
    }

    /// <summary>
    /// Constraints the tooltip to a specified pixel width by breaking lines for the <see cref="Text"/> and
    /// <see cref="Title"/>.
    /// </summary>
    /// <param name="maxWidth">The desired maximum pixel width for the displayed tooltip.</param>
    /// <returns>A <see cref="TooltipData"/> instance with any necessary line breaks added to its text properties in
    /// order to keep the displayed width equal to or less than <paramref name="maxWidth"/>.</returns>
    public TooltipData ConstrainTextWidth(int maxWidth)
    {
        var constrainedText = Game1.parseText(Text, Game1.smallFont, maxWidth);
        var constrainedTitle = !string.IsNullOrEmpty(Title)
            ? Game1.parseText(Title, Game1.dialogueFont, maxWidth)
            : null;
        return new(
            constrainedText,
            constrainedTitle,
            Item,
            CurrencyAmount,
            CurrencySymbol,
            RequiredItemId,
            RequiredItemAmount,
            CraftingRecipe,
            AdditionalCraftingMaterials
        );
    }

    /// <summary>
    /// Converts a simple text string to a <see cref="TooltipData"/> that has the specified <see cref="Text"/>.
    /// </summary>
    /// <param name="text">The tooltip text.</param>
    public static implicit operator TooltipData(string text)
    {
        return new(text);
    }
}
