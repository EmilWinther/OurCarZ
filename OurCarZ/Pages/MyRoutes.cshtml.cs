using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OurCarZ.Model;

namespace OurCarZ.Pages
{
    public class MyRoutesModel : PageModel
    {
        public List<Route> myRoutes { get; set; }
        public List<UserRoute> userRoutes { get; set; }
        public Route CurrentRoute { get; set; }
        public EmilDbContext DB = new EmilDbContext();
        public List<Address> AllAddresses { get; set; }
        public Address CurrentStartAddress { get; set; }
        [BindProperty]
        public User CurrentUser { get; set; }


        public void OnGet(int userId)
        {
            CurrentUser = DB.Users.Find(userId);
            myRoutes = DB.Routes.ToList();
            AllAddresses = DB.Addresses.ToList();
        }

        public IActionResult OnPost(int DeleteRid, int DeleteAid, int userId)
        {
            CurrentUser = DB.Users.Find(userId);
            myRoutes = DB.Routes.ToList();
            userRoutes = DB.UserRoutes.ToList();

            
            foreach (var route in myRoutes)
            {
                if (CurrentUser.UserId == route.UserId)
                {
                    DeleteAid = route.StartPoint;
                    DB.Routes.Remove(route);
                }
            }

            foreach (var userRoute in userRoutes)
            {
                if (CurrentUser.UserId == userRoute.UserId)
                {
                    DB.UserRoutes.Remove(userRoute);
                }
            }

            DB.SaveChanges();

            return RedirectToPage("/MyRoutes", new{ id = 6 });
        }
    }
}
