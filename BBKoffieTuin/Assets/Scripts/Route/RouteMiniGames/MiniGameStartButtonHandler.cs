using System;
using enums;
using Toolbox.Attributes;
using UnityEngine;
using UnityEngine.UI;

namespace Route.RouteMiniGames
{
    public class MiniGameStartButtonHandler : MonoBehaviour
    {
        [SerializeField] private Button startMiniGameButton;
        private void Awake()
        {
            startMiniGameButton.gameObject.SetActive(false);
            //enable the start game button based on if the route point has an mini game.
            RouteHandler.Instance.onPointReached.AddListener((routePoint, i) =>
            {
                print(routePoint.MiniGameOptions);
                if (routePoint.MiniGameOptions == MiniGameOptions.None)
                {
                    startMiniGameButton.gameObject.SetActive(false);        
                    return;
                }
                
                startMiniGameButton.gameObject.SetActive(true);
            });
        }
    }
}