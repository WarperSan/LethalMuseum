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
        if (item is null)
            return;

        var entries = Identifier.GetEntries(item);

        foreach (var entry in entries)
        {
            if (!itemsCollected.TryAdd(entry.ID, 1))
            {
                itemsCollected[entry.ID]++;
                continue;
            }

            OnCollected?.Invoke(entry.ID);
        }
    }

    /// <summary>
    /// Marks the given item as not collected
    /// </summary>
    public void Discard(GrabbableObject? item)
    {
        if (item is null)
            return;

        var entries = Identifier.GetEntries(item);

        foreach (var entry in entries)
        {
            if (!itemsCollected.TryGetValue(entry.ID, out var amount))
                continue;

            amount--;

            if (amount > 0)
            {
                itemsCollected[entry.ID] = amount;
                return;
            }
        
            itemsCollected.Remove(entry.ID);
            OnDiscarded?.Invoke(entry.ID);
        }
    }

    /// <summary>
    /// Resets the collected items and marks every item in the ship as collected
    /// </summary>
    public void ResetCollected()
    {
        var count = itemsCollected.Count;
        
        if (count <= 0)
            return;

        var ids = new List<string>();

        foreach (var (id, _) in itemsCollected)
            ids.Add(id);

        itemsCollected.Clear();
        
        foreach (var id in ids)
            OnDiscarded?.Invoke(id);
        
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
    public bool IsCollected(string id) => itemsCollected.ContainsKey(id);

    #endregion
}