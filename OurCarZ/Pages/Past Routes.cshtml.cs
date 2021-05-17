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
        [BindProperty] public string ZipCode { get; set; }
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
            ZipCode = Request.Form["ZipCodeSearch"];
            if (Request.Form["dateOfRoute"].Count > 0) date = Convert.ToDateTime(Request.Form["dateOfRoute"]);

            var allRoutes = DB.Routes.ToList();
            addresses = DB.Addresses.ToList();
            users = DB.Users.ToList();
            cars = DB.Cars.ToList();

            if (!string.IsNullOrEmpty(ZipCode))
            {
                UsedRoutes = new List<Route>();

                foreach (var route in allRoutes.Where(route => addresses.FirstOrDefault(a => (a.AddressId == route.StartPoint || a.AddressId == route.FinishPoint) &&
                    a.ZipCode.ToString().Contains(ZipCode)) != null))
                    UsedRoutes.Add(route);
            }
            else
            {
                UsedRoutes = allRoutes;
            }

            if (date != null)
                foreach (var route in UsedRoutes.ToList())
                    if (DateTime.Compare(route.StartTime, date) != 0)
                        UsedRoutes.Remove(route);
        }
    }
}