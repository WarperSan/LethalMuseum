using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using LethalMuseum.Dependencies.InputUtils;
using LethalMuseum.Dependencies.LethalLib;
using LethalMuseum.Helpers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LethalMuseum;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
[BepInDependency(LethalCompanyInputUtils.PluginInfo.PLUGIN_GUID)]
[BepInDependency(LethalLib.Plugin.ModGUID)]
public class LethalMuseum : BaseUnityPlugin
{
    private void Awake()
    {
        Helpers.Logger.SetLogger(Logger);
        
        if (!LoadAssets("lm-bundle"))
            return;

        LoadConfiguration(Config);
        LoadDependencies();
        Patch();
        
        SceneManager.sceneLoaded += OnSceneLoaded;

        Helpers.Logger.Info($"{MyPluginInfo.PLUGIN_GUID} v{MyPluginInfo.PLUGIN_VERSION} has loaded!");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != Constants.LOAD_ITEMS_SCENE)
            return;

        var items = Resources.FindObjectsOfTypeAll<Item>();
        
        foreach (var item in items)
        {
            if (item == null)
                continue;
            
            Objects.Register.RegisterItem(item);
        }
        
        if (Configuration != null)
            Objects.Register.ApplyBlacklist(Configuration.Blacklist.Value);
    }

    #region Bundle

    internal static GameObject? ITEMS_BOARD;
    internal static GameObject? MUSEUM_FORM;

    private static bool LoadAssets(string bundleName)
    {
        if (!Bundle.LoadBundle(bundleName))
            return false;

        ITEMS_BOARD = Bundle.LoadAsset<GameObject>("ItemsBoard");
        MUSEUM_FORM = Bundle.LoadAsset<GameObject>("MuseumForm");
        
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

    #region Dependencies

    private static void LoadDependencies()
    {
        CustomInputActions.Actions = new CustomInputActions();
        ModdedItemIdentifier.LoadModdedItems();
    }

    #endregion

    #region Configuration

    internal static Configuration? Configuration;

    private static void LoadConfiguration(ConfigFile file)
    {
        Configuration = new Configuration(file);
    }

    #endregion
}