using System.Collections.Generic;
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
    [SerializeField] private List<Sprite>? invalidIcons;

    #endregion

    public void SetItem(Item item)
    {
        var isValidIcon = invalidIcons?.Contains(item.itemIcon) ?? true;

        if (icon != null)
        {
            icon.sprite = item.itemIcon;
            icon.enabled = isValidIcon;
        }

        if (text != null)
        {
            text.text = item.itemName ?? "Scrap";
            text.enabled = !isValidIcon;
        }
    }
}