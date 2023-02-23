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
        private GameObject _userGameObject;

        private void OnEnable()
        {
            routeHandler.onRouteChanged.AddListener(InitializeMap);
            PhoneDirection.Instance.onNortherPointChange.AddListener(UpdateUserRotation);
            InitializeMap();
        }

        private void OnDisable()
        {
            routeHandler.onRouteChanged.RemoveListener(InitializeMap);
            PhoneDirection.Instance.onNortherPointChange.RemoveListener(UpdateUserRotation);
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
                point._markerGameObject = obj;
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
            _userGameObject = Instantiate(userPrefab, mapImage.transform, true);
            _userRect = _userGameObject.GetComponent<RectTransform>();
            _userRect.localScale = new Vector3(1,1,1);
                
            Vector2 newRectPosition = new Vector2();
            newRectPosition.x = (float)((width) * x) - width / 2;
            newRectPosition.y = (float)((height) * y) - height / 2;
                
            _userRect.localPosition = newRectPosition;
        }

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

        private void UpdateUserRotation(float newRotation)
        {
            _userRect.rotation = Quaternion.Euler(new Vector3(0,0,newRotation));
        }

        private double GetHorizontalPosition(double east, double west, double longitude)
        {
            var val = (longitude - west) / (east - west);
            return Math.Max(0, Math.Min(1, val));
        }
        
        private double GetVeritcalPosition(double north, double south, double latitude)
        {   
            var val = (latitude - south) / (north - south);
            return Math.Max(0, Math.Min(1, val));
        }
    }
}