using System;
using System.Collections.Generic;
using UnityEngine;

namespace LethalMuseum.Objects;

/// <summary>
/// Class that handles the tracking of the items
/// </summary>
public class Tracker : MonoBehaviour
{
    internal static Tracker? Instance;
    
    #region Unity

    private void Awake()
    {
        Instance = this;
        ResetCollected();
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    #endregion

    #region Events

    public event Action<string>? OnCollected;
    public event Action<string>? OnDiscarded;

    /// <summary>
    /// Marks the given item as collected
    /// </summary>
    public void Collect(GrabbableObject? item)
    {
        if (item == null || !Register.IsEnabled(item))
            return;

        var id = Identifier.GetID(item);

        // ReSharper disable once CanSimplifyDictionaryLookupWithTryAdd
        if (itemsCollected.ContainsKey(id))
        {
            itemsCollected[id]++;
            return;
        }

        itemsCollected.Add(id, 1);
        OnCollected?.Invoke(id);
    }

    /// <summary>
    /// Marks the given item as not collected
    /// </summary>
    public void Discard(GrabbableObject? item)
    {
        if (item == null || !Register.IsEnabled(item))
            return;

        var id = Identifier.GetID(item);

        // ReSharper disable once CanSimplifyDictionaryLookupWithTryGetValue
        if (!itemsCollected.ContainsKey(id))
            return;

        var amount = itemsCollected[id] - 1;

        if (amount > 0)
        {
            itemsCollected[id] = amount;
            return;
        }
        
        itemsCollected.Remove(id);
        OnDiscarded?.Invoke(id);
    }

    /// <summary>
    /// Resets the collected items and marks every item in the ship as collected
    /// </summary>
    public void ResetCollected()
    {
        itemsCollected.Clear();
        
        var items = FindObjectsOfType<GrabbableObject>();

        foreach (var item in items)
        {
            if (!item.isInShipRoom)
                continue;

            Collect(item);
        }
    }

    #endregion

    #region Items

    private readonly Dictionary<string, uint> itemsCollected = [];

    /// <summary>
    /// Fetches the amount of items collected
    /// </summary>
    public int GetCollectedCount() => itemsCollected.Count;

    /// <summary>
    /// Checks if the given item is collected
    /// </summary>
    public bool IsCollected(Item item)
    {
        var id = Identifier.GetID(item);
        return itemsCollected.ContainsKey(id);
    }

    #endregion
}