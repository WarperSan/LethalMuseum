using HarmonyLib;
using UnityEngine;
using Logger = LethalMuseum.Helpers.Logger;

namespace LethalMuseum.Patches;

[HarmonyPatch(typeof(MenuManager))]
internal class MenuManager_Patches
{
    private static GameObject? customUI;
    
    [HarmonyPostfix]
    [HarmonyPatch(nameof(MenuManager.Start))]
    private static void CreateUI(MenuManager __instance)
    {
        // Fetch container
        var container = GameObject.Find(Constants.MENU_CONTAINER_PATH);

        if (container == null)
        {
            Logger.Error($"Could not find the container at '{Constants.MENU_CONTAINER_PATH}'.");
            return;
        }

        // Create ui
        var ui = new GameObject(nameof(LethalMuseum) + "_UI");
        ui.transform.SetParent(container.transform, false);

        var rect = ui.AddComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.anchoredPosition = Vector2.zero;
        rect.offsetMin = rect.offsetMax = Vector2.zero;

        var index = container.transform.Find("LobbyHostSettings")?.GetSiblingIndex() ?? -1;

        if (index != -1)
            ui.transform.SetSiblingIndex(index);

        if (LethalMuseum.MUSEUM_FORM != null)
            customUI = Object.Instantiate(LethalMuseum.MUSEUM_FORM, ui.transform, false);
        else
            Logger.Error("Could not spawn the main form.");
    }

    [HarmonyPatch(nameof(MenuManager.SetLoadingScreen)), HarmonyPrefix]
    private static void ToggleUI(bool isLoading)
    {
        customUI?.SetActive(!isLoading);
    }
}