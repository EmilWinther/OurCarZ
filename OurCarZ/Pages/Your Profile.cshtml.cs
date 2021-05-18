using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using OurCarZ.Model;
using OurCarZ.Pages.UserPages;
using System.Linq;

namespace OurCarZ.Pages
{
    public class YourProfileModel : PageModel
    {
        private readonly ILogger<YourProfileModel> _logger;
        public EmilDbContext DB = new EmilDbContext();
        public User FoundUser { get; set; }
        public User CurrentUser { get; set; }
        public RatingDatabase Rating { get; set; }
        public RatingDatabase UserRated { get; set; }
        public double? avg { get; set; }

        public YourProfileModel(ILogger<YourProfileModel> logger, EmilDbContext db)
        {
            _logger = logger;
            DB = db;
        }

        public void OnGet(int id)
        {
            if (LogInPageModel.LoggedInUser != null)
            {
                CurrentUser = LogInPageModel.LoggedInUser;
                FoundUser = DB.Users.Find(id);

                var reviews = (from x in DB.RatingDatabases where x.UserRatedId.Equals(id) select x).ToList();
                avg = (from x in reviews select x.Rating).Average();
            }

        }
    }
}
