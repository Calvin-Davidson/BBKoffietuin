using System;
using TMPro;
using UnityEngine;

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
            var distance = nextPoint.Coordinates.DistanceTo(Input.location.lastData.latitude, Input.location.lastData.longitude);

            distanceDebugText.text = "Distance to next point (" + nextPoint.pointName + "): " + (distance * 1000) + " meters ";
        }

        private void Awake()    
        {
            routeHandler.onNextPointReached.AddListener((point, index) =>
            {
                triggeredDebugText.text = "You reached next point: " + point.pointName + " at index: " + index;
            });
            
            routeHandler.onFurtherPointReached.AddListener((point, index) =>
            {
                triggeredDebugText.text = "You reached further point: " + point.pointName + " at index: " + index;
            });
            
            routeHandler.onAlreadyReachedPointReached.AddListener((point, index) =>
            {
                triggeredDebugText.text = "already reached point : " + point.pointName + " at index: " + index;
            });
            
            routeHandler.onFinalPointReached.AddListener((point, index) =>
            {
                triggeredDebugText.text = "Final point reached! : " + point.pointName + " at index: " + index;
            });
            
            routeHandler.onFinalPointLeft.AddListener((point, index) =>
            {
                triggeredDebugText.text = "Final point left! : " + point.pointName + " at index: " + index;
            });
        }
        
    }
}