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
        public EmilDbContext DB = new EmilDbContext();
        public List<Address> AllAddresses { get; set; }
        [BindProperty]
        public User CurrentUser { get; set; }


        public void OnGet(int userId)
        {
            CurrentUser = DB.Users.Find(userId);
            myRoutes = DB.Routes.ToList();
            AllAddresses = DB.Addresses.ToList();
        }
    }
}
