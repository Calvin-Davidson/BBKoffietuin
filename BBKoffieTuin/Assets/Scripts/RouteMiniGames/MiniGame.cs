using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using enums;
using Toolbox.Attributes;
using UnityEngine;

namespace RouteMiniGames
{
    public class MiniGame : MonoBehaviour
    {
        [SerializeField] private MiniGameStateManager stateManager;
        [SerializeField] private MiniGameSceneManager sceneManager;
        private readonly Dictionary<MiniGameStates, Action> _stateActions = new Dictionary<MiniGameStates, Action>();

        private void Awake()
        {
            _stateActions.Add(MiniGameStates.StartMenu, OnStartMenuState);
            _stateActions.Add(MiniGameStates.Starting, OnStartingMenuState);
            _stateActions.Add(MiniGameStates.Playing, OnPlayingMenuState);
            _stateActions.Add(MiniGameStates.Ending, OnEndingMenuState);
            _stateActions.Add(MiniGameStates.Ended, OnEndedMenuState);
        }

        private void Start()
        {
            stateManager.CurrentState = MiniGameStates.StartMenu;
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
        public void OnStartMenuState()
        {
        }
        
        public void OnStartingMenuState()
        {
        }
        
        public void OnPlayingMenuState()
        {
        }

        public void OnEndingMenuState()
        {
        }

        public void OnEndedMenuState()
        {
            
            //when the game is ended we can remove the mini game from the scene.
            Destroy(this.gameObject);
        }
    }
}