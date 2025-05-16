using UnityEngine;
using UnityEngine.UI;

#pragma warning disable CS0649

namespace LethalMuseum.UI;

public class ToggleForm : MonoBehaviour
{
    #region Fields

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
}