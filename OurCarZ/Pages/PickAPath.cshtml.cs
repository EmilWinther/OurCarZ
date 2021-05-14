using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OurCarZ.Model;

namespace OurCarZ.Pages
{
    public class PickAPathModel : PageModel
    {
        private EmilDbContext _edb;
        

        public PickAPathModel(EmilDbContext edb)
        {
            _edb = edb;
        }
        [BindProperty]
        public Route Route { get; set; }
        [BindProperty]
        public Address StartAddress { get; set; }
        [BindProperty]
        public Address EndAddress { get; set; }
        [BindProperty]
        public string Lookatthisphotopath { get; set; }

        public void OnGet()
        {
            StartAddress = new Address();
            StartAddress.RoadName = "Roskilde, DK";
            EndAddress = new Address();
            EndAddress.RoadName = "København, DK";
        }

        public IActionResult OnPost()
        {
            return null;
        }
    }
}