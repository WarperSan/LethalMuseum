using System.Collections.Generic;
using GameNetcodeStuff;
using HarmonyLib;
using LethalMuseum.Objects;
using UnityEngine;

namespace LethalMuseum.Dependencies.RuntimeIcons;

[HarmonyPatch(typeof(PlayerControllerB))]
internal class PlayerControllerB_Patches
{
    internal static readonly Dictionary<string, GameObject> objectToClear = [];

    [HarmonyPatch(nameof(PlayerControllerB.ConnectClientToPlayerObject)), HarmonyPostfix]
    private static void ConnectClientToPlayerObject_Postfix(PlayerControllerB __instance)
    {
        var entries = Register.GetAll();

        foreach (var entry in entries)
        {
            if (entry.HasCustomIcon)
                continue;
            
            if (entry.Item.spawnPrefab?.GetComponent<GrabbableObject>() == null)
                continue;

            var newItem = Object.Instantiate(entry.Item.spawnPrefab).GetComponent<GrabbableObject>();
            newItem.enabled = false;
            objectToClear.Add(entry.ID, newItem.gameObject);
            
            global::RuntimeIcons.RuntimeIcons.RenderingStage.CameraQueue.EnqueueObject(
                newItem,
                global::RuntimeIcons.RuntimeIcons.WarningSprite,
                2
            );
        }
    }
}