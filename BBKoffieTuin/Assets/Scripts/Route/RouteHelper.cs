using System;
using System.Collections.Generic;
using Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Route
{
    public class RouteHelper
    {
        /// <summary>
        /// Create a route from a json string.
        /// </summary>
        public static Route CreateRouteFromJson(string json)
        {
            try
            {
                return JsonConvert.DeserializeObject<Route>(json);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        /// <summary>
        /// Creates json from route object using json.net
        /// </summary>
        public static string CreateJsonFromRoute(Route route)
        {
            try
            {
                return JsonConvert.SerializeObject(route);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        /// <summary>
        /// Debug route for testing purposes.
        /// </summary>
        /// <returns></returns>
        public static Route GetDebugRoute()
        {
            //load json from resources folder
            string json = Resources.Load<TextAsset>("Data/mapData").text;
            JObject jObject = JObject.Parse(json);
            string base64Image = jObject["base64Url"].Value<string>();

            var bounds = new MapBounds()
            {
                north = jObject["bounds"]["north"].Value<double>(),
                south = jObject["bounds"]["south"].Value<double>(),
                east = jObject["bounds"]["east"].Value<double>(),
                west = jObject["bounds"]["west"].Value<double>()
            };

            return new Route()
            {
                base64Image = base64Image,
                bounds = bounds,
                routeName = "debugRouteName",
                PointsOfInterest = new List<RoutePoint>()
                {
                    new RoutePoint()
                    {
                      PointName  = "A",
                      Coordinates = new Coordinates()
                      {
                          latitude = 52.390652,
                          longitude = 4.856711
                      }
                    },
                    new RoutePoint()
                    {
                        PointName  = "B",
                        Coordinates = new Coordinates()
                        {
                            latitude = 52.390283,
                            longitude = 4.855105
                        }
                    },
                    new RoutePoint()
                    {
                        PointName  = "C",
                        Coordinates = new Coordinates()
                        {
                            latitude = 52.390256,
                            longitude = 4.853196
                        }
                    },
                    new RoutePoint()
                    {
                        PointName  = "D",
                        Coordinates = new Coordinates()
                        {
                            latitude = 52.389640,
                            longitude = 4.851188
                        }
                    },
                    new RoutePoint()
                    {
                        PointName  = "E",
                        Coordinates = new Coordinates()
                        {
                            latitude = 52.389582,
                            longitude = 4.849565
                        }
                    },
                    new RoutePoint()
                    {
                        PointName  = "F",
                        Coordinates = new Coordinates()
                        {
                            latitude = 52.389521,
                            longitude = 4.846371
                        }
                    },
                    new RoutePoint()
                    {
                        PointName  = "G",
                        Coordinates = new Coordinates()
                        {
                            latitude = 52.389477,
                            longitude = 4.841773
                        }
                    },
                    new RoutePoint()
                    {
                        PointName  = "H",
                        Coordinates = new Coordinates()
                        {
                            latitude = 52.389513,
                            longitude = 4.839421
                        }
                    },
                    new RoutePoint()
                    {
                        PointName  = "I",
                        Coordinates = new Coordinates()
                        {
                            latitude = 52.389162,
                            longitude = 4.838407
                        }
                    },
                }
            };
        }
    }
}