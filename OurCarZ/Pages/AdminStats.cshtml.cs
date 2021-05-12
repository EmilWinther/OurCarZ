using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OurCarZ.Model;

namespace OurCarZ.Pages
{
    [Authorize(Roles = "admin")]
    public class AdminStatsModel : PageModel
    {

        EmilDbContext _edb = new EmilDbContext();
        
        public List<Route> RouteList = new List<Route>();
        public List<Numbers> NrList = new List<Numbers>();
        public Dictionary<int, string> AddressIdsCorrespondingRoadName = new Dictionary<int, string>();
        public AdminStatsModel(EmilDbContext edeebs) 
        {
            _edb = edeebs;
        }
        public string AddressIdsToName(Numbers id) 
        {
            string BothNames;
            if (AddressIdsCorrespondingRoadName.ContainsKey(id.Value1))
            {
                BothNames = AddressIdsCorrespondingRoadName[id.Value1];
            }
            else 
            {
                BothNames = "Who?";
            }
            BothNames += " - ";
            if (AddressIdsCorrespondingRoadName.ContainsKey(id.Value2))
            {
                BothNames += AddressIdsCorrespondingRoadName[id.Value2];
            }
            else
            {
                BothNames += "Who?";
            }
            return BothNames;
        }
        public Dictionary<Numbers, int> HowManySameRoutes()
        {
            
            Dictionary<Numbers, int> dict = new Dictionary<Numbers, int>();
            foreach (Route route in RouteList) 
            {
                Numbers StartAndFinish;
                StartAndFinish.Value1 = route.StartPoint;
                StartAndFinish.Value2 = route.FinishPoint;
                NrList.Add(StartAndFinish);
                if (!dict.ContainsKey(StartAndFinish)) 
                {
                    Numbers StartAndFinish2;
                    StartAndFinish2.Value2 = route.StartPoint;
                    StartAndFinish2.Value1 = route.FinishPoint;
                    if (!dict.ContainsKey(StartAndFinish2))
                    {
                        dict.Add(new Numbers { Value1 = route.StartPoint, Value2 = route.FinishPoint }, 1);
                    }
                    else
                    {
                        dict[StartAndFinish2] += 1;
                    }
                }
                else 
                {
                    dict[StartAndFinish] += 1;
                }
                
            }
            return dict;
        }
        public void OnGet()
        {
            RouteList = (from x in _edb.Routes select x).ToList();
            
            List<string> strList = (from y in _edb.Addresses select y.RoadName).ToList();

            List<int> intList = (from z in _edb.Addresses select z.AddressId).ToList();

            AddressIdsCorrespondingRoadName = (intList.Zip(strList, (k, v) => new { k, v }).ToDictionary(x => x.k, x => x.v));
            
        }
    }
    public struct Numbers
    {
        public int Value1;
        public int Value2;
        public override string ToString()
        {
            return "value 1 is " + Value1 + ", value 2 is " + Value2 + " ";
        }
    }
}
