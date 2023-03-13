using Toolbox.Utilities;
using UnityEngine;

public class MenuHandler : MonoSingleton<MenuHandler>
{
    [SerializeReference] private GameObject routeMenu;
    
    public void OpenRouteMenu()
    {
        DisableAllMenus();
        routeMenu.SetActive(true);
    }
    
    public void OpenMainMenu()
    {
        DisableAllMenus();
    }

    public void DisableAllMenus()
    {
        routeMenu.SetActive(false);
    }

    public override void Awake()
    {
        base.Awake();
        OpenMainMenu();
    }
}
