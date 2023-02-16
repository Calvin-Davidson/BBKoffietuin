﻿using System;
using TMPro;
using UnityEngine;

namespace Route
{
    /// <summary>
    /// Route debug handler is a temporary class that will be used to debug the route system.
    /// </summary>
    public class RouteDebugHandler : MonoBehaviour
    {
        [SerializeField] private RouteHandler routeHandler;
        public TMP_Text debugText;

        private void Awake()
        {
            routeHandler.onNextPointReached.AddListener((point, index) =>
            {
                debugText.text = "You reached next point: " + point.pointName + " at index: " + index;
            });
            
            routeHandler.onFurtherPointReached.AddListener((point, index) =>
            {
                debugText.text = "You reached further point: " + point.pointName + " at index: " + index;
            });
            
            routeHandler.onAlreadyReachedPointReached.AddListener((point, index) =>
            {
                debugText.text = "already reached point : " + point.pointName + " at index: " + index;
            });
        }
        
    }
}