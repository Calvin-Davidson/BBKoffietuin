using System;
using System.Collections.Generic;
using UnityEngine;

namespace Route
{
    public class RouteCodeHandler : MonoBehaviour
    {
        public List<GameObject> CodeObjects = new List<GameObject>();

        private void Awake()
        {
            RouteHandler.Instance.onRouteChanged.AddListener(ResetCodes);
            RouteHandler.Instance.onRouteChanged.AddListener(InitializeCodes);
        }

        private void ResetCodes()
        {
            
        }

        private void InitializeCodes()
        {
            
        }
    }
}
