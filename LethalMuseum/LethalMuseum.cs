using BepInEx;
using HarmonyLib;
using LethalMuseum.Helpers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LethalMuseum;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class LethalMuseum : BaseUnityPlugin
{
    private void Awake()
    {
        Helpers.Logger.SetLogger(Logger);
        
        if (!LoadAssets("lm-bundle"))
            return;

        Patch();
        
        SceneManager.sceneLoaded += OnSceneLoaded;

        Helpers.Logger.Info($"{MyPluginInfo.PLUGIN_GUID} v{MyPluginInfo.PLUGIN_VERSION} has loaded!");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Helpers.Logger.Info(scene.name);
        
        if (scene.name != Constants.LOAD_ITEMS_SCENE)
            return;

        var items = Resources.FindObjectsOfTypeAll<Item>();
        
        Helpers.Logger.Info("Items loaded count: " + items.Length);

        foreach (var item in items)
        {
            if (item == null)
                continue;
            
            Helpers.Logger.Info("Item loaded: " + item.itemName);
        }
    }

    #region Bundle

    internal static GameObject? ITEMS_BOARD;
    internal static GameObject? ITEM_BOARD;

    private static bool LoadAssets(string bundleName)
    {
        if (!Bundle.LoadBundle(bundleName))
            return false;

        ITEMS_BOARD = Bundle.LoadAsset<GameObject>("ItemsBoard");
        ITEM_BOARD = Bundle.LoadAsset<GameObject>("ItemBoard");
        
        return true;
    }

    #endregion

    #region Harmony

    private Harmony? Harmony;

    private void Patch()
    {
        Harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);
        Harmony.PatchAll();
    }

    #endregion
}