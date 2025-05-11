using BepInEx;
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
        
        if (!Bundle.LoadBundle("lm-bundle"))
            return;
        
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
}