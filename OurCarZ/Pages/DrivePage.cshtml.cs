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

        public void OnGet(int id)
        {
            CurrentUser = _edb.Users.Find(id);
            YourRoute = _edb.Routes.Find(9);
            StartAddress = _edb.Addresses.Find(1);
            EndAddress = _edb.Addresses.Find(2);
            if (YourRoute.UserId == CurrentUser.UserId)
            {
                YourRoute.StartPoint = StartAddress.AddressId;
                YourRoute.FinishPoint = EndAddress.AddressId;
            }
        }
    }
}
