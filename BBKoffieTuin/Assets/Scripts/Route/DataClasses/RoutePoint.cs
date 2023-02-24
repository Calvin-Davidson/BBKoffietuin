using Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Route
{
    public class RoutePoint
    {
        //variables
        public string pointName = "default";
        public Coordinates Coordinates;
        
        [JsonIgnore] public GameObject _markerGameObject;
        [JsonIgnore] public Color32 _reachedColor = new Color32(100, 100, 100, 255);
        
        [JsonIgnore] private bool _hasTriggered = false;
        [JsonIgnore] private bool _isTriggered = false;
        
        //events
        [JsonIgnore] public UnityEvent onPointReached = new UnityEvent();


        [JsonIgnore]
        public bool HasTriggered
        {
            get => _hasTriggered;
            set
            {
                _hasTriggered = value;

                if(_hasTriggered) ChangeMarkerColor(_reachedColor);
            }
        }

        [JsonIgnore]
        public bool IsTriggered
        {
            get => _isTriggered;
            set => _isTriggered = value;
        }

        private void ChangeMarkerColor(Color32 color)
        {
            if (_markerGameObject is null) return;
            var image = _markerGameObject.GetComponentInChildren<Image>();
            if (image is null) return;
            
            image.color = color;
        }
    }
}