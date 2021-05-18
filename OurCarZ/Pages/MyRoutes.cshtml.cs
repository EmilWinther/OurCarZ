using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OurCarZ.Model;
using System.Collections.Generic;
using System.Linq;

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

        public IActionResult OnPost(int DeleteRid, int userId)
        {
            CurrentUser = DB.Users.Find(userId);
            myRoutes = DB.Routes.ToList();
            userRoutes = DB.UserRoutes.ToList();
            DB.Routes.Find(DeleteRid);

            var RouteUser = DB.Routes.Where(x => x.RouteId == DeleteRid).ToList();
            foreach (var route in RouteUser)
            {
                DB.Routes.Remove(route);
            }

            DB.SaveChanges();
            return RedirectToPage("/MyRoutes", new { userId = CurrentUser.UserId });
        }
    }
}
