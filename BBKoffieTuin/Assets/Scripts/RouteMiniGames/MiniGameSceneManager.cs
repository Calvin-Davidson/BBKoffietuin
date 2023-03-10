using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using enums;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

namespace RouteMiniGames
{
    
    [Serializable]
    public class MiniGameSceneManager : MonoBehaviour
    {
        public GameObject startMenu;
        public GameObject startingMenu;
        public GameObject playingMenu;
        public GameObject endingMenu;
        public GameObject endedMenu;
        
        Dictionary<MiniGameStates, GameObject> scenes = new Dictionary<MiniGameStates, GameObject>();

        public void Awake()
        {
            scenes.Add(MiniGameStates.StartMenu, startMenu);
            scenes.Add(MiniGameStates.Starting, startingMenu);
            scenes.Add(MiniGameStates.Playing, playingMenu);
            scenes.Add(MiniGameStates.Ending, endingMenu);
            scenes.Add(MiniGameStates.Ended, endedMenu);
        }
        
        public void DisableAllScenes()
        {
            foreach (var scene in scenes)
            {
                if(scene.Value == null) continue;
                scene.Value.SetActive(false);
            }
        }

        public void EnableScene(MiniGameStates state)
        {
            if (!scenes.ContainsKey(state)) return;
            GameObject scene = scenes[state];
            if(scene == null) return;
            DisableAllScenes();
            scene.SetActive(true);
        }
    }
}
