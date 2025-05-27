using HarmonyLib;
using UnityEngine;
using Logger = LethalMuseum.Helpers.Logger;

namespace LethalMuseum.Patches;

[HarmonyPatch(typeof(MenuManager))]
internal class MenuManager_Patches
{
    private static GameObject? customUI;
    private static bool hasInitializedItems;

    [HarmonyPatch(nameof(MenuManager.Start)), HarmonyPostfix]
    private static void Start_Postfix(MenuManager __instance)
    {
        if (__instance.isInitScene)
            return;

        CreateButton();
        InitializeItems();
    }

    [HarmonyPatch(nameof(MenuManager.SetLoadingScreen)), HarmonyPrefix]
    private static void SetLoadingScreen_Prefix(bool isLoading)
    {
        customUI?.SetActive(!isLoading);
    }

    [HarmonyPatch(nameof(MenuManager.EnableUIPanel)), HarmonyPrefix]
    private static void EnableUIPanel_Prefix(GameObject enablePanel)
    {
        customUI?.SetActive(enablePanel.name == Constants.MAIN_MENU_CENTRAL_MENU_NAME);
    }
    
    [HarmonyPatch(nameof(MenuManager.DisableUIPanel)), HarmonyPrefix]
    private static void DisableUIPanel_Prefix(GameObject enablePanel)
    {
        customUI?.SetActive(enablePanel.name != Constants.MAIN_MENU_CENTRAL_MENU_NAME);
    }

    private static void CreateButton()
    {
        // Fetch container
        var container = GameObject.Find(Constants.MAIN_MENU_PARENT_PATH);

        if (container == null)
        {
            Logger.Error($"Could not find the container at '{Constants.MAIN_MENU_PARENT_PATH}'.");
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

        var index = container.transform.Find(Constants.MAIN_MENU_SIBLING_BEFORE)?.GetSiblingIndex() ?? -1;

        if (index != -1)
            ui.transform.SetSiblingIndex(index);

        if (LethalMuseum.MUSEUM_FORM != null)
            customUI = Object.Instantiate(LethalMuseum.MUSEUM_FORM, ui.transform, false);
        else
            Logger.Error("Could not spawn the main form.");
    }

    private static void InitializeItems()
    {
        if (hasInitializedItems)
            return;
        
        var items = Resources.FindObjectsOfTypeAll<Item>();
        
        foreach (var item in items)
        {
            if (item == null)
                continue;
            
            Objects.Register.RegisterItem(item);
        }

        hasInitializedItems = true;
    }
}