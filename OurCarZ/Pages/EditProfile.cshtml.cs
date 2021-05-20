
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
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
        private PasswordHasher<string> passwordHasher = new PasswordHasher<string>();
        [BindProperty]
        public User currentUser { get; set; }
#nullable enable
        [BindProperty]
        [StringLength(50)]
        public string? FirstName { get; set; }
        [BindProperty]
        [StringLength(50)]
        public string? LastName { get; set; }
        [BindProperty]
        [StringLength(28), MinLength(8, ErrorMessage = "Your password is too short"), MaxLength(28, ErrorMessage = "Your password is too long"), RegularExpression(@"(?!^[0-9]*$)(?!^[a-zA-Z]*$)^([a-zA-Z0-9]{8,28})$",
        ErrorMessage = "Password must contain one uppercase, one lowercase, one number, and be atleast 8 characters long")]
        public string? Password { get; set; }
        [BindProperty]
        [StringLength(28), MinLength(8, ErrorMessage = "Your password is too short"), MaxLength(28, ErrorMessage = "Your password is too long"), Compare(nameof(Password), ErrorMessage = "The passwords does not match")]
        public string? ConfirmPassword { get; set; }
        [BindProperty]
        [Required(ErrorMessage = "Please re-enter your current password here to update values."), StringLength(28), MinLength(8, ErrorMessage = "Your password is too short"), MaxLength(28, ErrorMessage = "Your password is too long")]
        public string? OldPassword { get; set; }
        [BindProperty]
        [StringLength(8), MinLength(8, ErrorMessage = "Your phonenumber should be 8 digits"), MaxLength(8, ErrorMessage = "Your phonenumber should be 8 digits"), Phone]
        public string? PhoneNumber { get; set; }
        [BindProperty]
        [MinLength(6), MaxLength(30), RegularExpression(@"^[a-zA-Z0-9_.+-]+@(edu.easj.dk)+$", ErrorMessage = "Invalid email format, use school mails")]
        public string? Email { get; set; }
        [BindProperty]
        [StringLength(7)]
        public string? LicensePlate { get; set; }
#nullable disable
        public EditProfileModel(ILogger<EditProfileModel> logger, EmilDbContext db)
        {
            _logger = logger;
            DB = db;
        }
        public void OnGet()
        {
            currentUser = UserPages.LogInPageModel.LoggedInUser;
        }
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            if (FirstName == null && LastName == null && PhoneNumber == null && Email == null && LicensePlate == null && Password == null)
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
            if (Password != null)
            { 
                if (passwordHasher.VerifyHashedPassword(null, currentUser.Password, OldPassword) ==
                    PasswordVerificationResult.Success)
                {
                    currentUser.Password = passwordHasher.HashPassword(null, Password);
                    currentUser.ConfirmPassword = passwordHasher.HashPassword(null, ConfirmPassword); ;
                }
            }
            
            //Update the user. Finds the user based on the primary key (UserId). If a new primary key is somehow inserted, it makes a new user instead.
            DB.Users.Update(currentUser);
            DB.SaveChanges();

            //Go to profile for the specified user.
            return RedirectToPage("/Profile");
        }
    }
}
