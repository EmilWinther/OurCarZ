using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using OurCarZ.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OurCarZ.Pages.UserPages;

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

        public void OnGet()
        {

            if (LogInPageModel.LoggedInUser != null)
            {
                currentUser = DB.Users.Find(UserPages.LogInPageModel.LoggedInUser.UserId);

                var reviews = (from x in DB.RatingDatabases where x.UserRatedId.Equals(currentUser) select x).ToList();
                avg = (from x in reviews select x.Rating).Average();
            }

        }


        public IActionResult OnPost(int DeleteID)
        {
            currentUser = DB.Users.Find(DeleteID);


            var RatingUser = DB.RatingDatabases.Where(x => x.UserRatedId == DeleteID || x.UserRatingId == DeleteID)
                .ToList();
            foreach (var rating in RatingUser)
            {
                DB.RatingDatabases.Remove(rating);
            }

            var MessageUser = DB.Messages.Where(x => x.MessageFrom == DeleteID || x.MessageTo == DeleteID).ToList();
            foreach (var message in MessageUser)
            {
                DB.Messages.Remove(message);
            }

            var RouteUser = DB.UserRoutes.Where(x => x.UserId == DeleteID).ToList();
            foreach (var user in RouteUser)
            {
                DB.UserRoutes.Remove(user);
            }

            var RouteUser2 = DB.Routes.Where(x => x.UserId == DeleteID).ToList();
            foreach (var user2 in RouteUser2)
            {
                DB.Routes.Remove(user2);
            }


            if (currentUser.LicensePlate != null)
            {
                var UserLicensePlate = DB.Cars.Where(x => x.LicensePlate == currentUser.LicensePlate).First();
                DB.Cars.Remove(UserLicensePlate);
            }


            DB.Users.Remove(currentUser);
            DB.SaveChanges();

            return RedirectToPage("/UserPages/LogOutPage");
        }
    }
}
