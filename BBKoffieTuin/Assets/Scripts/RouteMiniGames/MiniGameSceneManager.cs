using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using enums;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.Serialization;

namespace RouteMiniGames
{
    
    [Serializable]
    public class MiniGameSceneManager : MonoBehaviour
    {
        public GameObject StartMenu;
        public GameObject StartingMenu;
        public GameObject PlayingMenu;
        public GameObject EndingMenu;
        public GameObject EndedMenu;
        
        Dictionary<MiniGameStates, GameObject> scenes = new Dictionary<MiniGameStates, GameObject>();

        public void Awake()
        {
            scenes.Add(MiniGameStates.StartMenu, StartMenu);
            scenes.Add(MiniGameStates.Starting, StartingMenu);
            scenes.Add(MiniGameStates.Playing, PlayingMenu);
            scenes.Add(MiniGameStates.Ending, EndingMenu);
            scenes.Add(MiniGameStates.Ended, EndedMenu);
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
