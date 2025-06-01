using System;
using System.Collections.Generic;
using LethalMuseum.Dependencies.LethalLib;
using LethalMuseum.Objects.Models;
using UnityEngine;

namespace LethalMuseum.Objects;

/// <summary>
/// Class that handles the identifying of the items
/// </summary>
internal static class Identifier
{
    /// <summary>
    /// Fetches the generic ID of the given item
    /// </summary>
    public static string GetID(Item item)
    {
        if (ModdedItemIdentifier.GetModdedID(item, out var moddedID) && moddedID != null)
            return moddedID;
        
        return $"Vanilla/{item.itemName}";
    }
    
    /// <summary>
    /// Checks if the given item is allowed to be registered
    /// </summary>
    public static bool IsItemAllowed(Item item)
    {
        if (item.lockedInDemo)
            return false;
        
        if (item.spawnPrefab == null)
            return false;

        if (item.itemName == "Maneater")
            return LethalMuseum.Configuration?.AllowBaby.Value ?? false;

        if (item.itemName == "Body")
            return LethalMuseum.Configuration?.AllowBody.Value ?? false;

        return true;
    }

    /// <summary>
    /// Gets all the possible <see cref="ItemEntry"/> for the given <see cref="Item"/>
    /// </summary>
    public static ItemEntry[] GetAllEntries(Item item)
    {
        var entries = new List<ItemEntry>
        {
            new(item)
        };

        for (int i = 0; i < item.materialVariants.Length; i++)
            entries.Add(new ItemEntry(item, i));
        
        for (int i = 0; i < item.meshVariants.Length; i++)
            entries.Add(new ItemEntry(item, -1, i));
        
        return entries.ToArray();
    }

    /// <summary>
    /// Gets all the applicable <see cref="ItemEntry"/> for the given <see cref="GrabbableObject"/>
    /// </summary>
    public static ItemEntry[] GetEntries(GrabbableObject item)
    {
        var entries = new List<ItemEntry>
        {
            new(item.itemProperties)
        };

        var meshRenderer = item.GetComponent<MeshRenderer>();
        var meshFilter = item.GetComponent<MeshFilter>();

        var materialIndex = Array.IndexOf(item.itemProperties.materialVariants, meshRenderer?.sharedMaterial);
        var meshIndex = Array.IndexOf(item.itemProperties.meshVariants, meshFilter?.mesh);

        if (materialIndex != -1)
            entries.Add(new ItemEntry(item.itemProperties, materialIndex));

        if (meshIndex != -1)
            entries.Add(new ItemEntry(item.itemProperties, -1, meshIndex));
        
        return entries.ToArray();
    }
}