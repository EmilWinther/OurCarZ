using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OurCarZ.Model;

namespace OurCarZ.Pages
{
    public class RouteModel : PageModel
    {

        [BindProperty]
        public Route route{ get; set; }
        [BindProperty]
        public string StartPoint { get; set; }
        [BindProperty]
        public string FinishPoint { get; set; }

        public void OnGet(int id)
        {
            //Ret nedenstående til den rigtige kode
            //route = RouteService.GetOneRoute(id);
            //route.StartPoint = StartPoint;
            //route.FinishPoint = FinishPoint;

        }


    }
}
