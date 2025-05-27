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
        if (ItemsBoard.Instance == null)
            return;
        
        var entries = Identifier.GetAllEntries(item);

        foreach (var entry in entries)
            ItemsBoard.Instance.UpdateItem(entry.ID);
    }
}