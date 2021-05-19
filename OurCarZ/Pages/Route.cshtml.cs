using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Routing;
using OurCarZ.Model;

namespace OurCarZ.Pages
{
    public class RouteModel : PageModel
    {
        private EmilDbContext DB;
        [BindProperty]
        public Model.Route route { get; set; }
        [BindProperty]
        public string StartPoint { get; set; }
        [BindProperty]
        public string FinishPoint { get; set; }
        public List<string> ExtraStops { get; set; }
        public List<int?> XtraStopsDupe { get; set; }
        public RouteModel(EmilDbContext edb) 
        {
            DB = edb;
        }
        public string CombineZipAndRoad(int? id) 
        {
            string returner;
            returner = DB.Addresses.Find(id).RoadName + " " + DB.Addresses.Find(id).ZipCode;
            return returner;
        }
        public void ExtraWaypoints(int router) 
        {
            List<int?> XtraStops;
            XtraStops = DB.UserRoutes.Where(x => x.RouteId == router)
                .Select(x => x.Via).ToList();
            XtraStopsDupe = XtraStops.Distinct().ToList();
            if (XtraStopsDupe.Contains(route.StartPoint)) 
            {
                XtraStopsDupe.Remove(route.StartPoint);
            }
            if (XtraStopsDupe.Contains(route.FinishPoint))
            {
                XtraStopsDupe.Remove(route.FinishPoint);
            }
            ExtraStops = new List<string>();

            List<string> ExtraStopsDupe = new List<string>();
            foreach (int? y in XtraStopsDupe) 
            {
                ExtraStopsDupe.Add(DB.Addresses.Find(y).RoadName);
            }
            foreach (int? i in XtraStops) 
            {
                ExtraStops.Add(DB.Addresses.Find(i).RoadName);
            }
        }
        public void OnGet(int id)
        {
            route = DB.Routes.Find(id);
            if (route != null) 
            {
                StartPoint = DB.Addresses.Find(route.StartPoint).RoadName;
                FinishPoint = DB.Addresses.Find(route.FinishPoint).RoadName;
                ExtraWaypoints(route.RouteId);
            }
            
        }

    }
}
