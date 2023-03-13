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
        [SerializeField] private MiniGamesReferences miniGamesReferences;

        private void Start()
        {
            RouteHandler.Instance.onRouteChanged.AddListener(HandleRouteChange);
            
        }

        private void HandleRouteChange()
        {
            //enable the start game button based on if the route point has an mini game.
            RouteHandler.Instance.onPointReached.AddListener((routePoint, i) =>
            {
                if (routePoint.MiniGameOption == MiniGameOption.None) return;
                StartMiniGame();
            });
        }
        
        /// <summary>
        /// Spawns the mini game prefab 
        /// </summary>
        private void StartMiniGame()
        {
            RoutePoint routePoint = RouteHandler.Instance.ActiveRoutePoint;
            var miniGame = Instantiate(miniGamesReferences.miniGames.First(x => x.type == routePoint.MiniGameOption).obj, transform, true);
            miniGame.transform.localPosition = Vector3.zero;
            miniGame.transform.localScale = Vector3.one;
        }
    }
}