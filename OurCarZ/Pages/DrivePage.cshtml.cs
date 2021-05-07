using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OurCarZ.Model;

namespace OurCarZ.Pages
{
    public class DrivePageModel : PageModel
    {
        private EmilDbContext _edb = new EmilDbContext();


        public DrivePageModel(EmilDbContext edb)
        {
            _edb = edb; 
        }


        [BindProperty]
        public User CurrentUser { get; set; }
        [BindProperty]
        public User Passenger { get; set; }
        [BindProperty]
        public Route YourRoute { get; set; }
        [BindProperty]
        public Message YourMessages { get; set; }
        [BindProperty]
        public Address StartAddress { get; set; }
        [BindProperty]
        public Address EndAddress { get; set; }

        public void OnGet(int userId, int routeId, int startaddressId, int endaddressId)
        {
            //Finds the current user (7)
            CurrentUser = _edb.Users.Find(userId);
            //finds route 9
            YourRoute = _edb.Routes.Find(routeId);
            //finds AddressId 1
            StartAddress = _edb.Addresses.Find(startaddressId);
            //finds AddressId 2
            EndAddress = _edb.Addresses.Find(endaddressId);
            //Checks if the FK in Route is == PK 
            if (YourRoute.UserId == CurrentUser.UserId)
            {
                YourRoute.StartPoint = StartAddress.AddressId;
                YourRoute.FinishPoint = EndAddress.AddressId;
            }
        }
    }
}
