using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OurCarZ.Pages
{
    public class RouteModel : PageModel
    {
        public string StartPoint = "Ahornlunden 5, DK";
        public string FinishPoint = "Maglegårdsvej 2, Roskilde, DK";

        public void OnGet()
        {

        }
    }
}
