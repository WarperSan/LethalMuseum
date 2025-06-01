using HarmonyLib;
using LethalMuseum.Objects;
using RuntimeIcons.Components;

namespace LethalMuseum.Dependencies.RuntimeIcons;

[HarmonyPatch(typeof(CameraQueueComponent.RenderingRequest))]
internal class RenderingRequest_Patches
{
    [HarmonyPatch(MethodType.Constructor, [typeof(GrabbableObject), typeof(UnityEngine.Sprite)]), HarmonyPostfix]
    private static void Test(ref CameraQueueComponent.RenderingRequest __instance)
    {
        var entries = Identifier.GetEntries(__instance.GrabbableObject);

        if (entries.Length == 0)
            return;
        
        var field = AccessTools.Field(
            typeof(CameraQueueComponent.RenderingRequest),
            nameof(CameraQueueComponent.RenderingRequest.ItemKey)
        );

        if (field == null)
            return;
        
        field.SetValueDirect(__makeref(__instance), entries[^1].ID);
    }

    [HarmonyPatch(nameof(CameraQueueComponent.RenderingRequest.HasIcon), MethodType.Getter), HarmonyPrefix]
    private static bool HasIcon_Prefix(ref CameraQueueComponent.RenderingRequest __instance, ref bool __result)
    {
        var entries = Identifier.GetEntries(__instance.GrabbableObject);
        
        if (entries.Length == 0)
            return false;

        if (Dependency.HasCustomIcon(entries[^1], out _))
        {
            __result = true;
            return true;
        }

        return false;
    }
}