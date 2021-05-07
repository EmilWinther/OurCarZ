using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OurCarZ.Model;

namespace OurCarZ.Pages
{
    public class AdminStatsModel : PageModel
    {
        EmilDbContext _edb = new EmilDbContext();
        public List<Route> RouteList = new List<Route>();
        public Dictionary<int, string> AddressIdToRoadName = new Dictionary<int, string>();
        public void OnGet(EmilDbContext edb)
        {
            _edb = edb;

            RouteList = (from x in _edb.Routes select x).ToList();

            AddressIdToRoadName = (from x in _edb.Addresses select x.AddressId).ToList().Zip((from y in _edb.Addresses select y.RoadName).ToList(), (k, v) => new { k, v }).ToDictionary(x => x.k, x => x.v);

        }
    }
}
