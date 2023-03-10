using System.Collections.Generic;
using enums;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "Resources/Data", menuName = "ScriptableObjects/MiniGames", order = 1)]
    public class MiniGamesReferences : ScriptableObject
    {
        [System.Serializable]
        public struct MiniGameData
        {
            public MiniGameOptions type;
            public GameObject obj;
        }
        
        public List<MiniGameData> miniGames = new List<MiniGameData>();
    }
}