using System;
using UnityEngine;

namespace Toolbox.Utilities
{
    [CreateAssetMenu(menuName = "ScriptableObjects/PersistentEvent", order = 1)]
    public class PersistentEvent : ScriptableObject
    {
        private Action _action;

        public Action Action => _action;

        public void Invoke()
        {
            _action?.Invoke();
        }
    }
}
