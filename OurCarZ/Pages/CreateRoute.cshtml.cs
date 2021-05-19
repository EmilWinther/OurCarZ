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

        public void OnGet()
        {
            if (UserPages.LogInPageModel.LoggedInUser != null) 
            { 
                CurrentUser = _edb.Users.Find(UserPages.LogInPageModel.LoggedInUser.UserId);
            }
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {

                if (!_edb.Addresses.Any(x => x.RoadName == StartAddress.RoadName && x.ZipCode == StartAddress.ZipCode))
                {
                    _edb.Addresses.Add(StartAddress);
                }
                else
                {
                    StartAddress = _edb.Addresses.FirstOrDefault(x => x.RoadName == StartAddress.RoadName && x.ZipCode == StartAddress.ZipCode);
                }

                if (!_edb.Addresses.Any(x => x.RoadName == EndAddress.RoadName && x.ZipCode == EndAddress.ZipCode))
                {
                    _edb.Addresses.Add(EndAddress);
                }
                else
                {
                    EndAddress = _edb.Addresses.FirstOrDefault(x => x.RoadName == EndAddress.RoadName && x.ZipCode == EndAddress.ZipCode);
                }

                _edb.SaveChanges();

                Route.StartPoint = StartAddress.AddressId;
                Route.FinishPoint = EndAddress.AddressId;

                Route.UserId = UserPages.LogInPageModel.LoggedInUser.UserId;

                _edb.Routes.Add(Route);
                _edb.SaveChanges();
                OnGet();
                return Page();
            }
            return null;
        }
    }
}