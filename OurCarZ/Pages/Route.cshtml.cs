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
        public RouteModel(EmilDbContext edb) 
        {
            DB = edb;
        }
        public void OnGet(int id)
        {
            route = DB.Routes.Find(id);
            if (route != null) 
            {
                StartPoint = DB.Addresses.Find(route.StartPoint).RoadName;
                FinishPoint = DB.Addresses.Find(route.FinishPoint).RoadName;
            }
            
        }

    }
}
