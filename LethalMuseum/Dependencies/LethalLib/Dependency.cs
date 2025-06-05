using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace LethalMuseum.Dependencies.LethalLib;

internal static class Dependency
{
    public static bool Enabled => BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey(global::LethalLib.Plugin.ModGUID);

    private static Dictionary<Item, string>? _cachedModdedItems;

    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
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
    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
    public static bool GetModdedID(Item item, out string? moddedID)
    {
        moddedID = _cachedModdedItems?.GetValueOrDefault(item);

        return string.IsNullOrEmpty(moddedID);
    }

    /// <summary>
    /// Checks if the given item is modded
    /// </summary>
    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
    public static bool IsItemModded(Item item) => _cachedModdedItems?.ContainsKey(item) ?? false;
}