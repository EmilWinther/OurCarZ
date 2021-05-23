using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using OurCarZ.Model;
using Xceed.Wpf.Toolkit;

namespace OurCarZ.Pages
{
    public class EditRouteModel : PageModel
    {
        private EmilDbContext _edb;
        public EditRouteModel(EmilDbContext edb)
        {
            _edb = edb;
        }
        
        [BindProperty(SupportsGet = true)]
        public User CurrentUser { get; set; }
        [BindProperty]
        public Route YourRoute { get; set; }
        [BindProperty]
        public Car YourCar { get; set; }
        [BindProperty]
        public Address StartAddress { get; set; }
        [BindProperty]
        public Address EndAddress { get; set; }
        [BindProperty]
        public DateTime StartTime { get; set; }
        [BindProperty]
        public DateTime ArrivalTime { get; set; }
        [BindProperty]
        public string Start { get; set; }
        [BindProperty]
        public string End { get; set; }
        [BindProperty]
        public int Seats { get; set; }
        public string RouteDate { get; set; }
        public List<Address> AddressList { get; set; }
        public List<UserRoute> PassengerList { get; set; }
        [BindProperty]
        public int StartZip { get; set; }
        
        [BindProperty]
        public int EndZip { get; set; }
        public List<Institution> Zealand { get; set; }



        public void OnGet(int userId, int routeId, int startAddressId, int endAddressId)
        {
           
            CurrentUser = _edb.Users.Find(userId);
            YourRoute = _edb.Routes.Find(routeId);
            StartAddress = _edb.Addresses.Find(startAddressId);
            EndAddress = _edb.Addresses.Find(endAddressId);
          

            YourCar = _edb.Cars.Find(CurrentUser.LicensePlate);
            if (YourRoute.StartTime.Date == YourRoute.ArrivalTime.Value.Date)
            {
                RouteDate = YourRoute.StartTime.ToShortDateString();
            }

            AddressList = _edb.Addresses.Where(s => s.AddressId > 0).ToList();
            IQueryable<UserRoute> userRouteList = from s in _edb.UserRoutes
                select s;

            userRouteList = userRouteList.Where(s => s.RouteId == routeId);
            PassengerList = userRouteList.ToList();

        }

        


        public IActionResult OnPost()
        {
            YourRoute = _edb.Routes.Find(YourRoute.RouteId);
            CurrentUser = _edb.Users.Find(YourRoute.UserId);
            YourCar = _edb.Cars.Find(CurrentUser.LicensePlate);
            //finds AddressId 1
            StartAddress = _edb.Addresses.Find(YourRoute.StartPoint);
            //finds AddressId 2
            EndAddress = _edb.Addresses.Find(YourRoute.FinishPoint);

            if (Start != null && StartZip != 0)
            {
                if (!_edb.Addresses.Any(x => x.RoadName == Start && x.ZipCode == StartZip))
                {
                    Address newStartAddress = new Address();
                    newStartAddress.RoadName = Start;
                    newStartAddress.ZipCode = StartZip;
                    newStartAddress.Country = "DK";
                    _edb.Addresses.Add(newStartAddress);
                }
                else
                {
                    StartAddress = _edb.Addresses.FirstOrDefault(x => x.RoadName == Start && x.ZipCode == StartZip);
                }
            }
           

            if (End != null && EndZip != 0)
            {
                if (!_edb.Addresses.Any(x => x.RoadName == End && x.ZipCode == EndZip))
                {
                    Address newEndAddress = new Address();
                    newEndAddress.RoadName = End;
                    newEndAddress.ZipCode = EndZip;
                    newEndAddress.Country = "DK";
                    _edb.Addresses.Add(newEndAddress);
                }
                else
                {
                    EndAddress = _edb.Addresses.FirstOrDefault(x => x.RoadName == End && x.ZipCode == EndZip);
                }
            }
         

            YourRoute.StartPoint = StartAddress.AddressId;
            YourRoute.FinishPoint = EndAddress.AddressId;


            if (StartTime.Date != DateTime.MinValue)
            {
                YourRoute.StartTime = StartTime;
            }
            

            if (ArrivalTime != DateTime.MinValue)
            {
                YourRoute.ArrivalTime = ArrivalTime;
            }

            if (Seats != 0)
            {
                YourCar.Seats = Seats;
            }
            _edb.Cars.Update(YourCar);

            Zealand = _edb.Institutions.ToList();
            List<Institution> InstitutionList = new List<Institution>();
            foreach (var institution in Zealand)
            {
                if (StartAddress.RoadName == _edb.Addresses.Find(institution.Address).RoadName || EndAddress.RoadName == _edb.Addresses.Find(institution.Address).RoadName)
                {
                    InstitutionList.Add(institution);
                }
            }

            if (InstitutionList.Count == 0 || YourRoute.StartTime < DateTime.Now || YourRoute.StartTime > YourRoute.ArrivalTime)
            {
                return RedirectToPage("/DrivePage", new { routeId = 0 });
            }
            else
            {
                _edb.Routes.Update(YourRoute);
                _edb.SaveChanges();

                return RedirectToPage("/DrivePage", new { UserId = CurrentUser.UserId, routeId = YourRoute.RouteId, startAddressId = StartAddress.AddressId, endAddressId = EndAddress.AddressId });
            }

        }
    }
}
