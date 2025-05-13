using LethalCompanyInputUtils.Api;
using LethalCompanyInputUtils.BindingPathEnums;
using UnityEngine.InputSystem;

namespace LethalMuseum.Dependencies.InputUtils;

internal class CustomInputActions : LcInputActions
{
    internal static CustomInputActions? Actions;
    
    [InputAction(KeyboardControl.P, Name = "Toggle Visibility")]
    public InputAction? ToggleVisibilityKey { get; set; }
    
    [InputAction(KeyboardControl.PageDown, Name = "Page Left")]
    public InputAction? PageLeftKey { get; set; }
    
    [InputAction(KeyboardControl.PageUp, Name = "Page Right")]
    public InputAction? PageRightKey { get; set; }
}