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
            _stateActions.Add(MiniGameState.StartMenu, OnStartMenuState);
            _stateActions.Add(MiniGameState.Starting, OnStartingMenuState);
            _stateActions.Add(MiniGameState.Playing, OnPlayingMenuState);
            _stateActions.Add(MiniGameState.Ending, OnEndingMenuState);
            _stateActions.Add(MiniGameState.Ended, OnEndedMenuState);
        }
        
        public void SetState(MiniGameState state)
        {
            _stateActions[state].Invoke();
        }

        //handle state logic
        protected abstract void OnStartMenuState();

        protected abstract void OnStartingMenuState();

        protected abstract void OnPlayingMenuState();

        protected abstract void OnEndingMenuState();

        protected abstract void OnEndedMenuState();
    }
}