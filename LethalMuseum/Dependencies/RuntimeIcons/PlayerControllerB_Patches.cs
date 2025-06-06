﻿using GameNetcodeStuff;
using HarmonyLib;
using LethalMuseum.Objects;

namespace LethalMuseum.Dependencies.RuntimeIcons;

[HarmonyPatch(typeof(PlayerControllerB))]
internal class PlayerControllerB_Patches
{
    [HarmonyPatch(nameof(PlayerControllerB.ConnectClientToPlayerObject)), HarmonyPostfix]
    private static void ConnectClientToPlayerObject_Postfix(PlayerControllerB __instance)
    {
        if (LethalMuseum.Configuration == null || !LethalMuseum.Configuration.AutomaticIconGeneration.Value)
            return;
        
        var entries = Register.GetAll();
        Dependency.TryEnqueueAll(entries);
    }
}