using System;
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
        private Route route;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
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
            if (route == null)
            {
                //try set route from routeJson
                try
                {
                    route = JsonUtility.FromJson<Route>(routeJson);
                }
                catch (Exception e)
                {
                    return;
                }
            }

            if (route == null) return;

            //Before we can start we have to make sure we have GPS permission!
            GpsService.Instance.TryStartingLocationServices(() =>
            {
                Destroy(this.gameObject);
                Debug.Log("START ROUTE");
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
    }
}