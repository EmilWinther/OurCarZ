using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OurCarZ.Mock;
using OurCarZ.Model;

namespace OurCarZ.Pages
{
    public class RoutesModel : PageModel
    {

        public List<Route> routes = new List<Route>()
        {
            new Route(1, "Ahornlunden 5, Roskilde, Dk","Magleg�rdsvej 2, 4000 Roskilde, Dk"),
            new Route(2, "�rhus","Magleg�rdsvej 2, 4000 Roskilde, Dk"),
            new Route(3, "Roskilde, Dk","Magleg�rdsvej 2, 4000 Roskilde, Dk"),
            new Route(4, "K�benhavn","Magleg�rdsvej 2, 4000 Roskilde, Dk")
        };

        public void OnGet()
        {
        }
    }
}
