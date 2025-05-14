using System.Collections.Generic;

namespace LethalMuseum.Dependencies.LethalLib;

internal static class ItemIdentifier
{
    #region ID

    /// <summary>
    /// Builds the ID of the given item
    /// </summary>
    private static string BuildID(Item item, string mod) => $"{mod}/{item.itemName}";

    /// <summary>
    /// Fetches the ID of the given item
    /// </summary>
    public static string GetID(Item item) => GetModdedID(item) ?? BuildID(item, "Vanilla");

    #endregion
    
    #region Modded Items

    private static Dictionary<Item, string>? _cachedModdedItems;

    /// <summary>
    /// Fetches the ID of the given item from the cached modded items
    /// </summary>
    private static string? GetModdedID(Item item)
    {
        if (_cachedModdedItems == null)
        {
            _cachedModdedItems = [];

            foreach (var scrapItem in global::LethalLib.Modules.Items.scrapItems)
                _cachedModdedItems.TryAdd(scrapItem.item, BuildID(scrapItem.item, scrapItem.modName));

            foreach (var shopItem in global::LethalLib.Modules.Items.shopItems)
                _cachedModdedItems.TryAdd(shopItem.item, BuildID(shopItem.item, shopItem.modName));

            foreach (var plainItem in global::LethalLib.Modules.Items.plainItems)
                _cachedModdedItems.TryAdd(plainItem.item, BuildID(plainItem.item, plainItem.modName));
        }

        return _cachedModdedItems.GetValueOrDefault(item);
    }

    #endregion
}