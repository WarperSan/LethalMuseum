using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LethalMuseum.UI;

#pragma warning disable CS0649

public class ItemsBoard : MonoBehaviour
{
    internal static ItemsBoard? Instance;
    
    #region Fields

    [Header("Items")]
    [SerializeField] private Transform? itemsContainer;
    [SerializeField] private GameObject? itemPrefab;

    [Header("Amounts")]
    [SerializeField] private TMP_Text? collectedAmountText;
    [SerializeField] private TMP_Text? totalAmountText;
    [SerializeField] private TMP_Text? percentAmountText;
    
    [Header("Pages")]
    [SerializeField] private TMP_Text? pageText;
    [SerializeField] private Graphic? leftPageIcon;
    [SerializeField] private Graphic? rightPageIcon;
    
    #endregion

    #region Unity

    private void Start()
    {
        Instance = this;
    }

    #endregion

    public void SetItems(Item[] items)
    {
        if (itemsContainer == null)
            return;

        foreach (Transform child in itemsContainer)
            Destroy(child);
        
        if (itemPrefab == null)
            return;
        
        foreach (var item in items)
        {
            var instance = Instantiate(itemPrefab, itemsContainer, false);
            
            if (instance.TryGetComponent(out ItemBoard itemBoard))
                itemBoard.SetItem(item);
        }
    }
}