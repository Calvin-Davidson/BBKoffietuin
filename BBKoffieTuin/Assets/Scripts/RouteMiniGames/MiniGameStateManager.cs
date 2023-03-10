using System;
using enums;
using UnityEngine;
using UnityEngine.Events;

namespace RouteMiniGames
{
    [Serializable]
    public class MiniGameStateManager : MonoBehaviour
    {
        [SerializeField] private MiniGameStates currentState = MiniGameStates.Starting;

        public readonly UnityEvent<MiniGameStates> onStateChanged = new UnityEvent<MiniGameStates>();
        
        
        public void GotoNextStage()
        {
            int currentStateIndex = (int) currentState;
            MiniGameStates nextState = (MiniGameStates) currentStateIndex + 1;
            currentState = nextState;
            onStateChanged.Invoke(nextState);
        }

        public MiniGameStates CurrentState
        {
            get => currentState;
            set
            {
                currentState = value;
                onStateChanged.Invoke(value);
            }
        }
    }
}
