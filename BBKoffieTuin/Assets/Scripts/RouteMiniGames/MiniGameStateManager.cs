using System;
using enums;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace RouteMiniGames
{
    [Serializable]
    public class MiniGameStateManager : MonoBehaviour
    {
        [SerializeField] private MiniGameState currentState = MiniGameState.Starting;
        public readonly UnityEvent<MiniGameState> onStateChanged = new UnityEvent<MiniGameState>();

        public void GotoNextStage()
        {
            int currentStateIndex = (int) currentState;
            MiniGameState nextState = (MiniGameState) currentStateIndex + 1;
            currentState = nextState;
            onStateChanged.Invoke(nextState);
        }

        public MiniGameState CurrentState
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
