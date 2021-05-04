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
                DB.RatingDatabases.Add(new RatingDatabase(UserRating, Rating, UserToBeRated));
            }
            DB.SaveChanges();

            return Page();
        }
    }
}
