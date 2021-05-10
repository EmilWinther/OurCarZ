using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OurCarZ.Model;

namespace OurCarZ.Pages.Rating
{
    public class RatingPageModel : PageModel
    {
        public EmilDbContext DB = new EmilDbContext();
        [BindProperty]
        public int UserToBeRated { get; set; }
        [BindProperty]
        public int UserRating { get; set; }
        [BindProperty]
        public int Rating { get; set; }
        public RatingPageModel(EmilDbContext db) 
        {
            DB = db;
        }
        public void OnGet()
        {
        }
        public IActionResult OnPost() 
        {
            //Making sure nothing is zero, because it will break the program if true. Luckily, the integers will also return 0
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
                    DB.RatingDatabases.Add(new RatingDatabase(UserRating, Rating, UserToBeRated));
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
