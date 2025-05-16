using System.Collections.Generic;

namespace LethalMuseum.Dependencies.LethalLib;

internal static class ModdedItemIdentifier
{
    private static Dictionary<Item, string>? _cachedModdedItems;

    public static void LoadModdedItems()
    {
        _cachedModdedItems = [];

        foreach (var scrapItem in global::LethalLib.Modules.Items.scrapItems)
            _cachedModdedItems.TryAdd(scrapItem.item, $"{scrapItem.modName}/{scrapItem.item.itemName}");

        foreach (var shopItem in global::LethalLib.Modules.Items.shopItems)
            _cachedModdedItems.TryAdd(shopItem.item, $"{shopItem.modName}/{shopItem.item.itemName}");

        foreach (var plainItem in global::LethalLib.Modules.Items.plainItems)
            _cachedModdedItems.TryAdd(plainItem.item, $"{plainItem.modName}/{plainItem.item.itemName}");
    }
    
    /// <summary>
    /// Fetches the ID of the given item from the cached modded items
    /// </summary>
    public static bool GetModdedID(Item item, out string? moddedID)
    {
        moddedID = _cachedModdedItems?.GetValueOrDefault(item);

        return string.IsNullOrEmpty(moddedID);
    }
}