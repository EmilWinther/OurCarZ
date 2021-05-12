using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OurCarZ.Model;
using OurCarZ.Services;

namespace OurCarZ.Pages
{
    public class DrivePageModel : PageModel
    {
        public EmilDbContext _edb = new EmilDbContext();
        public DrivePageModel(EmilDbContext edb)
        {
            _edb = edb; 
        }

        [BindProperty]
        public UserRoute CancelUser { get; set; }

        [BindProperty (SupportsGet = true)]
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

        public void OnGet(int userId, int routeId, int startAddressId, int endAddressId, int pasId)
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

        public IActionResult OnPost(int userId, int routeid, int DeleteID, int startAddressId, int endAddressId)
        {
            CurrentUser = _edb.Users.Find(userId);
            YourRoute = _edb.Routes.Find(routeid);
            //finds AddressId 1
            StartAddress = _edb.Addresses.Find(startAddressId);
            //finds AddressId 2
            EndAddress = _edb.Addresses.Find(endAddressId);
            CancelUser = _edb.UserRoutes.Find(DeleteID);

            _edb.UserRoutes.Remove(CancelUser);
            _edb.SaveChanges();

            return RedirectToPage("/DrivePage", new { UserId = CurrentUser.UserId, routeId = YourRoute.RouteId, startAddressId = StartAddress.AddressId, endAddressId = EndAddress.AddressId });
        }
    }
}
