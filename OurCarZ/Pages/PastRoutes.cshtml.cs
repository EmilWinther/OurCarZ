using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OurCarZ.Model;
using OurCarZ.Pages.UserPages;

namespace OurCarZ.Pages
{
    public class Past_RoutesModel : PageModel
    {
        [BindProperty] public DateTime date { get; set; }
        [BindProperty]
        public User CurrentUser { get; set; }
        public List<UserRoute> PassengerRoutes { get; set; }

        public EmilDbContext DB = new();

        public List<Route> UsedRoutes = new();

        public List<Address> addresses;

        public List<User> users;

        public List<Car> cars;

        public List<int> RouteList { get; set; }

        public Past_RoutesModel(EmilDbContext db)
        {
            DB = db;
        }

        public void OnGet(int id)
        {
            RouteList = new List<int>();
            CurrentUser = DB.Users.Find(id);
            UsedRoutes = DB.Routes.Where(s => s.UserId == CurrentUser.UserId).ToList();
            PassengerRoutes = DB.UserRoutes.Where(s => s.UserId == CurrentUser.UserId).ToList();
            addresses = DB.Addresses.ToList();
            users = DB.Users.ToList();
            cars = DB.Cars.ToList();

            foreach (var userRoute in PassengerRoutes)
            {
                RouteList.Add((int)userRoute.RouteId);
            }

            foreach (var route in UsedRoutes)
            {
                RouteList.Add(route.RouteId);
            }
        }


        public void OnPost()
        {
            var allRoutes = DB.Routes.ToList();
            addresses = DB.Addresses.ToList();
            users = DB.Users.ToList();
            cars = DB.Cars.ToList();
        }

        public IActionResult OnPostRecreate(int routeId, int startAddressId, int endAddressId)
        {
            DateTime now = DateTime.Now;
            DateTime today = now.AddHours(1);
            var arrivalToday = today.AddHours(1);

            Route newRoute = new Route();
            newRoute.StartPoint = startAddressId;
            newRoute.FinishPoint = endAddressId;
            newRoute.StartTime = today;
            newRoute.ArrivalTime = arrivalToday;
            newRoute.UserId = LogInPageModel.LoggedInUser.UserId;
            DB.Routes.Add(newRoute);
            DB.SaveChanges();

            return RedirectToPage("/DrivePage", new {routeId = DB.Routes.Where(r => r.StartPoint == startAddressId && r.FinishPoint == endAddressId && r.UserId == LogInPageModel.LoggedInUser.UserId).OrderBy(o => o.RouteId).Last().RouteId, startAddressId, endAddressId });
        }
    }
}