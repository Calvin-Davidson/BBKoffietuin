using System;
using System.Collections.Generic;
using System.Linq;
using enums;
using Generic;
using Newtonsoft.Json;
using Toolbox.MethodExtensions;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

namespace Route
{
    public class Route
    {
        public int CodeSize = 4;
        public List<char> RouteCode = new List<char>();

        public string routeName = "";
        public string base64Image;
        public MapBounds bounds;

        public List<Coordinates> PathPoints = new List<Coordinates>();
        public List<RoutePoint> PointsOfInterest = new List<RoutePoint>();

        [JsonIgnore] private Sprite _imageSprite;
        [JsonIgnore] private Texture _imageTexture;
        [JsonIgnore] private readonly Dictionary<string, List<char>> _miniGamesWithCode = new Dictionary<string, List<char>>();

        public Dictionary<string, List<char>> MiniGamesWithCode => _miniGamesWithCode;

        /// <summary>
        /// Get the next point to reach in the route
        /// </summary>
        /// <returns></returns>
        public RoutePoint GetNextPointToReach()
        {
            var index = GetNextPointToReachIndex();
            var contains = PointsOfInterest.ContainsSlot(index);
            return (contains) ? PointsOfInterest[index] : null;
        }

        public bool HasNextPointToReach()
        {
            var point = GetNextPointToReach();
            return point != null;
        }

        /// <summary>
        /// Get the next point to reach in the route as index of the array.
        /// </summary>
        /// <returns></returns>
        public int GetNextPointToReachIndex()
        {
            int index = -1;
            foreach (var point in PointsOfInterest)
            {
                index++;
                if (point.HasTriggered == false) return index;
            }

            return PointsOfInterest.Count - 1;
        }

        public RoutePoint GetFinalPoint()
        {
            return PointsOfInterest[^1];
        }

        public int GetFinalPointIndex()
        {
            return PointsOfInterest.Count - 1;
        }

        public void InitializeRoute()
        {
            //if the route code is smaller then the code size, fill it with random characters
            if (RouteCode.Count < CodeSize)
            {
                while (RouteCode.Count < CodeSize)
                {
                    var random = UnityEngine.Random.Range(0, 10);
                    RouteCode.Add(random.ToString()[0]);
                }
            }
            
            
            //if the route code is to long make it shorter
            if (RouteCode.Count > CodeSize)
            {
                List<char> newCode = new List<char>();
                for (int i = 0; i < CodeSize; i++)
                {
                    newCode[i] = RouteCode[i];
                }

                RouteCode = newCode;
            }

            List<RoutePoint> pointsWithGame = PointsOfInterest.Where(x => x.MiniGameOption != MiniGameOption.None).ToList();
            if(pointsWithGame.Count == 0)
            {
                Debug.LogWarning("This route has no points with a mini-game");
                return;
            }
            
            //create a list called chosenGames this should be a list with 4 RoutePoints that are also in pointsWithGame with no duplicates
            List<RoutePoint> chosenGames = pointsWithGame.GetRandomItems(pointsWithGame.Count > CodeSize ? CodeSize : pointsWithGame.Count);
            if (pointsWithGame.Count >= CodeSize)
            {
                for (int i = 0; i < CodeSize; i++)
                {
                    _miniGamesWithCode.Add(chosenGames[i].MiniGameOption.ToString(), new List<char>() { RouteCode[i] });
                }
            }
            
            //if we have less games then codeSize we need to add multiple codes to miniGamesWithCode
            if (pointsWithGame.Count < CodeSize)
            {
                //initial fill of the dictionary
                for (int i = 0; i < chosenGames.Count; i++)
                {
                    _miniGamesWithCode.Add(chosenGames[i].PointName, new List<char>() { RouteCode[i] });
                }
                
                //for all codes that are not used yet we add them to the list of each game starting by the first.
                int codesLeftCount = CodeSize - chosenGames.Count;
                int index = 0;
                for (int i = 0; i < codesLeftCount; i++)
                {
                    if(index >= chosenGames.Count) index = 0;
                    _miniGamesWithCode[chosenGames[index].PointName].Add(RouteCode[i + (CodeSize - codesLeftCount)]);
                    index++;
                }
            }

        }

        /// <summary>
        /// Gets the texture of the route or creates it from the base64 string.
        /// </summary>
        [JsonIgnore]
        public Texture ImageTexture
        {
            get
            {
                if (_imageTexture != null) return _imageTexture;
                if (string.IsNullOrEmpty(base64Image)) return null;

                //create it from the base64 string and cache it.
                byte[] imageBytes = Convert.FromBase64String(base64Image);
                Texture2D tex = new Texture2D(2, 2);
                tex.LoadImage(imageBytes);

                return tex;
            }
            set => _imageTexture = value;
        }

        /// <summary>
        /// Gets the texture and converts it to a sprite.
        /// </summary>
        [JsonIgnore]
        public Sprite ImageSprite
        {
            get
            {
                if (_imageSprite != null) return _imageSprite;

                //create it from the base64 string and cache it.
                Texture2D tex = (Texture2D)ImageTexture;
                if (tex == null) return null;
                Sprite sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f),
                    100.0f);
                _imageSprite = sprite;
                return sprite;
            }
            set => _imageSprite = value;
        }
    }
}