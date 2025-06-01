using UnityEngine;

namespace LethalMuseum;

internal static class Constants
{
    #region Main Menu

    /// <summary>
    /// Path to the menu container in the main menu
    /// </summary>
    public const string MAIN_MENU_PARENT_PATH = "Canvas/MenuContainer";

    /// <summary>
    /// Name of the sibling before the main menu button
    /// </summary>
    public const string MAIN_MENU_SIBLING_BEFORE = "LobbyHostSettings";

    /// <summary>
    /// Name of the menu that is the central part of the main menu
    /// </summary>
    public const string MAIN_MENU_CENTRAL_MENU_NAME = "MainButtons";

    #endregion

    #region In Game

    /// <summary>
    /// Path to the in-game canvas
    /// </summary>
    public const string IN_GAME_PARENT_PATH = "Systems/UI/Canvas";
    
    /// <summary>
    /// Name of the sibling before the in-game UI
    /// </summary>
    public const string IN_GAME_SIBLING_BEFORE = "LoadingText";
    
    #endregion

    public const int ITEMS_PER_PAGE = 16;

    public static Color BLACKED_COLOR = new(0.3f, 0.3f, 0.3f);
}