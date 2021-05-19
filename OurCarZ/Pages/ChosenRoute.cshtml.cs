using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OurCarZ.Model;
using System.Collections.Generic;
using System.Linq;
using OurCarZ.Pages.UserPages;

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
        [BindProperty]
        public List<Route> TheRoute { get; set; }
        [BindProperty]
        public List<Car> Cars { get; set; }
        [BindProperty]
        public List<User> Users { get; set; }
        public void OnGet(int routeId)
        {
            UserList = DB.Users.ToList();
            CurrentUser = DB.Users.Find(LogInPageModel.LoggedInUser.UserId);
            currentAddress = DB.Addresses.ToList();
            myRoute = DB.Routes.Find(routeId);
            EndAddress = DB.Addresses.ToList();
            UserList = DB.Users.ToList();
            YourCar = DB.Cars.Find(CurrentUser.LicensePlate);
            Cars = DB.Cars.ToList();
            Users = DB.Users.ToList();
            TheRoute = DB.Routes.ToList();
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
