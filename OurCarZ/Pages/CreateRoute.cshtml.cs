using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OurCarZ.Model;

namespace OurCarZ.Pages
{
    public class CreateRouteModel : PageModel
    {
        private EmilDbContext _edb;

        public CreateRouteModel(EmilDbContext edb)
        {
            _edb = edb;
        }
        [BindProperty]
        public User CurrentUser { get; set; }
        [BindProperty]
        public Route Route { get; set; }
        [BindProperty]
        public Address StartAddress { get; set; }
        [BindProperty]
        public Address EndAddress { get; set; }

        public void OnGet(int id)
        {
            CurrentUser = _edb.Users.Find(id);
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {

                if (!_edb.Addresses.Any(x => x.RoadName == StartAddress.RoadName))
                {
                    _edb.Addresses.Add(StartAddress);
                }
                else
                {
                    StartAddress = _edb.Addresses.FirstOrDefault(x => x.RoadName == StartAddress.RoadName);
                }

                if (!_edb.Addresses.Any(x => x.RoadName == EndAddress.RoadName))
                {
                    _edb.Addresses.Add(EndAddress);
                }
                else
                {
                    EndAddress = _edb.Addresses.FirstOrDefault(x => x.RoadName == EndAddress.RoadName);
                }

                _edb.SaveChanges();

                Route.StartPoint = StartAddress.AddressId;
                Route.FinishPoint = EndAddress.AddressId;

                //Implement Login Functionality here:
                Route.UserId = 6;

                _edb.Routes.Add(Route);
                _edb.SaveChanges();

                return Page();
            }
            return null;
        }
    }
}