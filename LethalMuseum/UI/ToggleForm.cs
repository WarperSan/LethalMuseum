using LethalMuseum.Objects;
using UnityEngine;
using UnityEngine.UI;

#pragma warning disable CS0649

namespace LethalMuseum.UI;

public class ToggleForm : MonoBehaviour
{
    #region Fields
    
    [Header("Fields")]
    [SerializeField] private Transform? itemListContainer;
    [SerializeField] private GameObject? itemPrefab;
    [SerializeField] private GameObject? noItemText;

    [Header("Buttons")]
    [SerializeField] private Button? openBtn;
    [SerializeField] private Button? closeBtn;

    #endregion
    
    private void Start()
    {
        var menuManager = FindObjectOfType<MenuManager>();
        
        openBtn?.onClick.AddListener(OpenForm);
        openBtn?.onClick.AddListener(menuManager.PlayConfirmSFX);

        closeBtn?.onClick.AddListener(CloseForm);
        closeBtn?.onClick.AddListener(menuManager.PlayCancelSFX);
        
        var loadedItems = Register.GetAll();
        SetItems(loadedItems);
    }
    
    #region Animations

    [Header("Animations")]
    [SerializeField] private Animator? animator;

    private void OpenForm()
    {
        animator?.SetTrigger(OpenMenu);
    }

    private void CloseForm()
    {
        animator?.SetTrigger(CloseMenu);
    }

    private static readonly int OpenMenu = Animator.StringToHash("openMenu");
    private static readonly int CloseMenu = Animator.StringToHash("closeMenu");

    #endregion

    #region Items

    private void AddItem(Item item)
    {
        if (itemListContainer == null || itemPrefab == null)
            return;

        var newItem = Instantiate(itemPrefab, itemListContainer, false);

        if (newItem.TryGetComponent(out ItemList itemList))
        {
            itemList.SetItem(item);
            itemList.OnActiveChanged?.AddListener(isActive => Register.SetItemEnable(item, isActive));
        }
    }

    public void SetItems(Item[] items)
    {
        if (itemListContainer == null)
            return;
        
        foreach (Transform child in itemListContainer)
            Destroy(child.gameObject);

        noItemText?.SetActive(items.Length == 0);

        foreach (var item in items)
            AddItem(item);
    }

    #endregion
}