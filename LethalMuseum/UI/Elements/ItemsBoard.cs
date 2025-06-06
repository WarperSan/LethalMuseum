﻿using System;
using LethalMuseum.Dependencies.InputUtils;
using LethalMuseum.Objects;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace LethalMuseum.UI.Elements;

#pragma warning disable CS0649

public class ItemsBoard : MonoBehaviour
{
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
    
    internal static ItemsBoard? Instance; 

    private void Start()
    {
        Subscribe();
        UpdatePage();
        UpdateInformation();

        Instance = this;
    }

    private void OnDestroy()
    {
        UnSubscribe();

        if (Instance == this)
            Instance = null;
    }

    #endregion

    #region Information

    private void UpdateInformation()
    {
        var collectedCount = Tracker.Instance?.GetCollectedCount() ?? 0;
        var totalCount = Register.GetRegisteredCount();

        if (collectedAmountText != null)
        {
            collectedAmountText.text = collectedCount.ToString();
            collectedAmountText.color = collectedCount >= totalCount ? Color.green : Color.white;
        }

        if (totalAmountText != null)
        {
            totalAmountText.text = totalCount.ToString();
            totalAmountText.color = collectedCount >= totalCount ? Color.green : Color.white;
        }

        if (percentAmountText != null)
        {
            if (collectedCount < totalCount)
            {
                var percent = totalCount != 0 ? (float)collectedCount / totalCount * 100 : 0;
                percentAmountText.text = $"{percent:N1}%";
                percentAmountText.color = Color.white;
            }
            else
            {
                percentAmountText.text = "100%";
                percentAmountText.color = Color.green;
            }
        }
    }

    #endregion

    #region Pages

    private int pageIndex = 1;
    private event Action<string>? OnItemUpdated;

    private void OnPageMove(bool scrollLeft)
    {
        if (!gameObject.activeSelf)
            return;

        if (scrollLeft)
            pageIndex--;
        else
            pageIndex++;

        var maxPageCount = Register.GetPageCount(Constants.ITEMS_PER_PAGE);

        if (pageIndex < 1)
        {
            pageIndex = 1;
            Helpers.Audio.PlayUI(GameNetworkManager.Instance?.buttonCancelSFX);
        }
        else if (pageIndex > maxPageCount)
        {
            pageIndex = maxPageCount;
            Helpers.Audio.PlayUI(GameNetworkManager.Instance?.buttonCancelSFX);
        }
        else
        {
            Helpers.Audio.PlayUI(GameNetworkManager.Instance?.buttonTuneSFX);
            UpdatePage();
        }
    }

    private void UpdatePage()
    {
        pageText?.SetText(pageIndex.ToString());

        if (leftPageIcon != null)
            leftPageIcon.enabled = pageIndex > 1;

        if (rightPageIcon != null)
            rightPageIcon.enabled = pageIndex < Register.GetPageCount(Constants.ITEMS_PER_PAGE);

        if (itemsContainer != null && itemPrefab != null)
        {
            foreach (Transform child in itemsContainer)
            {
                if (child.gameObject.TryGetComponent(out ItemBoard itemBoard))
                    OnItemUpdated -= itemBoard.OnItemUpdate;

                Destroy(child.gameObject);
            }

            var items = Register.GetPage(pageIndex - 1, Constants.ITEMS_PER_PAGE);
            
            foreach (var item in items)
            {
                var instance = Instantiate(itemPrefab, itemsContainer, false);

                instance.name = item.Name;

                if (instance.TryGetComponent(out ItemBoard itemBoard))
                {
                    itemBoard.SetItem(item);
                    OnItemUpdated += itemBoard.OnItemUpdate;
                }
            }
        }
    }

    public void UpdateItem(string id) => OnItemUpdated?.Invoke(id);
    
    #endregion

    #region Events

    private void Subscribe()
    {
        if (CustomInputActions.Actions != null)
        {
            if (CustomInputActions.Actions.ToggleVisibilityKey != null)
                CustomInputActions.Actions.ToggleVisibilityKey.performed += OnToggle;

            if (CustomInputActions.Actions.PageLeftKey != null)
                CustomInputActions.Actions.PageLeftKey.performed += MovePageLeft;

            if (CustomInputActions.Actions.PageRightKey != null)
                CustomInputActions.Actions.PageRightKey.performed += MovePageRight;
        }

        if (Tracker.Instance != null)
        {
            Tracker.Instance.OnCollected += OnCollected;
            Tracker.Instance.OnDiscarded += OnDiscarded;
        }
    }

    private void UnSubscribe()
    {
        if (CustomInputActions.Actions != null)
        {
            if (CustomInputActions.Actions.ToggleVisibilityKey != null)
                CustomInputActions.Actions.ToggleVisibilityKey.performed -= OnToggle;

            if (CustomInputActions.Actions.PageLeftKey != null)
                CustomInputActions.Actions.PageLeftKey.performed -= MovePageLeft;

            if (CustomInputActions.Actions.PageRightKey != null)
                CustomInputActions.Actions.PageRightKey.performed -= MovePageRight;
        }
        
        if (Tracker.Instance != null)
        {
            Tracker.Instance.OnCollected -= OnCollected;
            Tracker.Instance.OnDiscarded -= OnDiscarded;
        }
    }
    
    private void OnToggle(InputAction.CallbackContext ctx)
    {
        if (Dependency.Enabled && !Dependency.AllowKeybind(ctx))
            return;
        
        var isActive = !gameObject.activeSelf;
        gameObject.SetActive(isActive);
        
        Helpers.Audio.PlayUI(
            isActive
                ? GameNetworkManager.Instance?.buttonPressSFX
                : GameNetworkManager.Instance?.buttonCancelSFX
        );
    }

    private void OnCollected(string id)
    {
        UpdateItem(id);
        UpdateInformation();
    }

    private void OnDiscarded(string id)
    {
        UpdateItem(id);
        UpdateInformation();
    }

    private void MovePageRight(InputAction.CallbackContext ctx)
    {
        if (Dependency.Enabled && !Dependency.AllowKeybind(ctx))
            return;

        OnPageMove(false);
    }

    private void MovePageLeft(InputAction.CallbackContext ctx)
    {
        if (Dependency.Enabled && !Dependency.AllowKeybind(ctx))
            return;

        OnPageMove(true);
    }

    #endregion
}