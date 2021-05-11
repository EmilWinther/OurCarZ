using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using OurCarZ.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OurCarZ.Pages
{
    public class ProfileModel : PageModel
    {
        private readonly ILogger<ProfileModel> _logger;
        public EmilDbContext DB = new EmilDbContext();
        public User currentUser { get; set; }
        public RatingDatabase RatingUser { get; set; }
        public double? avg { get; set; }

        public ProfileModel(ILogger<ProfileModel> logger, EmilDbContext db)
        {
            _logger = logger;
            DB = db;
        }

        public void OnGet(int id)
        {
            currentUser = DB.Users.Find(id);
            var reviews = (from x in DB.RatingDatabases where x.UserRatedId.Equals(id) select x).ToList();
            avg = (from x in reviews select x.Rating).Average();

        }

        public IActionResult OnPost(int DeleteID)
        {
            currentUser = DB.Users.Find(DeleteID);

            if (avg > 0)
            {
                var RatingUser = DB.RatingDatabases.Where(x => x.UserRatedId == DeleteID || x.UserRatingId == DeleteID).First();
                DB.RatingDatabases.Remove(RatingUser);
            }
            

            var MessageUser = DB.Messages.Where(x => x.MessageFrom == DeleteID || x.MessageTo == DeleteID).ToList();
            foreach (var message in MessageUser)
            {
                DB.Messages.Remove(message);
            }

            if (currentUser.LicensePlate != null)
            {
                var UserLicensePlate = DB.Cars.Where(x => x.LicensePlate == currentUser.LicensePlate).First();
                DB.Cars.Remove(UserLicensePlate);
            }


            DB.Users.Remove(currentUser);
            DB.SaveChanges();

            return RedirectToPage("Index");
        }
    }
}
