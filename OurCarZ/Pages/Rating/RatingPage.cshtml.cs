using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OurCarZ.Model;
using System.Linq;

namespace OurCarZ.Pages.Rating
{
    public class RatingPageModel : PageModel
    {
        public EmilDbContext DB = new EmilDbContext();
        public User CurrentUser { get; set; }
        public int UserRating { get; set; }
        [BindProperty]
        public int Rating { get; set; }
        [BindProperty]
        public int UserToBeRated { get; set; }
        public RatingPageModel(EmilDbContext db)
        {
            DB = db;
        }
        public void OnGet()
        {
            if (UserPages.LogInPageModel.LoggedInUser != null)
            {
                CurrentUser = DB.Users.Find(UserPages.LogInPageModel.LoggedInUser.UserId);
                UserRating = CurrentUser.UserId;
            }
        }
        public IActionResult OnPost()
        {
            //Making sure nothing is zero, because it will break the program if so. Luckily, the integers will also return 0
            //if an illegal character is input on the page (such as letters)
            if (UserToBeRated != 0 && UserRating != 0 && Rating != 0)
            {
                //Check if the specified Composite Key already exists in the Database and update it
                if (DB.RatingDatabases.Find(UserToBeRated, UserRating) != null)
                {
                    var Rater = DB.RatingDatabases.FirstOrDefault(b => b.UserRatedId == UserToBeRated && b.UserRatingId == UserRating);
                    Rater.Rating = Rating;
                    DB.RatingDatabases.Update(Rater);
                }
                //Else, add a new entry to the database.
                else
                {
                    RatingDatabase newRating = new RatingDatabase();
                    newRating.Rating = Rating;
                    newRating.UserRatedId = UserToBeRated;
                    newRating.UserRatingId = UserRating;
                    DB.RatingDatabases.Add(newRating);
                }
                //When in doubt, Try / Catch! This is here for when an incorrect UserID is input either as the Rater or the Ratee. 
                //Of course, it catches other errors too (That aren't already specified above! Those *SHOULD* already be handled)
                try
                {
                    DB.SaveChanges();
                }
                catch
                {
                    return RedirectToPage("../Index");
                }
            }
            return Page();
        }
    }
}
