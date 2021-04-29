using OurCarZ.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OurCarZ.Mock
{
    public class RouteMockData
    {
        private static List<Route> routes = new List<Route>()
        {
            new Route(1, "Ahornlunden 5, Roskilde, Dk","Maglegårdsvej 2, 4000 Roskilde, Dk"),
            new Route(2, "Århus","Maglegårdsvej 2, 4000 Roskilde, Dk"),
            new Route(3, "Roskilde, Dk","Maglegårdsvej 2, 4000 Roskilde, Dk"),
            new Route(4, "København","Maglegårdsvej 2, 4000 Roskilde, Dk")
        };

        public static List<Route> GetMockRoutes()
        {
            return routes;
        }
    }
}
