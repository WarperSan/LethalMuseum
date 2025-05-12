using GameNetcodeStuff;
using HarmonyLib;
using UnityEngine;
using Logger = LethalMuseum.Helpers.Logger;

namespace LethalMuseum.Patches;

[HarmonyPatch(typeof(PlayerControllerB))]
internal class PlayerControllerB_Patches
{
    [HarmonyPostfix]
    [HarmonyPatch(nameof(PlayerControllerB.ConnectClientToPlayerObject))]
    private static void PlayerLoad(PlayerControllerB __instance)
    {
        var canvas = GameObject.Find(Constants.CANVAS_PATH);
        
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

        var parent = new GameObject(MyPluginInfo.PLUGIN_GUID + "-InGameUI");
        parent.transform.SetParent(canvas.transform, false);
        
        var index = canvas.transform.Find(Constants.SIBLING_BEFORE)?.GetSiblingIndex() ?? -1;
        
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
}