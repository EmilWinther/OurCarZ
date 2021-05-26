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
        [BindProperty]
        public Route myRoute { get; set; }
        [BindProperty]
        public List<Address> currentAddress { get; set; }
        [BindProperty]
        public List<Address> EndAddress { get; set; }
        [BindProperty]
        public User CurrentUser { get; set; }
        [BindProperty]
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
        public UserRoute Passenger { get; set; }
        public EmilDbContext _edb;
        public ChosenRouteModel(EmilDbContext edb) 
        {
            _edb = edb;
        }
        public string CombineZipAndRoad(int? id)
        {
            //Get an Addresses name and zipcode based on AddressId
            if (id != null)
            {
                string returner;
                returner = _edb.Addresses.Find(id).RoadName + " " + _edb.Addresses.Find(id).ZipCode;
                return returner;
            }
            return "";
        }
        public void OnGet(int routeId)
        {
            UserList = _edb.Users.ToList();
            CurrentUser = _edb.Users.Find(LogInPageModel.LoggedInUser.UserId);
            currentAddress = _edb.Addresses.ToList();
            myRoute = _edb.Routes.Find(routeId);
            EndAddress = _edb.Addresses.ToList();
            UserList = _edb.Users.ToList();
            YourCar = _edb.Cars.Find(CurrentUser.LicensePlate);
            Cars = _edb.Cars.ToList();
            Users = _edb.Users.ToList();
            TheRoute = _edb.Routes.ToList();
            IQueryable<UserRoute> userRouteList = from s in _edb.UserRoutes
                select s;

            userRouteList = userRouteList.Where(s => s.RouteId == routeId);


            PassengerList = userRouteList.ToList();

            StartPoint = _edb.Addresses.Find(myRoute.StartPoint).RoadName;
            FinishPoint = _edb.Addresses.Find(myRoute.FinishPoint).RoadName;
            if (myRoute.UserId == myRoute.UserId)
            {
                Driver = _edb.Users.Find(myRoute.UserId);
            }
        }

    }
}
