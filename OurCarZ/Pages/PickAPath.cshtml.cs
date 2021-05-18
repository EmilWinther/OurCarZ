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
        public Address PhotopathAdr { get; set; }
        public List<string> ExtraStops { get; set; }
        public List<int?> XtraStopsDupe { get; set; }
        public string CombineZipAndRoad(int? id)
        {
            string returner;
            returner = _edb.Addresses.Find(id).RoadName + " " + _edb.Addresses.Find(id).ZipCode;
            return returner;
        }
        public void ExtraWaypoints(int router)
        {
            List<int?> XtraStops;
            XtraStops = _edb.UserRoutes.Where(x => x.RouteId == router)
                .Select(x => x.Via).ToList();
            XtraStopsDupe = XtraStops.Distinct().ToList();
            if (XtraStopsDupe.Contains(Route.StartPoint))
            {
                XtraStopsDupe.Remove(Route.StartPoint);
            }
            if (XtraStopsDupe.Contains(Route.FinishPoint))
            {
                XtraStopsDupe.Remove(Route.FinishPoint);
            }
            ExtraStops = new List<string>();

            List<string> ExtraStopsDupe = new List<string>();
            foreach (int? y in XtraStopsDupe)
            {
                ExtraStopsDupe.Add(_edb.Addresses.Find(y).RoadName);
            }
            foreach (int? i in XtraStops)
            {
                ExtraStops.Add(_edb.Addresses.Find(i).RoadName);
            }
        }

        public void OnGet(int id)
        {
            Route = _edb.Routes.Find(id);
            if (Route != null)
            {
                StartAddress = _edb.Addresses.Find(Route.StartPoint);
                EndAddress = _edb.Addresses.Find(Route.FinishPoint);
                ExtraWaypoints(Route.RouteId);
            }
        }
        public IActionResult OnPost(int id) 
        {
            OnGet(id);
            //Do you like it?
            return Page();
        }
        public IActionResult OnPostRequest()
        {
            return RedirectToPage("Index");
        }
    }
}