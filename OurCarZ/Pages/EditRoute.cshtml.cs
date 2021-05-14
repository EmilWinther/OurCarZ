using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using OurCarZ.Model;

namespace OurCarZ.Pages
{
    public class EditRouteModel : PageModel
    {
        public EmilDbContext _edb = new EmilDbContext();
        public EditRouteModel(EmilDbContext edb)
        {
            _edb = edb;
        }
        
        [BindProperty(SupportsGet = true)]
        public User CurrentUser { get; set; }
        [BindProperty]
        public Route YourRoute { get; set; }
        [BindProperty]
        public Car YourCar { get; set; }
        [BindProperty]
        public Address StartAddress { get; set; }
        [BindProperty]
        public Address EndAddress { get; set; }
        [BindProperty]
        public DateTime StartTime { get; set; }
        [BindProperty]
        public DateTime ArrivalTime { get; set; }
        [BindProperty]
        public string Start { get; set; }
        [BindProperty]
        public string End { get; set; }
        [BindProperty]
        public string Seats { get; set; }





        public void OnGet(int userId, int routeId, int startAddressId, int endAddressId)
        {
            //I guess we dont need to find all these ids when we implement login
            //Finds the current user (7)
            CurrentUser = _edb.Users.Find(userId);
            //finds route 9
            YourRoute = _edb.Routes.Find(routeId);
            //finds AddressId 1
            StartAddress = _edb.Addresses.Find(startAddressId);
            //finds AddressId 2
            EndAddress = _edb.Addresses.Find(endAddressId);
            //Checks if the FK in Route is == PK 
            YourCar = _edb.Cars.Find(CurrentUser.LicensePlate);

        }
        public IActionResult OnPost()
        {
            YourRoute = _edb.Routes.Find(YourRoute.RouteId);
            CurrentUser = _edb.Users.Find(YourRoute.UserId);
            YourCar = _edb.Cars.Find(CurrentUser.LicensePlate);
            //finds AddressId 1
            StartAddress = _edb.Addresses.Find(YourRoute.StartPoint);
            //finds AddressId 2
            EndAddress = _edb.Addresses.Find(YourRoute.FinishPoint);

            //Change the desired values, we grab from the page:
            if (Start != null)
            {
                StartAddress.RoadName = Start;
            }
            if (End != null)
            {
                EndAddress.RoadName = End;
            }

            YourRoute.StartTime = StartTime;

            YourRoute.ArrivalTime = ArrivalTime;

            if (Seats != null)
            {
                YourCar.Seats = Seats;
            }


            //Update the user. Finds the user based on the primary key (UserId). If a new primary key is somehow inserted, it makes a new user instead.
            _edb.Routes.Update(YourRoute);
            _edb.Cars.Update(YourCar);
            _edb.SaveChanges();

            //Go to profile for the specified user.
            return RedirectToPage("/DrivePage", new { UserId = CurrentUser.UserId, routeId = YourRoute.RouteId, startAddressId = StartAddress.AddressId, endAddressId = EndAddress.AddressId });
        }
    }
}
