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
        [SerializeField] private MiniGame miniGame;
        [SerializeField] private MiniGameState currentState = MiniGameState.Starting;
        public readonly UnityEvent<MiniGameState> onStateChanged = new UnityEvent<MiniGameState>();

        private void Start()
        {
            miniGame.SetState(currentState);
            onStateChanged.Invoke(currentState);
        }

        public void GotoNextStage()
        {
            int currentStateIndex = (int) currentState;
            MiniGameState nextState = (MiniGameState) currentStateIndex + 1;
            CurrentState = nextState;
        }

        public MiniGameState CurrentState
        {
            get => currentState;
            set
            {
                if (currentState == value) return;
                currentState = value;
                miniGame.SetState(value);
                onStateChanged.Invoke(value);
            }
        }
    }
}
