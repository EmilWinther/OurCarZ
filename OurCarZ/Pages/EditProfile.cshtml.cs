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
        public string Password { get; set; }
        [BindProperty]
        public string ConfirmPassword { get; set; }
        [BindProperty]
        public User currentUser { get; set; }
        [BindProperty]
        public string OldPassword { get; set; }
        [BindProperty]
        public string PhoneNumber { get; set; }
        [BindProperty]
        public string Email { get; set; }
        [BindProperty]
        public string LicensePlate { get; set; }

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
            if (OldPassword == currentUser.Password && Password == ConfirmPassword)
            {
                currentUser.Password = Password;
            }
            if (FirstName != null)
            {
                currentUser.FirstName = FirstName;
            }
            if (LastName != null)
            {
                currentUser.LastName = LastName;
            }
            if (PhoneNumber != null)
            {
                currentUser.PhoneNumber = PhoneNumber;
            }
            if (Email != null)
            {
                currentUser.Email = Email;
            }
            if (LicensePlate != null)
            {
                currentUser.LicensePlate = LicensePlate;
            }


            //Update the user. Finds the user based on the primary key (UserId). If a new primary key is somehow inserted, it makes a new user instead.
            DB.Users.Update(currentUser);
            DB.SaveChanges();

            //Go to profile for the specified user.
            return RedirectToPage("/Profile", new { id = currentUser.UserId});
        }
    }
}