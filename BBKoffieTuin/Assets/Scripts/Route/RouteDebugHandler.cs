using TMPro;
using Toolbox.Attributes;
using Toolbox.MethodExtensions;
using UnityEngine;
using UnityEngine.UI;

namespace Route
{
    /// <summary>
    /// Route debug handler is a temporary class that will be used to debug the route system.
    /// </summary>
    public class RouteDebugHandler : MonoBehaviour
    {
        [SerializeField] private RouteHandler routeHandler;
        public TMP_Text distanceDebugText;
        public TMP_Text triggeredDebugText;

        private void Update()
        {
            if (routeHandler == null || routeHandler.ActiveRoute == null)
            {
                distanceDebugText.text = "No active route";
                return;
            }

            if (!GpsService.Instance.GpsServiceEnabled)
            {
                distanceDebugText.text = "Gps service disabled";
                return;
            }
            
            var nextPoint = routeHandler.ActiveRoute.GetNextPointToReach();
            if(nextPoint == null)
            {
                distanceDebugText.text = "No next point";
                return;
            }
            
            var distance = nextPoint.Coordinates.DistanceTo(Input.location.lastData.latitude, Input.location.lastData.longitude);

            distanceDebugText.text = "Distance to next point (" + nextPoint.PointName + "): " + (distance * 1000) + " meters ";
        }

        private void Awake()    
        {
            routeHandler.onNextPointReached.AddListener((point, index) =>
            {
                triggeredDebugText.text = "You reached next point: " + point.PointName + " at index: " + index;
            });
            
            routeHandler.onFurtherPointReached.AddListener((point, index) =>
            {
                triggeredDebugText.text = "You reached further point: " + point.PointName + " at index: " + index;
            });
            
            routeHandler.onAlreadyReachedPointReached.AddListener((point, index) =>
            {
                triggeredDebugText.text = "already reached point : " + point.PointName + " at index: " + index;
            });
            
            routeHandler.onFinalPointReached.AddListener((point, index) =>
            {
                triggeredDebugText.text = "Final point reached! : " + point.PointName + " at index: " + index;
            });
            
            routeHandler.onFinalPointLeft.AddListener((point, index) =>
            {
                triggeredDebugText.text = "Final point left! : " + point.PointName + " at index: " + index;
            });
        }
    }
}