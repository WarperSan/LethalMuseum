using LethalMuseum.Dependencies.InputUtils;
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

        if (CustomInputActions.Actions != null)
        {
            if (CustomInputActions.Actions.ToggleVisibilityKey != null)
                CustomInputActions.Actions.ToggleVisibilityKey.performed += _ => OnToggle(!gameObject.activeSelf);

            if (CustomInputActions.Actions.PageLeftKey != null)
                CustomInputActions.Actions.PageLeftKey.performed += _ => OnPageMove(true);
            
            if (CustomInputActions.Actions.PageRightKey != null)
                CustomInputActions.Actions.PageRightKey.performed += _ => OnPageMove(false);
        }
    }

    private void OnToggle(bool isActive)
    {
        gameObject.SetActive(isActive);
        
        Helpers.Audio.PlayUI(
            isActive
                ? GameNetworkManager.Instance?.buttonPressSFX
                : GameNetworkManager.Instance?.buttonCancelSFX
        );
    }

    private void OnPageMove(bool scrollLeft)
    {
        if (scrollLeft)
            pageIndex--;
        else
            pageIndex++;

        if (pageIndex < 0)
        {
            pageIndex = 0;
            Helpers.Audio.PlayUI(GameNetworkManager.Instance?.buttonCancelSFX);
        }
        else if (pageIndex > int.MaxValue)
        {
            pageIndex = int.MaxValue;
            Helpers.Audio.PlayUI(GameNetworkManager.Instance?.buttonCancelSFX);
        }
        else
            Helpers.Audio.PlayUI(GameNetworkManager.Instance?.buttonTuneSFX);
        
        UpdatePage();
    }

    #endregion

    #region Pages

    private int pageIndex;

    private void UpdatePage()
    {
        pageText?.SetText((pageIndex + 1).ToString());

        if (leftPageIcon != null)
            leftPageIcon.enabled = pageIndex > 0;

        if (rightPageIcon != null)
            rightPageIcon.enabled = pageIndex < int.MaxValue;
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