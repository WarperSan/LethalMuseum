using HarmonyLib;
using LethalMuseum.Objects;
using LethalMuseum.UI.Elements;
using RuntimeIcons.Utils;

namespace LethalMuseum.Dependencies.RuntimeIcons;

[HarmonyPatch(typeof(HudUtils))]
internal class HudUtils_Patches
{
    [HarmonyPatch(nameof(HudUtils.UpdateIconsInHUD)), HarmonyPostfix]
    private static void UpdateIconsInHUD_Postfix(Item item)
    {
        var entries = Identifier.GetAllEntries(item);

        foreach (var entry in entries)
        {
            if (!entry.HasCustomIcon)
                continue;
            
            if (PlayerControllerB_Patches.objectToClear.TryGetValue(entry.ID, out var gameObject) && gameObject != null)
                UnityEngine.Object.Destroy(gameObject);
            
            PlayerControllerB_Patches.objectToClear.Remove(entry.ID);
            ItemsBoard.Instance?.UpdateItem(entry.ID);
        }
    }
}