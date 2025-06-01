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
    public static ItemEntry[] GetEntries(Item item)
    {
        var baseEntry = new ItemEntry(item);
        var entries = new List<ItemEntry> { baseEntry };

        for (int i = 0; i < item.materialVariants.Length; i++)
            entries.Add(baseEntry.Material(i));
        
        for (int i = 0; i < item.meshVariants.Length; i++)
            entries.Add(baseEntry.Mesh(i));
        
        return entries.ToArray();
    }

    /// <summary>
    /// Gets all the applicable <see cref="ItemEntry"/> for the given <see cref="GrabbableObject"/>
    /// </summary>
    public static ItemEntry[] GetEntries(GrabbableObject item)
    {
        var baseEntry = new ItemEntry(item.itemProperties);
        var entries = new List<ItemEntry> { baseEntry };

        if (item.TryGetComponent(out MeshRenderer meshRenderer))
        {
            var materialIndex = Array.IndexOf(item.itemProperties.materialVariants, meshRenderer.sharedMaterial);
            
            if (materialIndex != -1)
                entries.Add(baseEntry.Material(materialIndex));
        }
        
        if (item.TryGetComponent(out MeshFilter meshFilter))
        {
            var meshIndex = Array.IndexOf(item.itemProperties.meshVariants, meshFilter?.mesh);

            if (meshIndex != -1)
                entries.Add(baseEntry.Mesh(meshIndex));
        }
        
        return entries.ToArray();
    }
}