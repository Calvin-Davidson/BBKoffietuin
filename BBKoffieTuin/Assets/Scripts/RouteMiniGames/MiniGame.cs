using System;
using System.Collections.Generic;
using enums;
using UnityEngine;

namespace RouteMiniGames
{
    public abstract class MiniGame : MonoBehaviour
    {
        [SerializeField] private MiniGameSceneManager sceneManager;
        private readonly Dictionary<MiniGameState, Action> _stateActions = new Dictionary<MiniGameState, Action>();

        private void Awake()
        {
            _stateActions.Add(MiniGameState.StartMenu, OnStartState);
            _stateActions.Add(MiniGameState.Starting, OnStartingState);
            _stateActions.Add(MiniGameState.Playing, OnPlayingState);
            _stateActions.Add(MiniGameState.Ending, OnEndingState);
            _stateActions.Add(MiniGameState.Ended, OnEndedState);
        }
        
        public void SetState(MiniGameState state)
        {
            _stateActions[state].Invoke();
        }

        //handle state logic
        protected abstract void OnStartState();

        protected abstract void OnStartingState();

        protected abstract void OnPlayingState();

        protected abstract void OnEndingState();

        protected abstract void OnEndedState();
    }
}