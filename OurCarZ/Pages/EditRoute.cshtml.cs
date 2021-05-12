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
        public List<User> UserList { get; set; }
        [BindProperty]
        public List<UserRoute> PassengerList { get; set; }
        [BindProperty]
        public Route YourRoute { get; set; }
        [BindProperty]
        public Car YourCar { get; set; }
        [BindProperty]
        public List<Message> YourMessages { get; set; }
        [BindProperty]
        public Address StartAddress { get; set; }
        [BindProperty]
        public Address EndAddress { get; set; }
      

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
            UserList = _edb.Users.ToList();

            YourCar = _edb.Cars.Find(CurrentUser.LicensePlate);

            IQueryable<UserRoute> userRouteList = from s in _edb.UserRoutes
                select s;

            userRouteList = userRouteList.Where(s => s.RouteId == routeId);

            PassengerList = userRouteList.ToList();

            YourMessages = _edb.Messages.ToList();

            if (YourRoute.UserId == CurrentUser.UserId)
            {
                YourRoute.StartPoint = StartAddress.AddressId;
                YourRoute.FinishPoint = EndAddress.AddressId;
            }



        }
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            if (StartAddress == null && EndAddress == null && YourCar.Seats == null)
            {
                return Page();
            }
            //No persistence between OnGet and OnPost, load the currentUser to update:
            YourRoute = _edb.Routes.Find(YourRoute.RouteId);

            //Change the desired values, we grab from the page:
            if ( StartAddress != null)
            {
                YourRoute.StartPoint = StartAddress.AddressId;
            }
            if (EndAddress != null)
            {
                YourRoute.FinishPoint = EndAddress.AddressId;
            }
            if (YourRoute.StartTime != null)
            {
                YourRoute.StartTime = YourRoute.StartTime;
            }
            if (YourRoute.ArrivalTime != null)
            {
                YourRoute.ArrivalTime = YourRoute.ArrivalTime;
            }
            if (YourCar.Seats != null)
            {
                YourCar.Seats = YourCar.Seats;
            }
            

            //Update the user. Finds the user based on the primary key (UserId). If a new primary key is somehow inserted, it makes a new user instead.
            _edb.Cars.Update(YourCar);
            _edb.Routes.Update(YourRoute);
            _edb.SaveChanges();

            //Go to profile for the specified user.
            return RedirectToPage("/DrivePage", new { UserId = CurrentUser.UserId, routeId = YourRoute.RouteId, startAddressId = StartAddress.AddressId, endAddressId = EndAddress.AddressId });
        }
    }
}
