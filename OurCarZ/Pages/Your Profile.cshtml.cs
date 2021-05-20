using Microsoft.AspNetCore.Mvc;
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
        public RatingDatabase Rating { get; set; }
        public RatingDatabase UserRated { get; set; }
        public double? avg { get; set; }
        [BindProperty]
        public int UsersRating { get; set; }
        [BindProperty]
        public User FoundUser { get; set; }
        [BindProperty]
        public User CurrentUser { get; set; }

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
        public IActionResult OnPostRate() 
        {
            //Making sure nothing is zero, because it will break the program if so. Luckily, the integers will also return 0
            //if an illegal character is input on the page (such as letters)
            if (FoundUser.UserId != 0 && CurrentUser.UserId != 0 && UsersRating != 0)
            {
                //Check if the specified Composite Key already exists in the Database and update it
                if (DB.RatingDatabases.Find(FoundUser.UserId, CurrentUser.UserId) != null)
                {
                    var Rater = DB.RatingDatabases.FirstOrDefault(b => b.UserRatedId == FoundUser.UserId && b.UserRatingId == CurrentUser.UserId);
                    Rater.Rating = UsersRating;
                    DB.RatingDatabases.Update(Rater);
                }
                //Else, add a new entry to the database.
                else
                {
                    RatingDatabase newRating = new RatingDatabase();
                    newRating.UserRatedId = FoundUser.UserId;
                    newRating.UserRatingId = CurrentUser.UserId;
                    newRating.Rating = UsersRating;
                    DB.RatingDatabases.Add(newRating);
                }
                DB.SaveChanges();
            }
            OnGet(FoundUser.UserId);
            return Page();
        }
    }
}
