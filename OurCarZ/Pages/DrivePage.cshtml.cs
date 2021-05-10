using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OurCarZ.Model;
using OurCarZ.Services;

namespace OurCarZ.Pages
{
    public class DrivePageModel : PageModel
    {
        public EmilDbContext _edb = new EmilDbContext();
        public DrivePageModel(EmilDbContext edb)
        {
            _edb = edb; 
        }


        [BindProperty (SupportsGet = true)]
        public User CurrentUser { get; set; }
        [BindProperty]
        public List<User> UserList { get; set; }
        public List<User> Passenger { get; set; }
        [BindProperty]
        public Route YourRoute { get; set; }
        [BindProperty]
        public List<Message> YourMessages { get; set; }
        [BindProperty]
        public Address StartAddress { get; set; }
        [BindProperty]
        public Address EndAddress { get; set; }

        public void OnGet(int userId, int routeId, int startAddressId, int endAddressId, int pasId)
        {
            //I guess we dont need to find all these ids when we implement login
            //Finds the current user (7)
            CurrentUser = _edb.Users.Find(userId);
            //finds route 9
            YourRoute = _edb.Routes.Find(routeId);
            //finds AddressId 1
            StartAddress = _edb.Addresses.Find(startAddressId);
            //finds AddressId 2
            EndAddress = _edb.Addresses.Find(endAddressId);
            //Checks if the FK in Route is == PK 
            UserList = _edb.Users.ToList();
            Passenger = _edb.Users.ToList();

            if (YourRoute.UserId == CurrentUser.UserId)
            {
                YourRoute.StartPoint = StartAddress.AddressId;
                YourRoute.FinishPoint = EndAddress.AddressId;
            }
        }
    }
}
