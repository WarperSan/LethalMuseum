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

    internal void SetItem(Item item, DisplayItemMode mode)
    {
        if (icon != null)
        {
            icon.sprite = item.itemIcon;
            icon.enabled = mode.HasFlag(DisplayItemMode.ICON);
        }

        if (text != null)
        {
            text.text = item.itemName ?? "Scrap";
            text.enabled = mode.HasFlag(DisplayItemMode.TEXT);
        }

        if (Tracker.Instance != null && collectedBackground != null)
            collectedBackground.enabled = Tracker.Instance.IsCollected(item);
    }
}