using System.Collections.Generic;

namespace LethalMuseum.Dependencies.LethalLib;

internal static class ItemIdentifier
{
    #region ID

    private const string ID_FORMAT = "{0}/{1}";

    private static string ID(this Item item, string mod = VANILLA_ITEM_MOD) => string.Format(ID_FORMAT, mod, item.itemName);

    public static string GetID(Item item) => GetModdedID(item) ?? item.ID();

    #endregion
    
    #region Item

    private const string VANILLA_ITEM_MOD = "Vanilla";

    #endregion

    #region Modded Items

    private static Dictionary<string, Item>? _cachedModdedItems;

    private static void LoadModdedItems()
    {
        // If already loaded, skip
        if (_cachedModdedItems != null)
            return;
        
        _cachedModdedItems = [];

        foreach (var item in global::LethalLib.Modules.Items.scrapItems)
            _cachedModdedItems.TryAdd(item.item.ID(item.modName), item.item);

        foreach (var item in global::LethalLib.Modules.Items.shopItems)
            _cachedModdedItems.TryAdd(item.item.ID(item.modName), item.item);

        foreach (var item in global::LethalLib.Modules.Items.plainItems)
            _cachedModdedItems.TryAdd(item.item.ID(item.modName), item.item);
    }

    private static string? GetModdedID(Item item)
    {
        LoadModdedItems();

        if (_cachedModdedItems == null)
            return null;
        
        foreach (var (key, value) in _cachedModdedItems)
        {
            if (value != item)
                continue;

            return key;
        }

        return null;
    }

    #endregion
}