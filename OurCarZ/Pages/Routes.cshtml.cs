using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OurCarZ.Model;

namespace OurCarZ.Pages
{
    public class RoutesModel : PageModel
    {
        [BindProperty]
        public string ZipCode { get; set; }
        [BindProperty]
        public DateTime date { get; set; }

        public List<Route> routes = new List<Route>()
        {
            //new Route(1, "Ahornlunden 5, 4000 Roskilde, Dk","humle 2, 4000 Roskilde, Dk"),
            //new Route(2, "Århus 5000","Maglegårdsvej 2, 5000 Roskilde, Dk"),
            //new Route(3, "Roskilde 4000, Dk","Maglegårdsvej 2, 4000 Roskilde, Dk"),
            //new Route(4, "København 6000","sngel, 4000 Roskilde, Dk")
        };
        public List<Route> UsedRoutes = new List<Route>();
        



        public void OnGet(string zipCode, DateTime time)
        {
            UsedRoutes = routes;
            
        }

        public void OnPost()
        {
            ZipCode = Request.Form["ZipCodeSearch"];
            if (Request.Form["dateOfRoute"].Count > 0) { date = Convert.ToDateTime(Request.Form["dateOfRoute"]); }
            if (!string.IsNullOrEmpty(ZipCode))
            {
                //var sortedRoutes = routes.Where(r => r.StartPoint.Contains(ZipCode) || r.FinishPoint.Contains(ZipCode));
                UsedRoutes = new List<Route>();
                //foreach (var item in sortedRoutes)
                //{
                //   UsedRoutes.Add(item);
                //}
            } else { UsedRoutes = routes; }

            
        }


    }
}
