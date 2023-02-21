using Generic;
using Newtonsoft.Json;
using UnityEngine.Events;

namespace Route
{
    public class RoutePoint
    {
        public string pointName = "default";
        public Coordinates Coordinates;
        
        [JsonIgnore] public bool HasTriggered = false;
        [JsonIgnore] public bool isTriggered = false;
        [JsonIgnore] public UnityEvent onPointReached = new UnityEvent();
    }
}