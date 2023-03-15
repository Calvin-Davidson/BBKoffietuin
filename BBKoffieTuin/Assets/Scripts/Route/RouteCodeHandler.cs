using System;
using System.Collections.Generic;
using Renderers;
using Toolbox.Utilities;
using UnityEngine;
using UnityEngine.Events;

namespace Route
{
    public class RouteCodeHandler : MonoBehaviour
    {
        public PersistentEvent miniGameCompleteEvent;

        private char[] _unlockedCode = new char[4];
        public UnityEvent<char, int> onCodeChange = new();
        
        private void Awake()
        {
            RouteHandler.Instance.onRouteChanged.AddListener(ResetCodes);
            RouteHandler.Instance.onRouteChanged.AddListener(InitializeCodes);

            miniGameCompleteEvent.Action += HandleMiniGameComplete;
        }

        private void OnDestroy()
        {
            miniGameCompleteEvent.Action -= HandleMiniGameComplete;
        }

        private void ResetCodes()
        {
            _unlockedCode = new char[]
            {
                '-', '-', '-', '-',
            };
        }

        private void InitializeCodes()
        {
            _unlockedCode = new char[]
            {
                '-', '-', '-', '-',
            };
        }

        private void HandleMiniGameComplete()
        {
            for (int i = 0; i < _unlockedCode.Length; i++)
            {
                if (_unlockedCode[i] != '-') continue;
                _unlockedCode[i] = RouteHandler.Instance.ActiveRoute.RouteCode[i];
                onCodeChange?.Invoke(_unlockedCode[i], i);
                return;
            }
        }
    }
}
