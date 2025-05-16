using LethalMuseum.Dependencies.LethalLib;
using LethalMuseum.Objects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#pragma warning disable CS0649

namespace LethalMuseum.UI;

public class ItemBoard : MonoBehaviour
{
    #region Fields

    [SerializeField] private Image? icon;
    [SerializeField] private TMP_Text? text;
    [SerializeField] private Image? collectedBackground;

    #endregion

    private string? targetId;
    private Item? shownItem;

    internal void SetItem(Item item)
    {
        targetId = ItemIdentifier.GetID(item);
        shownItem = item;

        var showIcon = item.itemIcon != null && item.itemIcon.name != Constants.SCRAP_ICON_NAME;
        var showText = !showIcon;

        if (icon != null)
            icon.enabled = showIcon;

        if (text != null)
            text.enabled = showText;
        
        UpdateSelf();
    }

    internal void OnItemUpdate(string id)
    {
        if (targetId != id)
            return;
        
        UpdateSelf();
    }
    
    private void UpdateSelf()
    {
        if (shownItem == null)
            return;

        if (icon != null)
            icon.sprite = shownItem.itemIcon;

        if (text != null)
            text.text = shownItem.itemName ?? "Scrap";

        if (Tracker.Instance != null && collectedBackground != null)
            collectedBackground.enabled = Tracker.Instance.IsCollected(shownItem);
    }
}