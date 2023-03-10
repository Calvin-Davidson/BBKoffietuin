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
        [SerializeField] private MiniGame _miniGame;
        [SerializeField] private MiniGameState currentState = MiniGameState.Starting;
        public readonly UnityEvent<MiniGameState> onStateChanged = new UnityEvent<MiniGameState>();

        private void Start()
        {
            _miniGame.SetState(currentState);
            onStateChanged.Invoke(currentState);
        }

        public void GotoNextStage()
        {
            int currentStateIndex = (int) currentState;
            MiniGameState nextState = (MiniGameState) currentStateIndex + 1;
            currentState = nextState;
            onStateChanged.Invoke(nextState);
            _miniGame.SetState(nextState);
        }

        public MiniGameState CurrentState
        {
            get => currentState;
            set
            {
                currentState = value;
                _miniGame.SetState(value);
                onStateChanged.Invoke(value);
            }
        }
    }
}
