using System.Collections.Generic;
using System.Linq;
using Generic;
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
        public UnityEvent<RoutePoint, int> onFinalPointReached = new UnityEvent<RoutePoint, int>();
        public UnityEvent<RoutePoint, int> onFinalPointLeft = new UnityEvent<RoutePoint, int>();
        public UnityEvent onRouteChanged = new UnityEvent();
        
        public void Update()
        {
            if (!GpsService.Instance.GpsServiceEnabled) return;
            if (_activeRoute == null) return;
            
            CheckPointReaches();
        }

        private void CheckPointReaches()
        {
            var userCoords = new Coordinates(Input.location.lastData.latitude, Input.location.lastData.longitude, Input.location.lastData.altitude);
            CheckLeftPoints(userCoords);
            CheckReachedNextPoint(userCoords);
            CheckReachedOldOrFurtherPoint(userCoords);
        }
        
        private void CheckLeftPoints(Coordinates userCoords)
        {
            //check if we are still on triggered points if not remove 
            foreach (var triggeredPoint in ActiveRoute.PointsOfInterest.Where(p => p.isTriggered))
            {
                if (HasReachedPoint(userCoords.latitude, userCoords.longitude, triggeredPoint)) continue;
                triggeredPoint.isTriggered = false;

                //check if we left the final point.
                if (triggeredPoint == ActiveRoute.GetFinalPoint())
                {
                    onFinalPointLeft.Invoke(triggeredPoint, ActiveRoute.PointsOfInterest.Count - 1);
                }
            }
        }
        
        private bool CheckReachedNextPoint(Coordinates userCoords)
        {
            var pointToReach = ActiveRoute.GetNextPointToReach();
            var pointToReachIndex = ActiveRoute.GetNextPointToReachIndex();
            
            if (pointToReach == null) return false;
            if (pointToReach.HasTriggered) return false;
            if (!HasReachedPoint(userCoords.latitude, userCoords.longitude, pointToReach)) return false;

            ReachedNextPoint(pointToReach, pointToReachIndex);
            return true;
        }

        private void CheckReachedOldOrFurtherPoint(Coordinates userCoords)
        {
            var index = -1;
            foreach (var routePoint in _activeRoute.PointsOfInterest)
            {
                index++;
                
                if(routePoint.isTriggered) continue;

                bool hasReachedFurtherPoint = CheckReachedFurtherPoint(userCoords, routePoint, index);
                bool hasReachedPreviousPoint = CheckReachedFurtherPoint(userCoords, routePoint, index);
            }
        }

        private bool CheckReachedFurtherPoint(Coordinates userCoords, RoutePoint routePoint, int index)
        {
            if (!HasReachedPoint(userCoords.latitude, userCoords.longitude, routePoint)) return false;
            if (!routePoint.HasTriggered) return false;

            ReachedFurtherPoint(routePoint, index);

            return true;
        }
        
        private bool CheckReachedAlreadyReachedPoint(Coordinates userCoords, RoutePoint routePoint, int index)
        {
            if (!HasReachedPoint(userCoords.latitude, userCoords.longitude, routePoint)) return false;
            if (routePoint.HasTriggered) return false;
            
            ReachedAlreadyReachedPoint(routePoint, index);
            
            return true;
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
            routePoint.onPointReached.Invoke();
            
            routePoint.HasTriggered = true;
            routePoint.isTriggered = true;

            if (routePoint == ActiveRoute.GetFinalPoint())
            {
                onFinalPointReached.Invoke(routePoint, index);
            }
            
            onNextPointReached.Invoke(routePoint, index);
            onPointReached.Invoke(routePoint, index);
        }

        /// <summary>
        /// Gets triggered when you reach a point that you already reached.
        /// </summary>
        private void ReachedAlreadyReachedPoint(RoutePoint routePoint, int index)
        {
            routePoint.onPointReached.Invoke();
            routePoint.isTriggered = true;
            
            onAlreadyReachedPointReached.Invoke(routePoint, index);
            onPointReached.Invoke(routePoint, index);
        }

        /// <summary>
        /// Gets triggered when you reach a point but you skipped some points in the process.
        /// </summary>
        private void ReachedFurtherPoint(RoutePoint routePoint, int index)
        {
            routePoint.onPointReached.Invoke();
            routePoint.isTriggered = true;
            
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