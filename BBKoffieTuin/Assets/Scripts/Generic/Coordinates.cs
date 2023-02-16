using System;
using UnityEngine;

namespace Generic
{
    public class Coordinates
    {
        public double latitude;
        public double longitude;
        public double altitude;
        
        /// <summary>
        /// 
        /// </summary>
        public Coordinates(){}
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <param name="altitude"></param>
        public Coordinates(float latitude, float longitude, float altitude)
        {
            this.latitude = latitude;
            this.longitude = longitude;
            this.altitude = altitude;
        }
        
        //make a function to calculate distance to another coordinates
        public double DistanceTo(double toLat, double toLon, char unit = 'K')
        {
            double rlat1 = Math.PI*latitude/180;
            double rlat2 = Math.PI*toLat/180;
            double theta = longitude - toLon;
            double rtheta = Math.PI*theta/180;
            double dist =
                Math.Sin(rlat1)*Math.Sin(rlat2) + Math.Cos(rlat1)*
                Math.Cos(rlat2)*Math.Cos(rtheta);
            dist = Math.Acos(dist);
            dist = dist*180/Math.PI;
            dist = dist*60*1.1515;

            switch (unit)
            {
                case 'K': //Kilometers -> default
                    return dist*1.609344;
                case 'N': //Nautical Miles 
                    return dist*0.8684;
                case 'M': //Miles
                    return dist;
            }

            return dist;
        }
    }
}