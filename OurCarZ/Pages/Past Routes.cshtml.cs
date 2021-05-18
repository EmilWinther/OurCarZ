using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OurCarZ.Model;

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
    }
}