using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OurCarZ.Model;
using OurCarZ.Pages.UserPages;

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
        public Route Route { get; set; }
        [BindProperty]
        public Address StartAddress { get; set; }
        [BindProperty]
        public Address EndAddress { get; set; }
        [BindProperty]
        public DateTime StartTime { get; set; }
        public List<Institution> Zealand { get; set; }


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

                Route.StartTime = StartTime;
                Route.StartPoint = StartAddress.AddressId;
                Route.FinishPoint = EndAddress.AddressId;
                Zealand = _edb.Institutions.ToList();

                //Implement Login Functionality here:
                Route.UserId = @LogInPageModel.LoggedInUser.UserId;

                List<Institution> InstitutionList = new List<Institution>();
                foreach (var institution in Zealand)
                {
                    if (StartAddress.RoadName == _edb.Addresses.Find(institution.Address).RoadName || EndAddress.RoadName == _edb.Addresses.Find(institution.Address).RoadName)
                    {
                        InstitutionList.Add(institution);
                    }
                }

                if (InstitutionList == null || InstitutionList.Count == 0 || Route.StartTime < DateTime.Now)
                {
                    throw new Exception();
                }
                else
                {
                    _edb.Routes.Add(Route);
                    _edb.SaveChanges();

                    return Page();
                }

            }
            return null;
        }
    }
}