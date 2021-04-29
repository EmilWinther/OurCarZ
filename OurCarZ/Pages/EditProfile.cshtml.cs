using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using OurCarZ.Model;

namespace OurCarZ.Pages
{
    public class EditProfileModel : PageModel
    {
        private readonly ILogger<EditProfileModel> _logger;
        public EmilDbContext DB = new EmilDbContext();
        [BindProperty]
        public string FirstName { get; set; }
        [BindProperty]
        public string LastName { get; set; }
        [BindProperty]
        public User currentUser { get; set; }

        public EditProfileModel(ILogger<EditProfileModel> logger, EmilDbContext db)
        {
            _logger = logger;
            DB = db;
        }
        public void OnGet(int id)
        {
            currentUser = DB.Users.Find(id);
        }
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            //No persistence between OnGet and OnPost, load the currentUser to update:
            currentUser = DB.Users.Find(currentUser.UserId);

            //Change the desired values, we grab from the page:
            if (FirstName != null)
            {
                currentUser.FirstName = FirstName;
            }
            if (LastName != null)
            {
                currentUser.LastName = LastName;
            }
            
            //Update the user. Finds the user based on the primary key (UserId). If a new primary key is somehow inserted, it makes a new user instead.
            DB.Users.Update(currentUser);
            DB.SaveChanges();

            //Go to profile for the specified user.
            return RedirectToPage("/Profile", new { id = currentUser.UserId});
        }
    }
}
