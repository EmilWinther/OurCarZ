using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OurCarZ.Model;
using System.Collections.Generic;
using System.Linq;

namespace OurCarZ.Pages
{
    public class ChosenRouteModel : PageModel
    {
        [BindProperty]
        public List<UserRoute> PassengerList { get; set; }
        public Route myRoute { get; set; }
        public EmilDbContext DB = new EmilDbContext();
        public List<Address> currentAddress { get; set; }
        public List<Address> EndAddress { get; set; }
        [BindProperty]
        public User CurrentUser { get; set; }
        public User Driver { get; set; }
        [BindProperty]
        public string StartPoint { get; set; }
        [BindProperty]
        public string FinishPoint { get; set; }
        [BindProperty]
        public List<User> UserList { get; set; }
        [BindProperty]
        public Car YourCar { get; set; }
        public void OnGet(int userId, int routeId)
        {
            UserList = DB.Users.ToList();
            CurrentUser = DB.Users.Find(userId);
            currentAddress = DB.Addresses.ToList();
            myRoute = DB.Routes.Find(routeId);
            EndAddress = DB.Addresses.ToList();
            UserList = DB.Users.ToList();
            YourCar = DB.Cars.Find(CurrentUser.LicensePlate);
            IQueryable<UserRoute> userRouteList = from s in DB.UserRoutes
                select s;

            userRouteList = userRouteList.Where(s => s.RouteId == routeId);


            PassengerList = userRouteList.ToList();

            StartPoint = DB.Addresses.Find(myRoute.StartPoint).RoadName;
            FinishPoint = DB.Addresses.Find(myRoute.FinishPoint).RoadName;
            if (myRoute.UserId == myRoute.UserId)
            {
                Driver = DB.Users.Find(myRoute.UserId);
            }
        }
    }
}
