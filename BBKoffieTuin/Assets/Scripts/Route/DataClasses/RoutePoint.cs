using System.Collections.Generic;
using enums;
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
        public string PointName = "default";
        public Coordinates Coordinates;
        public  List<string> AudioPaths = new();
        public MiniGameOptions MiniGameOptions = MiniGameOptions.None;

        [JsonIgnore] public GameObject MarkerGameObject;
        [JsonIgnore] public Color32 ReachedColor = new Color32(100, 100, 100, 255);
        [JsonIgnore] public Color32 DefaultColor = new Color32(255, 255, 255, 255);
        
        [JsonIgnore] private bool _hasTriggered = false;
        [JsonIgnore] private bool _isTriggered = false;
        
        //events
        [JsonIgnore] public readonly UnityEvent onPointReached = new UnityEvent();


        [JsonIgnore]
        public bool HasTriggered
        {
            get => _hasTriggered;
            set
            {
                _hasTriggered = value;

                if(_hasTriggered) ChangeMarkerColor(ReachedColor);
                if(!_hasTriggered) ChangeMarkerColor(DefaultColor);
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
            if (MarkerGameObject is null) return;
            var image = MarkerGameObject.GetComponentInChildren<Image>();
            if (image is null) return;
            
            image.color = color;
        }
    }
}