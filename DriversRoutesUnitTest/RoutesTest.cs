using FluentAssertions;

using Xunit;

namespace DriversRoutesUnitTest
{
    public class RoutesTest
    {
        private readonly DriversRoutes.Data.GoogleApi.IRoutes _routes;
        private static DriversRoutes.Model.Route.ComputeRoutesRequest Request = new DriversRoutes.Model.Route.ComputeRoutesRequest()
        {
            Origin = new DriversRoutes.Model.Route.Waypoint()
            {
                Location = new DriversRoutes.Model.Route.Location()
                {
                    LatLng = new DriversRoutes.Model.Route.LatLng()
                    {
                        Latitude = 49.72208583461659,
                        Longitude = 20.404211995808193,
                    }
                }
            },
            Destination = new DriversRoutes.Model.Route.Waypoint()
            {
                Location = new DriversRoutes.Model.Route.Location()
                {
                    LatLng = new DriversRoutes.Model.Route.LatLng()
                    {
                        Latitude = 49.70659910495156,
                        Longitude = 20.421569710955264,
                    }
                }
            },
            TravelMode = DriversRoutes.Model.Route.RouteTravelMode.DRIVE,
            RoutingPreference = DriversRoutes.Model.Route.RoutingPreference.TRAFFIC_AWARE,
            ComputeAlternativeRoutes = false,
            RouteModifiers = new DriversRoutes.Model.Route.RouteModifiers()
            {
                AvoidTolls = false,
                AvoidHighways = false,
                AvoidFerries = false,
            },
            LanguageCode = "pl-pl",
            Units = DriversRoutes.Model.Route.Units.METRIC,
        };

        public RoutesTest()
        {
            _routes = new DriversRoutes.Data.GoogleApi.Routes(new HttpClient(), "AIzaSyDMfTC47bnsNBAK8S4xKk7Mhb_aiSqnCYU");
        }
        [Fact]
        public async Task GetOnlyRouteStepsDurationDistanceTest()
        {
            var response = new DriversRoutes.Model.Route.Response()
            {
                Routes =
                [
                    new DriversRoutes.Model.Route.Route()
                    {
                        DistanceMeters=2705,
                        Duration="0s"
                    },
                ]
            };

            var obj = await _routes.GetOnlyDistanceAndDuration(Request);
            obj.Routes[0].Duration = "0s";
            var objJson = System.Text.Json.JsonSerializer.Serialize(obj);
            var responseJson = System.Text.Json.JsonSerializer.Serialize(response);

            objJson.Should().Be(responseJson);
        }



        [Fact]
        public async Task GetOnlyRouteStepsDurationDistance()
        {
            var response = new DriversRoutes.Model.Route.Response()
            {
                Routes =
                [
                    new DriversRoutes.Model.Route.Route()
                    {
                        Legs =
                        [
                            new DriversRoutes.Model.Route.RouteLeg()
                            {
                                Steps=
                                [
                                    new DriversRoutes .Model.Route.RouteLegStep()
                                    {
                                        DistanceMeters=6,
                                        StaticDuration="1s",
                                        Polyline=new DriversRoutes.Model.Route.Polyline
                                        {
                                            EncodedPolyline="{innHiep{BAQ",
                                        },
                                        StartLocation=new DriversRoutes.Model.Route.Location()
                                        {
                                            LatLng=new DriversRoutes.Model.Route.LatLng()
                                            {
                                                Latitude=49.722062699999995,
                                                Longitude=20.404209599999998,
                                            }
                                        },
                                        EndLocation=new DriversRoutes.Model.Route.Location()
                                        {
                                            LatLng=new DriversRoutes.Model.Route.LatLng()
                                            {
                                                Latitude=49.7220696,
                                                Longitude=20.4042975,
                                            }
                                        },
                                        NavigationInstruction=new DriversRoutes.Model.Route.NavigationInstruction()
                                        {
                                            Maneuver= DriversRoutes.Model.Route.Maneuver.DEPART,
                                            Instructions= "Kieruj się na wschód w stronę Lipowa",
                                        },
                                        LocalizedValues=new DriversRoutes.Model.Route.RouteLegStepLocalizedValues()
                                        {
                                            Distance=new DriversRoutes.Model.Route.LocalizedText()
                                            {
                                                Text="6 m",
                                            },
                                            StaticDuration=new DriversRoutes.Model.Route.LocalizedText()
                                            {
                                                Text="1 min",
                                            },                                           
                                        },
                                        TravelMode=DriversRoutes.Model.Route.RouteTravelMode.DRIVE,
                                    },
                                    new DriversRoutes .Model.Route.RouteLegStep()
                                    {
                                        DistanceMeters=491,
                                        StaticDuration="49s",
                                        Polyline=new DriversRoutes.Model.Route.Polyline
                                        {
                                            EncodedPolyline="}innH{ep{BCIJo@bImQ|EsK",
                                        },
                                        StartLocation=new DriversRoutes.Model.Route.Location()
                                        {
                                            LatLng=new DriversRoutes.Model.Route.LatLng()
                                            {
                                                Latitude=49.7220696,
                                                Longitude=20.4042975,
                                            }
                                        },
                                        EndLocation=new DriversRoutes.Model.Route.Location()
                                        {
                                            LatLng=new DriversRoutes.Model.Route.LatLng()
                                            {
                                                Latitude=49.719302,
                                                Longitude=20.409561,
                                            }
                                        },
                                        NavigationInstruction=new DriversRoutes.Model.Route.NavigationInstruction()
                                        {
                                            Maneuver= DriversRoutes.Model.Route.Maneuver.ROUNDABOUT_RIGHT,
                                            Instructions= "Zjedź z ronda w Piłsudskiego/DK28",
                                        },
                                        LocalizedValues=new DriversRoutes.Model.Route.RouteLegStepLocalizedValues()
                                        {
                                            Distance=new DriversRoutes.Model.Route.LocalizedText()
                                            {
                                                Text="0,5 km",
                                            },
                                            StaticDuration=new DriversRoutes.Model.Route.LocalizedText()
                                            {
                                                Text="1 min",
                                            },
                                        },
                                        TravelMode=DriversRoutes.Model.Route.RouteTravelMode.DRIVE,
                                    },
                                    new DriversRoutes .Model.Route.RouteLegStep()
                                    {
                                        DistanceMeters=1478,
                                        StaticDuration="139s",
                                        Polyline=new DriversRoutes.Model.Route.Polyline
                                        {
                                            EncodedPolyline="sxmnHwfq{BzGcOlD_J|I{TdBkEz@yB|AqD~@sAxK_Ml@c@h@WdBe@l@IhAUd@?",
                                        },
                                        StartLocation=new DriversRoutes.Model.Route.Location()
                                        {
                                            LatLng=new DriversRoutes.Model.Route.LatLng()
                                            {
                                                Latitude=49.719302,
                                                Longitude=20.409561,
                                            }
                                        },
                                        EndLocation=new DriversRoutes.Model.Route.Location()
                                        {
                                            LatLng=new DriversRoutes.Model.Route.LatLng()
                                            {
                                                Latitude=49.709871899999996,
                                                Longitude=20.423231599999998,
                                            }
                                        },
                                        NavigationInstruction=new DriversRoutes.Model.Route.NavigationInstruction()
                                        {
                                            Maneuver= DriversRoutes.Model.Route.Maneuver.STRAIGHT,
                                            Instructions= "Dalej prosto, pozostając na Piłsudskiego/DK28",
                                        },
                                        LocalizedValues=new DriversRoutes.Model.Route.RouteLegStepLocalizedValues()
                                        {
                                            Distance=new DriversRoutes.Model.Route.LocalizedText()
                                            {
                                                Text="1,5 km",
                                            },
                                            StaticDuration=new DriversRoutes.Model.Route.LocalizedText()
                                            {
                                                Text="2 min",
                                            },
                                        },
                                        TravelMode=DriversRoutes.Model.Route.RouteTravelMode.DRIVE,
                                    },
                                    new DriversRoutes .Model.Route.RouteLegStep()
                                    {
                                        DistanceMeters=467,
                                        StaticDuration="62s",
                                        Polyline=new DriversRoutes.Model.Route.Polyline
                                        {
                                            EncodedPolyline="u}knHe|s{BRFBA^b@pDvG|@dBr@jAVPP@nFmAVMh@K",
                                        },
                                        StartLocation=new DriversRoutes.Model.Route.Location()
                                        {
                                            LatLng=new DriversRoutes.Model.Route.LatLng()
                                            {
                                                Latitude=49.709871899999996,
                                                Longitude=20.423231599999998,
                                            }
                                        },
                                        EndLocation=new DriversRoutes.Model.Route.Location()
                                        {
                                            LatLng=new DriversRoutes.Model.Route.LatLng()
                                            {
                                                Latitude=49.706387299999996,
                                                Longitude=20.4211469,
                                            }
                                        },
                                        NavigationInstruction=new DriversRoutes.Model.Route.NavigationInstruction()
                                        {
                                            Maneuver= DriversRoutes.Model.Route.Maneuver.ROUNDABOUT_RIGHT,
                                            Instructions= "Na rondzie pierwszy zjazd w Jana Pawła II/DK28",
                                        },
                                        LocalizedValues=new DriversRoutes.Model.Route.RouteLegStepLocalizedValues()
                                        {
                                            Distance=new DriversRoutes.Model.Route.LocalizedText()
                                            {
                                                Text="0,5 km",
                                            },
                                            StaticDuration=new DriversRoutes.Model.Route.LocalizedText()
                                            {
                                                Text="1 min",
                                            },
                                        },
                                        TravelMode=DriversRoutes.Model.Route.RouteTravelMode.DRIVE,
                                    },
                                    new DriversRoutes .Model.Route.RouteLegStep()
                                    {
                                        DistanceMeters=157,
                                        StaticDuration="27s",
                                        Polyline=new DriversRoutes.Model.Route.Polyline
                                        {
                                            EncodedPolyline="}gknHeos{BVRTFf@SLSPwCGk@Qa@KI",
                                        },
                                        StartLocation=new DriversRoutes.Model.Route.Location()
                                        {
                                            LatLng=new DriversRoutes.Model.Route.LatLng()
                                            {
                                                Latitude=49.706387299999996,
                                                Longitude=20.4211469,
                                            }
                                        },
                                        EndLocation=new DriversRoutes.Model.Route.Location()
                                        {
                                            LatLng=new DriversRoutes.Model.Route.LatLng()
                                            {
                                                Latitude=49.705993799999995,
                                                Longitude=20.422406199999998,
                                            }
                                        },
                                        NavigationInstruction=new DriversRoutes.Model.Route.NavigationInstruction()
                                        {
                                            Maneuver= DriversRoutes.Model.Route.Maneuver.ROUNDABOUT_RIGHT,
                                            Instructions= "Na Rynek zjedź w Kościuszki/DK28",
                                        },
                                        LocalizedValues=new DriversRoutes.Model.Route.RouteLegStepLocalizedValues()
                                        {
                                            Distance=new DriversRoutes.Model.Route.LocalizedText()
                                            {
                                                Text="0,2 km",
                                            },
                                            StaticDuration=new DriversRoutes.Model.Route.LocalizedText()
                                            {
                                                Text="1 min",
                                            },
                                        },
                                        TravelMode=DriversRoutes.Model.Route.RouteTravelMode.DRIVE,
                                    },
                                    new DriversRoutes .Model.Route.RouteLegStep()
                                    {
                                        DistanceMeters=105,
                                        StaticDuration="26s",
                                        Polyline=new DriversRoutes.Model.Route.Polyline
                                        {
                                            EncodedPolyline="meknHaws{Bs@KY^Md@IpB",
                                        },
                                        StartLocation=new DriversRoutes.Model.Route.Location()
                                        {
                                            LatLng=new DriversRoutes.Model.Route.LatLng()
                                            {
                                                Latitude=49.705993799999995,
                                                Longitude=20.422406199999998,
                                            }
                                        },
                                        EndLocation=new DriversRoutes.Model.Route.Location()
                                        {
                                            LatLng=new DriversRoutes.Model.Route.LatLng()
                                            {
                                                Latitude= 49.706500000000005,
                                                Longitude=20.421552000000002,
                                            }
                                        },
                                        LocalizedValues=new DriversRoutes.Model.Route.RouteLegStepLocalizedValues()
                                        {
                                            Distance=new DriversRoutes.Model.Route.LocalizedText()
                                            {
                                                Text="0,1 km",
                                            },
                                            StaticDuration=new DriversRoutes.Model.Route.LocalizedText()
                                            {
                                                Text="1 min",
                                            },
                                        },
                                        TravelMode=DriversRoutes.Model.Route.RouteTravelMode.DRIVE,
                                    },
                                ]
                            }
                        ],
                        DistanceMeters=2704,
                        Duration="0s"
                    },
                ]
            };

            var obj = await _routes.GetOnlyRouteStepsDurationDistance(Request);
            obj.Routes[0].Duration = "0s";
            var objJson = System.Text.Json.JsonSerializer.Serialize(obj);
            var responseJson = System.Text.Json.JsonSerializer.Serialize(response);

            objJson.Should().Be(responseJson);
        }


    }
}
