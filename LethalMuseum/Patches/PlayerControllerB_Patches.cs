﻿using GameNetcodeStuff;
using HarmonyLib;
using UnityEngine;

namespace LethalMuseum.Patches;

[HarmonyPatch(typeof(PlayerControllerB))]
internal class PlayerControllerB_Patches
{
    [HarmonyPostfix]
    [HarmonyPatch(nameof(PlayerControllerB.ConnectClientToPlayerObject))]
    private static void PlayerLoad(PlayerControllerB __instance)
    {
        
    }

    // private static Transform? CreateBoardParent()
    // {
    //     // Find canvas
    //     var canvas = GameObject.Find(Constants.CANVAS_PATH);
    //
    //     if (canvas == null)
    //     {
    //         Logger.Error("Could not find the canvas.");
    //         return null;
    //     }
    //
    //     // Create parent
    //     var bingoParent = new GameObject(nameof(LethalBingo) + "Bingo-InGame-UI");
    //     bingoParent.transform.SetParent(canvas.transform, false);
    //
    //     var index = canvas.transform.Find(Constants.SIBLING_BEFORE)?.GetSiblingIndex() ?? -1;
    //
    //     if (index >= 0)
    //         bingoParent.transform.SetSiblingIndex(index);
    //
    //     // Set rect
    //     var rectTransform = bingoParent.AddComponent<RectTransform>();
    //     rectTransform.anchorMin = Vector2.zero;
    //     rectTransform.anchorMax = Vector2.one;
    //     rectTransform.anchoredPosition = Vector2.zero;
    //     rectTransform.offsetMin = Vector2.zero;
    //     rectTransform.offsetMax = Vector2.zero;
    //
    //     return bingoParent.transform;
    // }
}