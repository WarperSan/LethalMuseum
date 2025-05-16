using System.Collections.Generic;
using LethalMuseum.Helpers;

namespace LethalMuseum.Objects;

/// <summary>
/// Class that handles the registering of the items
/// </summary>
internal static class Register
{
    private static readonly Dictionary<string, Item> itemsData = [];

    /// <summary>
    /// Registers the given item
    /// </summary>
    public static void RegisterItem(Item item, bool registerAsEnabled = true)
    {
        var id = Identifier.GetID(item);

        if (!itemsData.TryAdd(id, item))
        {
            Logger.Debug($"An item has already been registered under the value '{id}'.");
            return;
        }

        SetItemEnable(item, registerAsEnabled);
    }

    #region Status
    
    private static readonly HashSet<string> disabledItems = [];

    /// <summary>
    /// Sets the status of the given item
    /// </summary>
    public static void SetItemEnable(Item item, bool isEnabled) => SetItemEnable(Identifier.GetID(item), isEnabled);
    
    /// <summary>
    /// Sets the status of the item with the given ID
    /// </summary>
    private static void SetItemEnable(string id, bool isEnabled)
    {
        if (isEnabled)
            disabledItems.Remove(id);
        else
            disabledItems.Add(id);
    }
    
    /// <summary>
    /// Checks if the specific given item is enabled
    /// </summary>
    public static bool IsEnabled(GrabbableObject item) => IsEnabled(item.itemProperties);
    
    /// <summary>
    /// Checks if the given item is enabled
    /// </summary>
    public static bool IsEnabled(Item item) => IsEnabled(Identifier.GetID(item));

    /// <summary>
    /// Checks if the item with the given ID is enabled
    /// </summary>
    private static bool IsEnabled(string id) => itemsData.ContainsKey(id) && !disabledItems.Contains(id);

    #endregion

    #region Count

    /// <summary>
    /// Fetches the amount of items registered
    /// </summary>
    public static int GetRegisteredCount() => itemsData.Count - disabledItems.Count;

    #endregion

    #region Get
    
    /// <summary>
    /// Fetches all the items registered
    /// </summary>
    /// <returns></returns>
    public static Item[] GetAll()
    {
        var items = new Item[itemsData.Count];

        int index = 0;

        foreach (var (_, item) in itemsData)
        {
            items[index] = item;
            index++;
        }

        return items;
    }
    
    /// <summary>
    /// Fetches the items to display on the given page
    /// </summary>
    public static Item[] GetPage(int index, int pageSize)
    {
        var offset = pageSize * index;
        var items = new List<Item>();

        foreach (var (id, item) in itemsData)
        {
            if (!IsEnabled(id))
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

    /// <summary>
    /// Fetches the number of pages
    /// </summary>
    public static int GetPageCount(int pageSize)
    {
        var totalItemCount = GetRegisteredCount();
        var pageCount = (float)totalItemCount / pageSize;

        return UnityEngine.Mathf.FloorToInt(pageCount);
    }

    #endregion
}