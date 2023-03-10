using System;
using System.Collections.Generic;
using enums;
using UnityEngine;

namespace RouteMiniGames
{
    public abstract class MiniGame : MonoBehaviour
    {
        [SerializeField] private MiniGameStateManager stateManager;
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

        private void Start()
        {
            stateManager.CurrentState = MiniGameState.StartMenu;
        }

        //enable the right scene.
        private void OnEnable()
        {
            sceneManager.EnableScene(stateManager.CurrentState);
            stateManager.onStateChanged.AddListener(sceneManager.EnableScene);
            stateManager.onStateChanged.AddListener((state) => _stateActions[state].Invoke());
        }

        private void OnDisable()
        {
            stateManager.onStateChanged.RemoveListener(sceneManager.EnableScene);
        }
        
        //handle state logic
        protected abstract void OnStartMenuState();

        protected abstract void OnStartingMenuState();

        protected abstract void OnPlayingMenuState();

        protected abstract void OnEndingMenuState();

        protected abstract void OnEndedMenuState();
    }
}