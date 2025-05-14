using LethalMuseum.Dependencies.LethalLib;
using LethalMuseum.Objects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LethalMuseum.UI;

#pragma warning disable CS0649

public class ItemBoard : MonoBehaviour
{
    #region Fields

    [SerializeField] private Image? icon;
    [SerializeField] private TMP_Text? text;
    [SerializeField] private Image? collectedBackground;

    #endregion

    [System.Flags]
    internal enum DisplayItemMode
    {
        NONE = 0b0,
        ICON = 0b01,
        TEXT = 0b10
    }
    
    private string? targetId;
    private Item? shownItem;

    internal void SetItem(Item item, DisplayItemMode mode)
    {
        targetId = ItemIdentifier.GetID(item);
        shownItem = item;
        
        if (icon != null)
            icon.enabled = mode.HasFlag(DisplayItemMode.ICON);

        if (text != null)
            text.enabled = mode.HasFlag(DisplayItemMode.TEXT);
        
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