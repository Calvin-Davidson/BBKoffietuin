using System.Collections.Generic;
using UnityEngine;

namespace Route
{
    public class RouteCodeHandler : MonoBehaviour
    {
        public List<GameObject> codeObjects = new List<GameObject>();

        private void Awake()
        {
            RouteHandler.Instance.onRouteChanged.AddListener(ResetCodes);
            RouteHandler.Instance.onRouteChanged.AddListener(InitializeCodes);
            
            //get event scriptable from resources Assets/Resources/PersistentEvents/MinigameCompleteEvent.asset
            WTF_IS_THIS_TYPE minigameCompleteEvent = Resources.Load<WTF_IS_THIS_TYPE>("PersistentEvents/MinigameCompleteEvent");
        }

        private void ResetCodes()
        {
            
        }

        private void InitializeCodes()
        {
            
        }
    }
}
