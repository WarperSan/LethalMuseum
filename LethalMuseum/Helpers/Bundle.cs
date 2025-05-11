﻿using System;
using System.IO;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LethalMuseum.Helpers;

/// <summary>
///     Helper to load a bundle
/// </summary>
internal static class Bundle
{
    private static AssetBundle? loadedBundle;

    /// <summary>
    ///     Tries to load the bundle with the given name
    /// </summary>
    /// <returns>Success of the load</returns>
    public static bool LoadBundle(string name)
    {
        string path = Assembly.GetExecutingAssembly().Location;
        path = Path.GetDirectoryName(path) ?? throw new NullReferenceException();
        path = Path.Combine(path, name);
        
        loadedBundle = AssetBundle.LoadFromFile(path);

        if (loadedBundle == null)
        {
            Logger.Error("Failed to load custom assets.");
            return false;
        }

        return true;
    }

    /// <summary>
    ///     Tries to load the asset of the given name in the current bundle
    /// </summary>
    /// <returns>Asset loaded or null</returns>
    public static T? LoadAsset<T>(string name) where T : Object
    {
        if (loadedBundle == null)
        {
            Logger.Error("Tried to load asset from unloaded bundle.");
            return null;
        }

        var asset = loadedBundle.LoadAsset<T>(name);

        if (asset == null)
            Logger.Error($"No asset named '{name}' was found.");

        return asset;
    }
    
    /// <summary>
    ///     Tries to load all the assets of the given type  in the current bundle
    /// </summary>
    /// <returns>Asset loaded or null</returns>
    public static T[]? LoadAllAsset<T>() where T : Object
    {
        if (loadedBundle == null)
        {
            Logger.Error("Tried to load assets from unloaded bundle.");
            return null;
        }

        var assets = loadedBundle.LoadAllAssets<T>();

        if (assets == null)
            Logger.Error($"No asset of type '{typeof(T)}' was found.");

        return assets;
    }
}