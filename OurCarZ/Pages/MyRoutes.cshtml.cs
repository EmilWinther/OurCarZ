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
            var DeleteRoute2 = DB.UserRoutes.Where(x => x.RouteId == DeleteRid).First();
            DB.UserRoutes.Remove(DeleteRoute2);

            var DeleteAddress = DB.Addresses.Where(x => x.AddressId == DeleteAid).ToList();
            foreach (var addresses in DeleteAddress)
            {
                DB.Addresses.Remove(addresses);
            }

            DB.UserRoutes.Remove(DeleteRoute2);
            DB.SaveChanges();
            return RedirectToPage("/MyRoutes", new { id = CurrentUser.UserId });
        }
    }
}
