using System.Collections.Generic;
using LethalMuseum.Dependencies.LethalLib;
using LethalMuseum.Helpers;

namespace LethalMuseum.Objects;

/// <summary>
/// Class that handles the registering of the items
/// </summary>
internal static class Register
{
    private static readonly Dictionary<string, Item> itemsData = [];
    private static readonly HashSet<string> disabledItems = [];

    /// <summary>
    /// Registers the given item
    /// </summary>
    public static void RegisterItem(Item item, bool registerAsEnabled = true)
    {
        var id = ItemIdentifier.GetID(item);

        if (itemsData.ContainsKey(id))
        {
            Logger.Debug($"An item has already been registered under the value '{id}'.");
            return;
        }
        
        itemsData.Add(id, item);

        if (!registerAsEnabled)
            disabledItems.Add(id);
    }

    /// <summary>
    /// Checks if the item with the given ID is allowed
    /// </summary>
    public static bool IsAllowed(string id) => itemsData.ContainsKey(id) && !disabledItems.Contains(id);

    /// <summary>
    /// Fetches the amount of items registered
    /// </summary>
    public static int GetRegisteredCount() => itemsData.Count;

    /// <summary>
    /// Fetches the items to display on the given page
    /// </summary>
    public static Item[] GetPage(int index, int pageSize)
    {
        var offset = pageSize * index;
        var items = new List<Item>();

        foreach (var (id, item) in itemsData)
        {
            if (disabledItems.Contains(id))
                continue;

            if (offset > 0)
            {
                offset--;
                continue;
            }
            
            items.Add(item);
            
            if (items.Count >= pageSize)
                break;
        }
        
        return items.ToArray();
    }
}