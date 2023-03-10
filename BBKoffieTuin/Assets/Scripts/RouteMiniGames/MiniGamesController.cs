using System.Linq;
using enums;
using Route;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

namespace RouteMiniGames
{
    public class MiniGamesController : MonoBehaviour
    {
        [SerializeField] MiniGamesReferences miniGamesReferences;
        [SerializeField] private Button miniGameStartButton;
        
    
        private void Awake()
        {
            miniGameStartButton.gameObject.SetActive(false);
            //enable the start game button based on if the route point has an mini game.
            RouteHandler.Instance.onPointReached.AddListener((routePoint, i) =>
            {
                miniGameStartButton.onClick.RemoveListener(StartMiniGame);

                if (routePoint.MiniGameOptions == MiniGameOptions.None)
                {
                    miniGameStartButton.gameObject.SetActive(false);    
                    return;
                }
                
                miniGameStartButton.onClick.AddListener(StartMiniGame);
                miniGameStartButton.gameObject.SetActive(true);
            });
            
        }

        /// <summary>
        /// Spawns the mini game prefab 
        /// </summary>
        private void StartMiniGame()
        {
            print("starting");
            RoutePoint routePoint = RouteHandler.Instance.ActiveRoutePoint;
            var miniGame = Instantiate(miniGamesReferences.miniGames.First(x => x.type == routePoint.MiniGameOptions).obj, transform, true);
            miniGame.transform.localPosition = Vector3.zero;
            miniGame.transform.localScale = Vector3.one;
        }
    }
}