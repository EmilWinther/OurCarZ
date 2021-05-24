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
        [StringLength(28), MinLength(8, ErrorMessage = "Your password is too short"), MaxLength(28, ErrorMessage = "Your password is too long"), Compare(nameof(Password), ErrorMessage = "The passwords do not match")]
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
        [BindProperty]
        [RegularExpression(@"[1-9]")]
        public int? Seats { get; set; }

        [BindProperty]
        [RegularExpression(@"[0-2]+[0-9]+[0-9]+[0-9]")]
        public string? Year { get; set; }
        [BindProperty]
        [MinLength(1), MaxLength(30)]
        public string? Model { get; set; }
#nullable disable
        [BindProperty]
        public bool IsChecked { get; set; }
        Car newcar { get; set; }
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
                OnGet();
                return Page();
            }
            if (FirstName == null && LastName == null && PhoneNumber == null && Email == null && LicensePlate == null && Password == null && Model == null && Year == null && Seats == null)
            {
                OnGet();
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
            if (LicensePlate != null || Model != null || Year != null || Seats != null)
            {
                if (currentUser.LicensePlate != null)
                {
                    newcar = DB.Cars.Find(currentUser.LicensePlate);
                }
                else
                {
                    newcar = new Car();
                }
                if (LicensePlate.ToLower() == "delete")
                {
                }
                else if (LicensePlate != null)
                {
                    newcar.LicensePlate = LicensePlate;
                }
                if (Model != null)
                {
                    newcar.Model = Model;
                }
                if (Year != null)
                {
                    newcar.Year = Year;
                }
                if (Seats != null)
                {
                    newcar.Seats = Convert.ToInt32(Seats);
                }
                string shh = currentUser.LicensePlate;
                if (LicensePlate.ToLower() == "delete")
                {
                }
                else if (DB.Cars.Find(newcar.LicensePlate) == null)
                {
                    DB.Cars.Add(newcar);
                }
                else if (DB.Cars.Find(newcar.LicensePlate) != null)
                {
                    DB.Remove(DB.Cars.Find(newcar.LicensePlate));
                    DB.Cars.Add(newcar);
                }
                DB.SaveChanges();
                if (DB.Cars.Find(currentUser.LicensePlate) != null)
                {
                    DB.Cars.Remove(DB.Cars.Find(shh));
                }
                DB.SaveChanges();
                if (LicensePlate.ToLower() == "delete")
                {
                    currentUser.LicensePlate = null;
                }
                else
                {
                    currentUser.LicensePlate = newcar.LicensePlate;
                }
            }
            if (Password != null)
            {
                if (passwordHasher.VerifyHashedPassword(null, currentUser.Password, OldPassword) ==
                    PasswordVerificationResult.Success)
                {
                    currentUser.Password = passwordHasher.HashPassword(null, Password);
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