using System;
using Newtonsoft.Json;
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

        private RectTransform _userRect;

        private void OnEnable()
        {
            routeHandler.onRouteChanged.AddListener(InitializeMap);
            InitializeMap();
        }

        private void OnDisable()
        {
            routeHandler.onRouteChanged.RemoveListener(InitializeMap);
        }

        private void Update()
        {
            UpdateUserPosition();
            UpdateUserRotation();
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

            foreach (var point in routeHandler.ActiveRoute.PointsOfInterest)
            {
                //this gives us the 0 - 1 value of where the circle should be drawn.
                double x = GetHorizontalPosition(routeHandler.ActiveRoute.bounds.east, routeHandler.ActiveRoute.bounds.west, point.Coordinates.longitude);
                double y = GetVeritcalPosition(routeHandler.ActiveRoute.bounds.north, routeHandler.ActiveRoute.bounds.south, point.Coordinates.latitude);

                //instantiate markers
                var obj = Instantiate(markerPrefab, mapImage.transform, true);
                obj.GetComponentInChildren<TMP_Text>().text = point.pointName;
                var rect = obj.GetComponent<RectTransform>();
                rect.localScale = new Vector3(1,1,1);
                
                Vector2 newRectPosition = new Vector2();
                newRectPosition.x = (float)((width) * x) - width / 2;
                newRectPosition.y = (float)((height) * y) - height / 2;
                
                rect.localPosition = newRectPosition;
            }
        }

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
            var obj = Instantiate(userPrefab, mapImage.transform, true);
            _userRect = obj.GetComponent<RectTransform>();
            _userRect.localScale = new Vector3(1,1,1);
                
            Vector2 newRectPosition = new Vector2();
            newRectPosition.x = (float)((width) * x) - width / 2;
            newRectPosition.y = (float)((height) * y) - height / 2;
                
            _userRect.localPosition = newRectPosition;
        }

        public void UpdateUserPosition()
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

        public void UpdateUserRotation()
        {
            
        }

        public double GetHorizontalPosition(double east, double west, double longitude)
        {
            return (longitude - west) / (east - west);
        }
        
        public double GetVeritcalPosition(double north, double south, double latitude)
        {   
            return (latitude - south) / (north - south);
        }
    }
}