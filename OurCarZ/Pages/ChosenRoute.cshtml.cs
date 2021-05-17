using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OurCarZ.Model;

namespace OurCarZ.Pages
{
    public class ChosenRouteModel : PageModel
    {
        public Route myRoute { get; set; }
        public List<UserRoute> userRoute { get; set; }
        public EmilDbContext DB = new EmilDbContext();
        public List <Address> currentAddress { get; set; }
        public List<Address> EndAddress { get; set; }
        [BindProperty]
        public User CurrentUser { get; set; } 
        public User Driver { get; set; }
        public void OnGet(int userId, int routeId)
        {
            CurrentUser = DB.Users.Find(userId);
            currentAddress = DB.Addresses.ToList();
            myRoute = DB.Routes.Find(routeId);
            EndAddress = DB.Addresses.ToList();

            if (myRoute.UserId == myRoute.UserId)
            {
                Driver = DB.Users.Find(myRoute.UserId);
            }
        }
    }
}
