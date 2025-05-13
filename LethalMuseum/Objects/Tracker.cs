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
        {
            Logger.Debug($"The item '{item}' is not an item able to be collected.");
            return;
        }
        
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
        {
            Logger.Debug($"The item '{item}' is not an item able to be collected.");
            return;
        }
        
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

    #endregion
}