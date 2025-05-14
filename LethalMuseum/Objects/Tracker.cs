using System;
using System.Collections.Generic;
using LethalMuseum.Dependencies.LethalLib;
using UnityEngine;
using Logger = LethalMuseum.Helpers.Logger;

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
        if (item == null)
            return;

        var id = ItemIdentifier.GetID(item.itemProperties);
        
        if (!Register.IsAllowed(id))
            return;
        
        // ReSharper disable once CanSimplifySetAddingWithSingleCall
        if (itemsCollected.Contains(id))
            return;

        itemsCollected.Add(id);
        OnCollected?.Invoke(id);
    }

    /// <summary>
    /// Marks the given item as not collected
    /// </summary>
    public void Discard(GrabbableObject? item)
    {
        if (item == null)
            return;

        var id = ItemIdentifier.GetID(item.itemProperties);

        if (!Register.IsAllowed(id))
            return;
        
        if (!itemsCollected.Contains(id))
            return;

        itemsCollected.Remove(id);
        OnDiscarded?.Invoke(id);
    }

    #endregion

    #region Items

    private readonly HashSet<string> itemsCollected = [];

    /// <summary>
    /// Fetches the amount of items collected
    /// </summary>
    public int GetCollectedCount() => itemsCollected.Count;

    /// <summary>
    /// Checks if the given item is collected
    /// </summary>
    public bool IsCollected(Item item)
    {
        var id = ItemIdentifier.GetID(item);
        return itemsCollected.Contains(id);
    }

    #endregion
}