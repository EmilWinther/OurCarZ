using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public List<int?> XtraStops { get; set; }
        public string CombineZipAndRoad(int? id)
        {
            //Get an Addresses name and zipcode based on AddressId
            string returner;
            returner = _edb.Addresses.Find(id).RoadName + " " + _edb.Addresses.Find(id).ZipCode;
            return returner;
        }
        public void ExtraWaypoints(int router)
        {
            XtraStops = _edb.UserRoutes.Where(x => x.RouteId == router)
                .Select(x => x.Via).ToList();
            //Distinct, to ensure no dupes.
            XtraStops = XtraStops.Distinct().ToList();
            //remove start and end point for XtraStops, the maps library we used doesn't like dupe addresses :c
            if (XtraStops.Contains(Route.StartPoint))
            {
                XtraStops.Remove(Route.StartPoint);
            }
            if (XtraStops.Contains(Route.FinishPoint))
            {
                XtraStops.Remove(Route.FinishPoint);
            }
        }

        public IActionResult OnGet(int id)
        {
            //Check, if the user is logged.
            if (UserPages.LogInPageModel.LoggedInUser != null)
            {
                //Sets the currentUser as the loggedin user, used later for adding UserId unto the chosen Route, along with the extra pick-up point.
                currentUser = _edb.Users.Find(UserPages.LogInPageModel.LoggedInUser.UserId);
            
                Route = _edb.Routes.Find(id);
                //check if the route isn't null (just in case)
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
            //OnGet is called to ensure OnGet is run, when Page is reloaded. Otherwise, it doesn't!
            //(Last this was checked, without this line additional stops for a Route doesn't get loaded, only the base Route.)
            OnGet(id);
            return Page();
        }
        public IActionResult OnPostRequest()
        {
            Route = _edb.Routes.Find(Route.RouteId);
            //check to see if values are typed in the input fields
            if (PhotopathAdr.RoadName != null || ZipCode != 0)
            {
                //check to see if there are less participants in a route, than there are seats in the car driving them.
                if (Convert.ToInt32(_edb.Cars.Find(_edb.Users.Find(Route.UserId).LicensePlate).Seats) > _edb.UserRoutes.Where(x => x.RouteId == Route.RouteId).Select(x => x.Via).ToList().Count) 
                {
                    PhotopathAdr.ZipCode = ZipCode;
                    //add an address, if it doesn't exist in the address table
                    if (!_edb.Addresses.Any(x => x.RoadName == PhotopathAdr.RoadName))
                    {
                        _edb.Addresses.Add(PhotopathAdr);
                        _edb.SaveChanges();
                    }
                    //Adds the users pickup point to the route.
                    UserRoute ur = new UserRoute();
                    ur.RouteId = Route.RouteId;
                    ur.UserId = _edb.Users.Find(UserPages.LogInPageModel.LoggedInUser.UserId).UserId;
                    ur.Via = _edb.Addresses.FirstOrDefault(x => x.RoadName == PhotopathAdr.RoadName).AddressId;

                    _edb.UserRoutes.Add(ur);
                    _edb.SaveChanges();

                    //Boots to index. Could be a little nicer...
                    return RedirectToPage("/Rating/RatingPage");
                }
            }
            //OnGet is called to ensure OnGet is run, when Page is reloaded. Otherwise, it doesn't!
            //(Last this was checked, without this line additional stops for a Route doesn't get loaded, only the base Route.)
            OnGet(Route.RouteId);
            return Page();

        }
    }
}