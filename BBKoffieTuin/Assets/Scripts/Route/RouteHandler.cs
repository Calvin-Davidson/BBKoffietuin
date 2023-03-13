using System.Collections.Generic;
using System.Linq;
using Generic;
using Newtonsoft.Json;
using Toolbox.Attributes;
using Toolbox.Utilities;
using UnityEngine;
using UnityEngine.Events;

namespace Route
{
    public class RouteHandler : MonoSingleton<RouteHandler>
    {
        private Route _activeRoute = null;
        private readonly float _distanceInMetersForTrigger = 15;
        private RoutePoint _activeRoutePoint = null;
        private int _activeRoutePointIndex = -1;

        public UnityEvent<RoutePoint, int> onNextPointReached = new UnityEvent<RoutePoint, int>();
        public UnityEvent<RoutePoint, int> onFurtherPointReached = new UnityEvent<RoutePoint, int>();
        public UnityEvent<RoutePoint, int> onAlreadyReachedPointReached = new UnityEvent<RoutePoint, int>();
        public UnityEvent<RoutePoint, int> onPointReached = new UnityEvent<RoutePoint, int>();
        public UnityEvent<RoutePoint, int> onFinalPointReached = new UnityEvent<RoutePoint, int>();
        public UnityEvent<RoutePoint, int> onFinalPointLeft = new UnityEvent<RoutePoint, int>();
        public UnityEvent onRouteChanged = new UnityEvent();

        [Button]
        public void DebugRoute()
        {
            print(ActiveRoute.RouteCode);
            print(ActiveRoute.RouteCode.Count);
            print(JsonConvert.SerializeObject(ActiveRoute.RouteCode));
        }   

        public override void Awake()
        {
            base.Awake();
            onPointReached.AddListener(UpdateActiveRoutePointCache);
        }



        public void Update()
        {
            if (!GpsService.Instance.GpsServiceEnabled) return;
            if (_activeRoute == null) return;

            CheckPointReaches();
        }

        private void CheckPointReaches()
        {
            var userCoords = new Coordinates(Input.location.lastData.latitude, Input.location.lastData.longitude,
                Input.location.lastData.altitude);
            CheckLeftPoints(userCoords);
            CheckReachedNextPoint(userCoords);
            CheckReachedOldOrFurtherPoint(userCoords);
        }

        private void CheckLeftPoints(Coordinates userCoords)
        {
            //check if we are still on triggered points if not remove 
            foreach (var triggeredPoint in ActiveRoute.PointsOfInterest.Where(p => p.IsTriggered))
            {
                if (HasReachedPoint(userCoords.latitude, userCoords.longitude, triggeredPoint)) continue;
                triggeredPoint.IsTriggered = false;

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

                if (routePoint.IsTriggered) continue;

                bool hasReachedFurtherPoint = CheckReachedFurtherPoint(userCoords, routePoint, index);
                bool hasReachedPreviousPoint = CheckReachedAlreadyReachedPoint(userCoords, routePoint, index);
            }
        }

        private bool CheckReachedFurtherPoint(Coordinates userCoords, RoutePoint routePoint, int index)
        {
            if (!HasReachedPoint(userCoords.latitude, userCoords.longitude, routePoint)) return false;
            if (routePoint.HasTriggered) return false;

            ReachedFurtherPoint(routePoint, index);

            return true;
        }

        private bool CheckReachedAlreadyReachedPoint(Coordinates userCoords, RoutePoint routePoint, int index)
        {
            if (!HasReachedPoint(userCoords.latitude, userCoords.longitude, routePoint)) return false;
            if (!routePoint.HasTriggered) return false;

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
        public void ReachedNextPoint(RoutePoint routePoint, int index)
        {
            routePoint.onPointReached.Invoke();
            onNextPointReached.Invoke(routePoint, index);
            onPointReached.Invoke(routePoint, index);

            routePoint.HasTriggered = true;
            routePoint.IsTriggered = true;

            //check if it is the final point.
            if (routePoint == ActiveRoute.GetFinalPoint())
            {
                onFinalPointReached.Invoke(routePoint, index);
            }
        }

        /// <summary>
        /// Gets triggered when you reach a point that you already reached.
        /// </summary>
        public void ReachedAlreadyReachedPoint(RoutePoint routePoint, int index)
        {
            onAlreadyReachedPointReached.Invoke(routePoint, index);
            onPointReached.Invoke(routePoint, index);
            routePoint.onPointReached.Invoke();

            routePoint.IsTriggered = true;
            routePoint.HasTriggered = true;

            //reset all further points to not triggered.
            for (int i = index + 1; i < ActiveRoute.PointsOfInterest.Count; i++)
            {
                var point = ActiveRoute.PointsOfInterest[i];
                point.IsTriggered = false;
                point.HasTriggered = false;
            }
        }

        /// <summary>
        /// Gets triggered when you reach a point but you skipped some points in the process.
        /// </summary>
        public void ReachedFurtherPoint(RoutePoint routePoint, int index)
        {
            onFurtherPointReached.Invoke(routePoint, index);
            onPointReached.Invoke(routePoint, index);
            routePoint.onPointReached.Invoke();

            routePoint.IsTriggered = true;
            routePoint.HasTriggered = true;

            //check if it is the final point.
            if (routePoint == ActiveRoute.GetFinalPoint())
            {
                onFinalPointReached.Invoke(routePoint, index);
            }

            //SET ALL POINTS BEFORE THIS POINT TO TRIGGERED.
            for (int i = 0; i < index; i++)
            {
                var point = ActiveRoute.PointsOfInterest[i];
                point.HasTriggered = true;
            }
        }

        public void GoToIndex(int index)
        {
            print("clicked index: " + index);
            bool isPreviousPoint = index < ActiveRoute.GetNextPointToReachIndex();
            bool isCurrentPoint = index == ActiveRoute.GetNextPointToReachIndex();
            bool isFurtherPoint = index > ActiveRoute.GetNextPointToReachIndex();

            RoutePoint clickedPoint = ActiveRoute.PointsOfInterest[index];
            print(clickedPoint.PointName);

            if (isPreviousPoint)
            {
                ReachedAlreadyReachedPoint(clickedPoint, index);
            }
            else if (isCurrentPoint)
            {
                ReachedNextPoint(clickedPoint, index);
            }
            else if (isFurtherPoint)
            {
                ReachedFurtherPoint(clickedPoint, index);
            }
        }
        
        public void UpdateActiveRoutePointCache(RoutePoint point, int index)
        {
            _activeRoutePoint = point;
            _activeRoutePointIndex = index;
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
                _activeRoute.InitializeRoute();
                onRouteChanged?.Invoke();
            }
        }

        public RoutePoint ActiveRoutePoint => _activeRoutePoint;

        public int ActiveRoutePointIndex => _activeRoutePointIndex;

        #endregion GETTERS & SETTERS
    }
}