using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OurCarZ.Model;
using OurCarZ.Pages.UserPages;

namespace OurCarZ.Pages
{
    public class PickAPathModel : PageModel
    {
        private EmilDbContext _edb;
        public User currentUser { get; set; }
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
        [BindProperty]
        [Required(ErrorMessage = "Zipcode is required."), RegularExpression(@"^(\d{4})$", ErrorMessage = "Enter a valid 4 digit ZipCode")]
        public int ZipCode { get; set; }
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

        public IActionResult OnGet(int id)
        {
            if (LogInPageModel.LoggedInUser != null)
            {
                currentUser = _edb.Users.Find(UserPages.LogInPageModel.LoggedInUser.UserId);
            
            Route = _edb.Routes.Find(id);
            if (Route != null)
            {
                StartAddress = _edb.Addresses.Find(Route.StartPoint);
                EndAddress = _edb.Addresses.Find(Route.FinishPoint);
                ExtraWaypoints(Route.RouteId);
            }
            }
            return Page();
        }
        public IActionResult OnPost(int id) 
        {
            PhotopathAdr.ZipCode = ZipCode;
            OnGet(id);
            //Do you like it?
            return Page();
        }
        public IActionResult OnPostRequest()
        {
            if (PhotopathAdr.RoadName != null || ZipCode != 0)
            {
                PhotopathAdr.ZipCode = ZipCode;
                if (!_edb.Addresses.Any(x => x.RoadName == PhotopathAdr.RoadName))
                {
                    _edb.Addresses.Add(PhotopathAdr);
                    _edb.SaveChanges();
                }
                UserRoute ur = new UserRoute();
                ur.RouteId = Route.RouteId;
                ur.UserId = _edb.Users.Find(UserPages.LogInPageModel.LoggedInUser.UserId).UserId;
                ur.Via = _edb.Addresses.FirstOrDefault(x => x.RoadName == PhotopathAdr.RoadName).AddressId;

                _edb.UserRoutes.Add(ur);
                _edb.SaveChanges();

                return RedirectToPage("Index");
            }
            else 
            {
                OnGet(Route.RouteId);
                return Page();
            }

        }
    }
}