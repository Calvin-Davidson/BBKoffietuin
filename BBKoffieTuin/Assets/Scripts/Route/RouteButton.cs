using System;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

namespace Route
{
    /// <summary>
    /// Route button can be placed on a 'button' in for example the main menu and handles that when the button is clicked
    /// the user is send to the route section of the application. and the route is set to the route that is connected to the button.
    /// </summary>
    public class RouteButton : MonoBehaviour
    {
        [SerializeField] private string routeJson;
        private Route _route;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            GUIUtility.systemCopyBuffer = JsonConvert.SerializeObject(RouteHelper.GetDebugRoute());
            if (gameObject.TryGetComponent(out Button button))
            {
                button.onClick.AddListener(OnClick);
            }
        }

        /// <summary>
        /// OnClick should be called when the button is clicked and will try to start the route.
        /// </summary>
        private void OnClick()
        {
            TryStartRoute();
        }

        /// <summary>
        /// Will check if route can be started and if so will start the route. and enable all objects needed.
        /// </summary>
        private void TryStartRoute()
        {
            //Before we can start we have to make sure we have GPS permission!
            GpsService.Instance.TryStartingLocationServices(() =>
            {
                RouteHandler.Instance.ActiveRoute = Route;
                MenuHandler.Instance.OpenRouteMenu();
                //START THE ROUTE!
            }, () =>
            {
                Debug.Log("NO GPS PERMISSION CAN'T START");
                //NO PERMISSION!
            }, () =>
            {
                Debug.Log("SOMETHING WENT WRONG!");
                //SOMETHING WENT WRONG IN GENERAL!
            });
        }

        private Route Route
        {
            get
            {
                if (_route != null) return _route;
                
                try
                {
                    _route = JsonConvert.DeserializeObject<Route>(routeJson);
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                    return null;
                }

                return _route;
            }
        }
    }
}