using LethalMuseum.Objects;
using LethalMuseum.Objects.Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#pragma warning disable CS0649

namespace LethalMuseum.UI.Elements;

public class ItemBoard : MonoBehaviour
{
    #region Fields

    [SerializeField] private Image? icon;
    [SerializeField] private TMP_Text? text;
    [SerializeField] private Image? collectedBackground;

    #endregion

    private ItemEntry? shownItem;

    internal void SetItem(ItemEntry item)
    {
        shownItem = item;

        var showIcon = item.Icon != null && item.Icon.name != Constants.SCRAP_ICON_NAME;
        var showText = !showIcon;

        if (icon != null)
            icon.enabled = showIcon;

        if (text != null)
            text.enabled = showText;
        
        UpdateSelf();
    }

    internal void OnItemUpdate(string id)
    {
        if (!shownItem.HasValue || shownItem.Value.ID != id)
            return;
        
        UpdateSelf();
    }
    
    private void UpdateSelf()
    {
        if (!shownItem.HasValue)
            return;

        if (icon != null)
            icon.sprite = shownItem.Value.Icon;

        if (text != null)
            text.text = shownItem.Value.Name;

        if (collectedBackground != null)
            collectedBackground.enabled = Tracker.Instance?.IsCollected(shownItem.Value.ID) ?? false;
    }
}