using System.Collections.Generic;
using Generic;

namespace Route
{
    public class RouteHelper
    {
        /// <summary>
        /// Create a route from a json string.
        /// </summary>
        public static void CreateRouteFromJson()
        {
            //todo: implement this.
        }
        
        /// <summary>
        /// Creates json from route object using json.net
        /// </summary>
        public static void CreateJsonFromRoute()
        {
            //todo: implement this.
        }

        /// <summary>
        /// Debug route for testing purposes.
        /// </summary>
        /// <returns></returns>
        public static Route GetDebugRoute()
        {
            return new Route()
            {
                routeName = "debugRouteName",
                PointsOfInterest = new List<RoutePoint>()
                {
                    new RoutePoint()
                    {
                      pointName  = "A",
                      Coordinates = new Coordinates()
                      {
                          latitude = 52.39152,
                          longitude = 4.857674
                      }
                    },
                    new RoutePoint()
                    {
                        pointName  = "B",
                        Coordinates = new Coordinates()
                        {
                            latitude = 52.3913,
                            longitude = 4.857858
                        }
                    },
                    new RoutePoint()
                    {
                        pointName  = "C",
                        Coordinates = new Coordinates()
                        {
                            latitude = 52.39152,
                            longitude = 4.857677
                        }
                    },
                }
            };
        }
    }
}