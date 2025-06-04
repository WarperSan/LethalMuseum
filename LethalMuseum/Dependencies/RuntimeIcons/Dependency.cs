using System.Collections.Generic;
using System.Runtime.CompilerServices;
using LethalMuseum.Objects;
using LethalMuseum.Objects.Models;
using LethalMuseum.UI.Elements;
using RuntimeIcons.Utils;
using UnityEngine;

namespace LethalMuseum.Dependencies.RuntimeIcons;

internal static class Dependency
{
    public static bool Enabled => BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey(global::RuntimeIcons.MyPluginInfo.PLUGIN_GUID);
    
    #region Custom Icons

    private static readonly Dictionary<string, Sprite?> generatedIcons = [];
    
    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
    public static bool HasCustomIcon(ItemEntry entry, out Sprite? sprite) => generatedIcons.TryGetValue(entry.ID, out sprite);
    
    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
    public static bool IsLoadingIcon(Sprite sprite)
    {
        if (sprite == global::RuntimeIcons.RuntimeIcons.LoadingSprite)
            return true;
        
        if (sprite == global::RuntimeIcons.RuntimeIcons.LoadingSprite2)
            return true;
        
        return false;
    }

    internal static void OnRender(GrabbableObject item, Texture2D texture)
    {
        var entries = Identifier.GetEntries(item);
        var hasDequeued = new HashSet<Item>();

        foreach (var entry in entries)
        {
            if (entry.IsBase)
                continue;
            
            var sprite = SpriteUtils.CreateSprite(texture);
            sprite.name = sprite.texture.name = $"{nameof(LethalMuseum)}+{nameof(RuntimeIcons)}.{entry.ID}";
            generatedIcons.Add(entry.ID, sprite);

            //if (objectToClear.Remove(entry.ID, out var gameObject) && gameObject != null)
            //    Object.Destroy(gameObject);
            
            if (!hasDequeued.Add(entry.Item))
                continue;
            
            EnqueueNext(entry.Item);
        }

        foreach (var entry in entries)
        {
            if (!hasDequeued.Contains(entry.Item))
                continue;
            
            ItemsBoard.Instance?.UpdateItem(entry.ID);
        }
    }

    #endregion

    #region Icon Queueing

    private static readonly Dictionary<Item, Queue<ItemEntry>> spawnQueue = [];
    private static readonly Dictionary<string, GameObject> objectToClear = [];

    internal static void TryEnqueueAll(ItemEntry[] entries)
    {
        var entriesToSpawn = new Dictionary<Item, ItemEntry>();

        foreach (var entry in entries)
        {
            if (entry.IsBase)
                continue;
            
            if (entry.HasCustomIcon)
                continue;
            
            if (entry.Item.spawnPrefab?.GetComponent<GrabbableObject>() == null)
                continue;
            
            if (entriesToSpawn.TryAdd(entry.Item, entry))
            {
                spawnQueue.Add(entry.Item, []);
                continue;
            }
            
            spawnQueue[entry.Item].Enqueue(entry);
        }

        foreach (var (_, entry) in entriesToSpawn)
            EnqueueEntry(entry);
    }
    
    private static void EnqueueEntry(ItemEntry entry)
    {
        var newItem = Object.Instantiate(entry.Item.spawnPrefab).GetComponent<GrabbableObject>();
        newItem.enabled = false;
        newItem.gameObject.name = $"{nameof(LethalMuseum)}.processing-icon.{entry.ID}";
        newItem.transform.position = new Vector3(0, 1_000, 0);
        
        if (entry.IsVariant)
        {
            if (entry.MaterialIndex != -1 && newItem.TryGetComponent(out MeshRenderer renderer))
                renderer.sharedMaterial = entry.Item.materialVariants[entry.MaterialIndex];

            if (entry.MeshIndex != -1 && newItem.TryGetComponent(out MeshFilter filter))
                filter.mesh = entry.Item.meshVariants[entry.MeshIndex];
        }
            
        objectToClear.Add(entry.ID, newItem.gameObject);

        global::RuntimeIcons.RuntimeIcons.RenderingStage.CameraQueue.EnqueueObject(
            newItem,
            global::RuntimeIcons.RuntimeIcons.WarningSprite,
            2
        );
    }
    
    private static void EnqueueNext(Item item)
    {
        if (!spawnQueue.TryGetValue(item, out var queue))
            return;
        
        if (queue.TryDequeue(out var entry))
            EnqueueEntry(entry);

        if (queue.Count == 0)
            spawnQueue.Remove(item);
    }

    #endregion
}