using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OurCarZ.Model;
using System.Collections.Generic;
using System.Linq;
using OurCarZ.Pages.UserPages;

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
        [BindProperty] public User CurrentUser { get; set; }
        public List<UserRoute> PassengerRoutes { get; set; }


        public void OnGet(int userId)
        {
            PassengerRoutes = DB.UserRoutes.ToList();
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
            return RedirectToPage("/MyRoutes", new {userId = LogInPageModel.LoggedInUser.UserId});
        }

        public IActionResult OnPostRemove(int RemoveRid, int removeId)
        {
            var RoutePassenger = DB.UserRoutes.Where(p => p.RouteId == RemoveRid).ToList();
            foreach (var userRoute in RoutePassenger)
            {
                if (userRoute.UserId == LogInPageModel.LoggedInUser.UserId)
                {
                    DB.UserRoutes.Remove(userRoute);
                }
            }

            DB.SaveChanges();
            return RedirectToPage("/MyRoutes", new {userId = LogInPageModel.LoggedInUser.UserId});
        }
    }
}
