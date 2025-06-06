﻿using GameNetcodeStuff;
using HarmonyLib;
using LethalMuseum.Objects;
using UnityEngine;
using Logger = LethalMuseum.Helpers.Logger;

namespace LethalMuseum.Patches;

[HarmonyPatch(typeof(PlayerControllerB))]
internal class PlayerControllerB_Patches
{
    [HarmonyPatch(nameof(PlayerControllerB.ConnectClientToPlayerObject)), HarmonyPostfix]
    private static void ConnectClientToPlayerObject_Postfix(PlayerControllerB __instance)
    {
        var canvas = GameObject.Find(Constants.IN_GAME_PARENT_PATH);
        
        if (canvas == null)
        {
            Logger.Error("Could not find the canvas.");
            return;
        }

        if (LethalMuseum.ITEMS_BOARD == null)
        {
            Logger.Error($"The asset for '{nameof(LethalMuseum.ITEMS_BOARD)}' was not loaded.");
            return;
        }

        var parent = new GameObject(nameof(LethalMuseum) + "-InGameUI");
        parent.transform.SetParent(canvas.transform, false);
        
        var index = canvas.transform.Find(Constants.IN_GAME_SIBLING_BEFORE)?.GetSiblingIndex() ?? -1;
        
        if (index >= 0) 
            parent.transform.SetSiblingIndex(index);
        
        var rectTransform = parent.AddComponent<RectTransform>();
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.anchoredPosition = Vector2.zero;
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;

        Object.Instantiate(LethalMuseum.ITEMS_BOARD, parent.transform, false);
    }

    [HarmonyPatch(nameof(PlayerControllerB.SetItemInElevator)), HarmonyPrefix]
    private static void SetItemInElevator_Prefix(bool droppedInShipRoom, bool droppedInElevator, GrabbableObject gObject)
    {
        if (gObject == null)
            return;
        
        if (gObject.isInShipRoom == droppedInShipRoom)
            return;

        if (droppedInShipRoom)
            Tracker.Instance?.Collect(gObject);
        else
            Tracker.Instance?.Discard(gObject);
    }
}