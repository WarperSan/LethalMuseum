using HarmonyLib;
using LethalMuseum.Helpers;
using RuntimeIcons.Components;

namespace LethalMuseum.Dependencies.RuntimeIcons;

[HarmonyPatch(typeof(CameraQueueComponent))]
internal class CameraQueueComponent_Patches
{
    [HarmonyPatch(nameof(CameraQueueComponent.PullLastRender)), HarmonyPostfix]
    private static void PullLastRender_Postfix(CameraQueueComponent.RenderingResult render, ref bool __result)
    {
        if (!__result)
            return;
        
        Dependency.OnRender(render.Request.GrabbableObject, render.Texture);
    }

    [HarmonyPatch(nameof(CameraQueueComponent.EnqueueObject)), HarmonyPostfix]
    private static void Test(GrabbableObject grabbableObject, ref bool __result)
    {
        Logger.Info("State of the enqueue: " + grabbableObject.itemProperties.itemName + " > " + __result);
    }
}