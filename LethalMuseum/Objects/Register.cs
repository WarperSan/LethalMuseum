using System.Collections.Generic;
using LethalMuseum.Helpers;
using LethalMuseum.Objects.Models;

namespace LethalMuseum.Objects;

/// <summary>
/// Class that handles the registering of the items
/// </summary>
internal static class Register
{
    private static readonly SortedDictionary<string, ItemEntry> itemsData = [];

    /// <summary>
    /// Registers the given item
    /// </summary>
    public static void RegisterItem(Item item, bool registerAsEnabled = true)
    {
        if (!Identifier.IsItemAllowed(item))
            return;

        var entries = Identifier.GetEntries(item);

        foreach (var entry in entries)
        {
            if (!itemsData.TryAdd(entry.ID, entry))
            {
                Logger.Debug($"An item has already been registered under the value '{entry.ID}'.");
                continue;
            }
            
            if (!disabledItems.Contains(entry.ID))
                SetItemEnable(entry.ID, registerAsEnabled);
        }
    }

    #region Status
    
    private static readonly HashSet<string> disabledItems = [];
    private const char SEPARATION_CHARACTER = '|';

    /// <summary>
    /// Sets the status of the item with the given ID
    /// </summary>
    public static void SetItemEnable(string id, bool isEnabled)
    {
        if (isEnabled)
            disabledItems.Remove(id);
        else
            disabledItems.Add(id);
    }

    /// <summary>
    /// Sets the status of the items that matches the given condition
    /// </summary>
    public static void ApplyFilter(System.Func<ItemEntry, bool> condition, bool isEnabled)
    {
        foreach (var (id, item) in itemsData)
        {
            if (!condition.Invoke(item))
                continue;
            
            SetItemEnable(id, isEnabled);
        }
    }

    /// <summary>
    /// Checks if the item with the given ID is enabled
    /// </summary>
    public static bool IsEnabled(string id) => itemsData.ContainsKey(id) && !disabledItems.Contains(id);

    /// <summary>
    /// Converts the disabled items to a single value
    /// </summary>
    public static string GenerateBlacklist()
    {
        var builder = new System.Text.StringBuilder();

        var count = 0;

        foreach (var disabledItem in disabledItems)
        {
            builder.Append(disabledItem);

            if (count != disabledItems.Count - 1)
                builder.Append(SEPARATION_CHARACTER);

            count++;
        }

        return builder.ToString();
    }

    /// <summary>
    /// Applies the given blacklist as the disabled items
    /// </summary>
    public static void ApplyBlacklist(string blacklist)
    {
        var items = blacklist.Split(SEPARATION_CHARACTER, System.StringSplitOptions.RemoveEmptyEntries);

        foreach (var item in items)
            SetItemEnable(item, false);
    }

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
    public static ItemEntry[] GetAll()
    {
        var items = new ItemEntry[itemsData.Count];

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
    public static ItemEntry[] GetPage(int index, int pageSize)
    {
        var offset = pageSize * index;
        var items = new List<ItemEntry>();

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

        return UnityEngine.Mathf.CeilToInt(pageCount);
    }

    #endregion
}