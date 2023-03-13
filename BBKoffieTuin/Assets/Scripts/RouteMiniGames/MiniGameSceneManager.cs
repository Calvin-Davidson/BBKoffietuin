using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using enums;
using TMPro;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.Serialization;

namespace RouteMiniGames
{
    
    [Serializable]
    public class MiniGameSceneManager : MonoBehaviour
    {
        [SerializeField] private MiniGameStateManager miniGameStateManager;
        public GameObject StartMenu;
        public GameObject StartingMenu;
        public GameObject PlayingMenu;
        public GameObject EndingMenu;
        public GameObject EndedMenu;
        
        Dictionary<MiniGameState, GameObject> scenes = new Dictionary<MiniGameState, GameObject>();

        public void Awake()
        {
            scenes.Add(MiniGameState.StartMenu, StartMenu);
            scenes.Add(MiniGameState.Starting, StartingMenu);
            scenes.Add(MiniGameState.Playing, PlayingMenu);
            scenes.Add(MiniGameState.Ending, EndingMenu);
            scenes.Add(MiniGameState.Ended, EndedMenu);
            
            miniGameStateManager.onStateChanged?.AddListener((state) =>
            {
                DisableAllScenes();
                EnableScene(state);
            });
        }
        
        public void DisableAllScenes()
        {
            foreach (var scene in scenes)
            {
                if(scene.Value == null) continue;
                scene.Value.SetActive(false);
            }
        }

        public void EnableScene(MiniGameState state)
        {
            if (!scenes.ContainsKey(state)) return;
            GameObject scene = scenes[state];
            if(scene == null) return;
            DisableAllScenes();
            scene.SetActive(true);
        }
    }
}
