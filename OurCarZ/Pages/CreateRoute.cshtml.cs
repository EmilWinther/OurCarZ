using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OurCarZ.Model;

namespace OurCarZ.Pages
{
    public class CreateRouteModel : PageModel
    {
        private EmilDbContext _edb;

        public CreateRouteModel(EmilDbContext edb)
        {
            _edb = edb;
        }
        [BindProperty]
        public Address StartAddress { get; set; }
        [BindProperty]
        public Address EndAddress { get; set; }
        [BindProperty]
        public Route Route { get; set; }

        public void OnGet()
        {

        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                Route route = new Route();
                route.StartPoint = StartAddress.AddressId;
                route.FinishPoint = EndAddress.AddressId;
                
                _edb.Addresses.Add(StartAddress);
                _edb.Addresses.Add(EndAddress);
                _edb.SaveChanges();
                _edb.Routes.Add(route);
                _edb.SaveChanges();
                return Page();
            }
            return null;
        }
    }
}
