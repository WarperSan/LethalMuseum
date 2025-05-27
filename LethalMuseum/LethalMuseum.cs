using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using LethalMuseum.Dependencies.InputUtils;
using LethalMuseum.Dependencies.LethalLib;
using LethalMuseum.Helpers;
using UnityEngine;

namespace LethalMuseum;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
[BepInDependency(LethalCompanyInputUtils.PluginInfo.PLUGIN_GUID)]
[BepInDependency(LethalLib.Plugin.ModGUID)]
[BepInDependency(LethalConfig.PluginInfo.Guid, BepInDependency.DependencyFlags.SoftDependency)]
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
        
        Helpers.Logger.Info($"{MyPluginInfo.PLUGIN_GUID} v{MyPluginInfo.PLUGIN_VERSION} has loaded!");
    }

    #region Bundle

    internal static GameObject? ITEMS_BOARD;
    internal static GameObject? MUSEUM_FORM;
    internal static Texture2D? MOD_ICON;

    private static bool LoadAssets(string bundleName)
    {
        if (!Bundle.LoadBundle(bundleName))
            return false;

        ITEMS_BOARD = Bundle.LoadAsset<GameObject>("ItemsBoard");
        MUSEUM_FORM = Bundle.LoadAsset<GameObject>("MuseumForm");
        MOD_ICON = Bundle.LoadAsset<Texture2D>("lm-icon");
        
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
        
        if (Dependencies.LethalConfig.ConfigToUI.Enabled)
            Dependencies.LethalConfig.ConfigToUI.AddConfigs(Configuration);
    }

    #endregion

    #region Configuration

    internal static Configuration? Configuration;

    private static void LoadConfiguration(ConfigFile file)
    {
        Configuration = new Configuration(file);

        Configuration.Blacklist.SettingChanged += (_, _) =>
        {
            Objects.Register.ApplyBlacklist(Configuration.Blacklist.Value);
        };
        
        Objects.Register.ApplyBlacklist(Configuration.Blacklist.Value);
    }

    #endregion
}