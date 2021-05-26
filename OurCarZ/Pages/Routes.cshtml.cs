using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OurCarZ.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using OurCarZ.Pages.UserPages;
using UserRoute = OurCarZ.Model.UserRoute;

namespace OurCarZ.Pages
{
    public class RoutesModel : PageModel
    {
        [BindProperty] public string ZipCode { get; set; }
        [BindProperty] public DateTime date { get; set; }

        public EmilDbContext DB = new EmilDbContext();

        public List<Route> UsedRoutes = new List<Route>();

        public List<UserRoute> PassengerRoutes = new List<UserRoute>();

        public List<Address> addresses;

        public List<User> users;

        public List<Car> cars;

        public int count;

        
        public RoutesModel(EmilDbContext db)
        {
            DB = db;
        }

        public void OnGet()
        {
            UsedRoutes = DB.Routes.ToList();
            PassengerRoutes = DB.UserRoutes.ToList();
            addresses = DB.Addresses.ToList();
            users = DB.Users.ToList();
            cars = DB.Cars.ToList();
            count = 0;
        }



        public void OnPost()
        {
            ZipCode = Request.Form["ZipCodeSearch"];



            
            addresses = DB.Addresses.ToList();
            users = DB.Users.ToList();
            cars = DB.Cars.ToList();

            UsedRoutes = Filter();

            
        }

        public int increaseCount()
        {
            count++;
            return count;
        }

        public List<Route> Filter ()
        {

            DateTime dateValue;
            var datebool = DateTime.TryParse(Request.Form["dateOfRoute"], out dateValue);
            if (datebool)
            {
                date = Convert.ToDateTime(Request.Form["dateOfRoute"]);
            }

            var allRoutes = DB.Routes.ToList();
            if (!string.IsNullOrEmpty(ZipCode))
            {
                UsedRoutes = new List<Route>();

                foreach (var route in allRoutes)
                {
                    if (addresses.Where(a => (a.AddressId == route.StartPoint || a.AddressId == route.FinishPoint) && a.ZipCode.ToString().Contains(ZipCode)).FirstOrDefault() != null)
                    {
                        UsedRoutes.Add(route);
                    }
                }
            }
            else { UsedRoutes = allRoutes; }

            if (datebool)
            {
                foreach (var route in UsedRoutes.ToList())
                {
                    if (DateTime.Compare(route.StartTime.Date, date.Date) != 0)
                    {
                        UsedRoutes.Remove(route);
                    }
                }
            }
            return UsedRoutes;
        }
    }
}