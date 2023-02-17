using System;
using System.Collections.Generic;
using Generic;
using UnityEngine;

namespace Route
{
    public class Route
    {
        /// <summary>
        /// The name of the route.
        /// </summary>
        public string routeName = "";
        
        /// <summary>
        /// Base64Url of the image 'map' 
        /// </summary>
        public string base64Image;
        
        /// <summary>
        /// Holds the image sprite cache if it was created.
        /// </summary>
        private Sprite _imageSprite;

        /// <summary>
        /// Gets the sprite of the route or creates it from the base64 string.
        /// </summary>
        public Sprite ImageSprite
        {
            get
            {
                if (_imageSprite != null) return _imageSprite;
                if (string.IsNullOrEmpty(base64Image)) return null;

                //create it from the base64 string and cache it.
                byte[] imageBytes = Convert.FromBase64String(base64Image);
                Texture2D tex = new Texture2D(2, 2);
                tex.LoadImage(imageBytes);
                Sprite sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f),
                    100.0f);

                _imageSprite = sprite;
                return sprite;
            }
            set => _imageSprite = value;
        }


        /// <summary>
        /// Holds the image texture cache if it was created.
        /// </summary>
        private Texture _imageTexture;

        /// <summary>
        /// Gets the texture of the route or creates it from the base64 string.
        /// </summary>
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
        /// Points of interest are all points that are part of the path 
        /// </summary>
        public List<RoutePoint> PointsOfInterest = new List<RoutePoint>();

        /// <summary>
        /// PathPoints are points on the path that are 'corners' can be used to more easily draw the path.
        /// </summary>
        public List<Coordinates> PathPoints = new List<Coordinates>();

        /// <summary>
        /// Get the next point to reach in the route
        /// </summary>
        /// <returns></returns>
        public RoutePoint GetNextPointToReach()
        {
            return PointsOfInterest[GetNextPointToReachIndex()];
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
                
                if(point.HasTriggered == false) return index;
            }

            return -1;
        }
    }
}