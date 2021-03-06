
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OurCarZ.Model;
using OurCarZ.Pages.UserPages;
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
        [BindProperty]
        public UserRoute CancelUser { get; set; }
        [BindProperty]
        public List<Address> AddressList { get; set; }
        [BindProperty (SupportsGet = true)]
        public User CurrentUser { get; set; }
        [BindProperty]
        public List<User> UserList { get; set; }
        [BindProperty]
        public List<UserRoute> PassengerList { get; set; }
        [BindProperty]
        public Route YourRoute { get; set; }
        [BindProperty] 
        public Car YourCar { get; set; }
        [BindProperty]
        public List<Message> YourMessages { get; set; }
        [BindProperty]
        public Address StartAddress { get; set; }
        [BindProperty]
        public Address EndAddress { get; set; }
        public string RouteDate { get; set; }
        public List<string> MessageList { get; set; }
        

        public void OnGet(int routeId, int startAddressId, int endAddressId)
        {
            if(routeId != 0)
            {
            
            CurrentUser = _edb.Users.Find(LogInPageModel.LoggedInUser.UserId);
            
            YourRoute = _edb.Routes.Find(routeId);
           
            StartAddress = _edb.Addresses.Find(startAddressId);
            
            EndAddress = _edb.Addresses.Find(endAddressId);
            
            UserList = _edb.Users.ToList();

            AddressList = _edb.Addresses.Where(s => s.AddressId > 0).ToList();


            YourCar = _edb.Cars.Find(CurrentUser.LicensePlate);

            IQueryable<UserRoute> userRouteList = from s in _edb.UserRoutes
                select s;

            userRouteList = userRouteList.Where(s => s.RouteId == routeId);


            PassengerList = userRouteList.ToList();

            YourMessages = _edb.Messages.ToList();

            if (YourRoute.UserId == CurrentUser.UserId)
            {
                YourRoute.StartPoint = StartAddress.AddressId;
                YourRoute.FinishPoint = EndAddress.AddressId;
            }


            if (YourRoute.StartTime.Date == YourRoute.ArrivalTime.Value.Date)
            {
                RouteDate = YourRoute.StartTime.ToShortDateString();
            }
            }
        }




        public IActionResult OnPost(int routeid, int DeleteID, int startAddressId, int endAddressId)
        {
            CurrentUser = _edb.Users.Find(LogInPageModel.LoggedInUser.UserId);
            YourRoute = _edb.Routes.Find(routeid);
            
            StartAddress = _edb.Addresses.Find(startAddressId);
            
            EndAddress = _edb.Addresses.Find(endAddressId);
            CancelUser = _edb.UserRoutes.Find(DeleteID);

            Message msg = new Message();
            msg.MessageFrom = CurrentUser.UserId;
            msg.DateTime = DateTime.Now;
            msg.MessageText = $"You have been removed from route {StartAddress.RoadName} to {EndAddress.RoadName} start time {YourRoute.StartTime} by {CurrentUser.FirstName} {CurrentUser.LastName}";
            msg.MessageTo = CancelUser.UserId;
            
            _edb.Messages.Add(msg);
            _edb.UserRoutes.Remove(CancelUser);
            _edb.SaveChanges();

            return RedirectToPage("/DrivePage", new { UserId = CurrentUser.UserId, routeId = YourRoute.RouteId, startAddressId = StartAddress.AddressId, endAddressId = EndAddress.AddressId });
        }

        public IActionResult OnPostStart(int routeid)
        {
            IQueryable<UserRoute> userRouteList = from s in _edb.UserRoutes
                select s;

            userRouteList = userRouteList.Where(s => s.RouteId == routeid);


            PassengerList = userRouteList.ToList();

            foreach (var passenger in PassengerList)
            {
                Message msg = new Message();
                msg.MessageFrom = CurrentUser.UserId;
                msg.DateTime = DateTime.Now;
                msg.MessageText = "The Driver has Begun the route";
                msg.MessageTo = passenger.UserId;
                _edb.Messages.Add(msg);
                _edb.SaveChanges();
            }

            return RedirectToPage("/Rating/RatingPage");
        }

    }
}
