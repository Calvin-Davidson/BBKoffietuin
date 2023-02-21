using Toolbox.Utilities;
using UnityEngine;

public class MenuHandler : MonoSingleton<MenuHandler>
{
    public GameObject mainMenu;
    public GameObject routeMenu;
    
    public void OpenRouteMenu()
    {
        DisableAllMenus();
        routeMenu.SetActive(true);
    }
    
    public void OpenMainMenu()
    {
        DisableAllMenus();
        mainMenu.SetActive(true);
    }

    public void DisableAllMenus()
    {
        mainMenu.SetActive(false);
        routeMenu.SetActive(false);
    }

    public override void Awake()
    {
        base.Awake();
        OpenMainMenu();
    }
}
