using System;
using System.Collections.Generic;
using Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Route
{
    /// <summary>
    /// Handles everything related to the map.
    /// </summary>
    public class MapHandler : MonoBehaviour
    {
        [SerializeField] private RouteHandler routeHandler;
        [SerializeField] private Image mapImage;
        [SerializeField] private GameObject markerPrefab;
        [SerializeField] private GameObject userPrefab;

        private readonly List<GameObject> _markers = new List<GameObject>();
        private RectTransform _userRect;
        private GameObject _userGameObject;

        private void OnEnable()
        {
            routeHandler.onRouteChanged.AddListener(InitializeMap);
            PhoneDirection.Instance.onTargetPointChange.AddListener(UpdateUserRotation);
            InitializeMap();
        }

        private void OnDisable()
        {
            routeHandler.onRouteChanged.RemoveListener(InitializeMap);
            PhoneDirection.Instance.onTargetPointChange.RemoveListener(UpdateUserRotation);
        }

        private void Update()
        {
            UpdateUserPosition();
        }

        private void InitializeMap()
        {
            if (mapImage == null) return;
            if (routeHandler.ActiveRoute == null) return;

            var sprite = routeHandler.ActiveRoute.ImageSprite;
            if (sprite == null) return;
            
            mapImage.sprite = sprite;

            InstantiateMarkers();
            InitializeUserImage();
        }
        
        /// <summary>
        /// Create all marker object on the map by using the width and height en coordinates of the map and the coordinate of the marker.
        /// the created marker will also be set under the 'routePoint' object under the markerObject to be used.
        /// </summary>
        public void InstantiateMarkers()
        {
            if (routeHandler.ActiveRoute.ImageTexture == null)
            {
                Debug.LogWarning("The texture was null so we can't draw on it.");
                return;
            }

            var mapRect = mapImage.rectTransform.rect;
            var width = mapRect.width;
            var height = mapRect.height;

            int index = -1;
            foreach (var point in routeHandler.ActiveRoute.PointsOfInterest)
            {
                index++;
                //this gives us the 0 - 1 value of where the circle should be drawn.
                double x = GetHorizontalPosition(routeHandler.ActiveRoute.bounds.east, routeHandler.ActiveRoute.bounds.west, point.Coordinates.longitude);
                double y = GetVeritcalPosition(routeHandler.ActiveRoute.bounds.north, routeHandler.ActiveRoute.bounds.south, point.Coordinates.latitude);

                //instantiate markers
                var obj = Instantiate(markerPrefab, mapImage.transform, true);
                obj.name = "marker: " + point.PointName;
                var newReferenceIndex = index; //must be a new reference or the index will be the last one.
                obj.AddComponent<Clickable>().onClick.AddListener(() => { RouteHandler.Instance.GoToIndex(newReferenceIndex);});
                _markers.Add(obj);
                
                point.MarkerGameObject = obj;
                obj.GetComponentInChildren<TMP_Text>().text = point.PointName;
                var rect = obj.GetComponent<RectTransform>();
                rect.localScale = new Vector3(1,1,1);
                
                Vector2 newRectPosition = new Vector2();
                newRectPosition.x = (float)((width) * x) - width / 2;
                newRectPosition.y = (float)((height) * y) - height / 2;
                
                rect.localPosition = newRectPosition;
            }
        }

        /// <summary>
        /// Creates an GameObject for the user (the player) and sets the position to the position on the map image
        /// by using a 0-1 value by calculating the width and height of the map image and the coordinates of the map.
        /// an caches this object for later use.
        /// </summary>
        public void InitializeUserImage()
        {
            if (!GpsService.Instance.GpsServiceEnabled) return;
            if (routeHandler.ActiveRoute.ImageTexture == null)
            {
                Debug.LogWarning("The texture was null so we can't draw on it.");
                return;
            }

            var mapRect = mapImage.rectTransform.rect;
            var width = mapRect.width;
            var height = mapRect.height;
            
            //this gives us the 0 - 1 value of where the circle should be drawn.
            double x = GetHorizontalPosition(routeHandler.ActiveRoute.bounds.east, routeHandler.ActiveRoute.bounds.west, Input.location.lastData.latitude);
            double y = GetVeritcalPosition(routeHandler.ActiveRoute.bounds.north, routeHandler.ActiveRoute.bounds.south, Input.location.lastData.longitude);

            //instantiate markers
            _userGameObject = Instantiate(userPrefab, mapImage.transform, true);
            _userRect = _userGameObject.GetComponent<RectTransform>();
            _userRect.localScale = new Vector3(1,1,1);
                
            Vector2 newRectPosition = new Vector2();
            newRectPosition.x = (float)((width) * x) - width / 2;
            newRectPosition.y = (float)((height) * y) - height / 2;
                
            _userRect.localPosition = newRectPosition;
        }

        /// <summary>
        /// Updates the users object on the map by using a 0-1 value by calculating the width and height of the map image and the coordinates of the map.
        /// and the coordinates if the users device (in latitude and longitude).
        /// </summary>
        private void UpdateUserPosition()
        {
            if (!GpsService.Instance.GpsServiceEnabled) return;
            
            var mapRect = mapImage.rectTransform.rect;
            var width = mapRect.width;
            var height = mapRect.height;
            
            //this gives us the 0 - 1 value of where the circle should be drawn.
            double x = GetHorizontalPosition(routeHandler.ActiveRoute.bounds.east, routeHandler.ActiveRoute.bounds.west, Input.location.lastData.longitude);
            double y = GetVeritcalPosition(routeHandler.ActiveRoute.bounds.north, routeHandler.ActiveRoute.bounds.south, Input.location.lastData.latitude);

            Vector2 newRectPosition = new Vector2();
            newRectPosition.x = (float)((width) * x) - width / 2;
            newRectPosition.y = (float)((height) * y) - height / 2;
                
            _userRect.localPosition = newRectPosition;
        }

        /// <summary>
        /// Sets the users rotation to the new rotation.
        /// </summary>
        /// <param name="newRotation"></param>
        private void UpdateUserRotation(float newRotation)
        {
            _userRect.rotation = Quaternion.Euler(new Vector3(0,0,newRotation));
        }

        /// <summary>
        /// Gets a 0-1 value of the horizontal position of the map by using the width and height of the map and the coordinates of the map and user.
        /// </summary>
        /// <param name="east"></param>
        /// <param name="west"></param>
        /// <param name="longitude"></param>    
        /// <returns></returns>
        private double GetHorizontalPosition(double east, double west, double longitude)
        {
            var val = (longitude - west) / (east - west);
            return Math.Max(0, Math.Min(1, val));
        }
        
        /// <summary>
        /// Gets a 0-1 value of the vertical position of the map by using the width and height of the map and the coordinates of the map and user.
        /// </summary>
        /// <param name="north"></param>
        /// <param name="south"></param>
        /// <param name="latitude"></param>
        /// <returns></returns>
        private double GetVeritcalPosition(double north, double south, double latitude)
        {   
            var val = (latitude - south) / (north - south);
            return Math.Max(0, Math.Min(1, val));
        }
    }
}