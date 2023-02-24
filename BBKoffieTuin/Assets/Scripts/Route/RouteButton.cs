using System;
using System.IO;
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
        [SerializeField] private TextAsset textAsset; //first option
        [SerializeField] private string routeJson; //second option
        private Route _route;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            if (gameObject.TryGetComponent(out Button button))
            {
                button.onClick.AddListener(Clicked);
            }
        }

        /// <summary>
        /// OnClick should be called when the button is clicked and will try to start the route.
        /// </summary>
        private void Clicked()
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
                //START THE ROUTE!
                RouteHandler.Instance.ActiveRoute = Route;
                MenuHandler.Instance.OpenRouteMenu();
            }, () =>
            {
                //NO PERMISSION!
                Debug.Log("NO GPS PERMISSION CAN'T START");
            }, () =>
            {
                //SOMETHING WENT WRONG IN GENERAL!
                Debug.Log("SOMETHING UNEXPECTED HAPPENED! BUT I DON'T KNOW WHAT!");
            });
        }

        private Route Route
        {
            get
            {
                if (_route != null) return _route;
                
                try
                {
                    //first try from text asset
                    if (textAsset != null)
                    {   
                        _route = JsonConvert.DeserializeObject<Route>(textAsset.text);
                        if (_route != null) return _route;
                    }

                    //then try from json string
                    _route = JsonConvert.DeserializeObject<Route>(routeJson);
                    if (_route != null) return _route;
                }
                catch (Exception e)
                {
                    Debug.Log("No valid json found!");
                    return null;
                }

                return _route;
            }
        }
    }
}