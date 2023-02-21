using System.Collections.Generic;
using System.Linq;
using Toolbox.Utilities;
using UnityEngine;
using UnityEngine.Events;

namespace Route
{
    public class RouteHandler : MonoSingleton<RouteHandler>
    {
        private Route _activeRoute = null;
        private readonly float _distanceInMetersForTrigger = 10;
        
        public UnityEvent<RoutePoint, int> onNextPointReached = new UnityEvent<RoutePoint, int>();
        public UnityEvent<RoutePoint, int> onFurtherPointReached = new UnityEvent<RoutePoint, int>();
        public UnityEvent<RoutePoint, int> onAlreadyReachedPointReached = new UnityEvent<RoutePoint, int>();
        public UnityEvent<RoutePoint, int> onPointReached = new UnityEvent<RoutePoint, int>();
        public UnityEvent onRouteChanged = new UnityEvent();


        private void Start()
        {
            ActiveRoute = RouteHelper.GetDebugRoute();
        }

        public void Update()
        {
            if (!GpsService.Instance.GpsServiceEnabled) return;
            if (_activeRoute == null) return;
            if (GpsService.Instance.GpsServiceEnabled == false) return;

            //the route is not null, so we can check if we are close to a point.

            int index = -1;
            var currentLat = Input.location.lastData.latitude;
            var currentLong = Input.location.lastData.longitude;

            //Check if we reached the next point in the route.
            RoutePoint nextPoint = _activeRoute.GetNextPointToReach();
            int nextPointIndex = _activeRoute.GetNextPointToReachIndex();
            
            if (nextPoint == null) return;
            if (nextPoint.HasTriggered) return;

            //check if we are still on triggered points if not remove 
            foreach (var triggeredPoint in ActiveRoute.PointsOfInterest.Where(p => p.isTriggered))
            {
                if(HasReachedPoint(currentLat, currentLong, triggeredPoint)) continue;
                triggeredPoint.isTriggered = false;
            }

            //check if we reached the point and if we weren't already on it.
            if (HasReachedPoint(currentLat, currentLong, nextPoint) && !nextPoint.isTriggered)
            {
                nextPoint.HasTriggered = true;
                nextPoint.isTriggered = true;
                
                nextPoint.onPointReached.Invoke();
                ReachedNextPoint(nextPoint, nextPointIndex);
                return;
            }

            //Check if we reached a point that we already reached. or if we skipped some points.
            foreach (var routePoint in _activeRoute.PointsOfInterest)
            {
                index++;
                if (!HasReachedPoint(currentLat, currentLong, routePoint)) continue;

                if (!routePoint.HasTriggered)
                {
                    nextPoint.isTriggered = true;
                    
                    ReachedFurtherPoint(routePoint, index);
                    return;
                }
                
                if (routePoint.HasTriggered)
                {
                    nextPoint.isTriggered = true;
                    
                    ReachedAlreadyReachedPoint(routePoint, index);
                    return;
                }
            }
        }

        /// <summary>
        /// Checks if the user has reached a point. 
        /// </summary>
        /// <param name="lat"></param>
        /// <param name="lon"></param>
        /// <param name="routePoint"></param>
        /// <returns></returns>
        public bool HasReachedPoint(double lat, double lon, RoutePoint routePoint)
        {
            var distanceInKm = routePoint.Coordinates.DistanceTo(lat, lon);
            var distanceInMeters = distanceInKm * 1000;

            bool reached = distanceInMeters < _distanceInMetersForTrigger;

            return reached;
        }

        /// <summary>
        /// Reached the next point in the route.
        /// </summary>
        private void ReachedNextPoint(RoutePoint routePoint, int index)
        {
            onNextPointReached.Invoke(routePoint, index);
            onPointReached.Invoke(routePoint, index);
        }

        /// <summary>
        /// Gets triggered when you reach a point that you already reached.
        /// </summary>
        private void ReachedAlreadyReachedPoint(RoutePoint routePoint, int index)
        {
            onAlreadyReachedPointReached.Invoke(routePoint, index);
            onPointReached.Invoke(routePoint, index);
        }

        /// <summary>
        /// Gets triggered when you reach a point but you skipped some points in the process.
        /// </summary>
        private void ReachedFurtherPoint(RoutePoint routePoint, int index)
        {
            onFurtherPointReached.Invoke(routePoint, index);
            onPointReached.Invoke(routePoint, index);
        }

        #region GETTERS & SETTERS
        
        /// <summary>
        /// Getter & setter for the active route.
        /// </summary>
        public Route ActiveRoute
        {
            get => _activeRoute;
            set
            {
                if (_activeRoute == value) return;

                _activeRoute = value;
                onRouteChanged?.Invoke();
            }
        }
        
        #endregion GETTERS & SETTERS
    }
}