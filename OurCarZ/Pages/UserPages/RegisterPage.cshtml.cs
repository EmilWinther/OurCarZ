using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OurCarZ.Model;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
using System;

namespace OurCarZ.Pages.UserPages
{
    public class RegisterPageModel : PageModel
    {
        private EmilDbContext DB = new EmilDbContext();
        private PasswordHasher<string> passwordHasher = new PasswordHasher<string>();

        public RegisterPageModel(EmilDbContext userDb)
        {
            DB = userDb;
        }

        [BindProperty]
        public User currentUser { get; set; }
#nullable enable
        [BindProperty]
        [Required(ErrorMessage = "First Name is required"), StringLength(50)]
        public string? FirstName { get; set; }
        [BindProperty]
        [Required(ErrorMessage = "Last Name is required"), StringLength(50)]
        public string? LastName { get; set; }
        [BindProperty]
        [Required(ErrorMessage = "Password is required"), StringLength(28), MinLength(8, ErrorMessage = "Your password is too short"), MaxLength(28, ErrorMessage = "Your password is too long"), RegularExpression(@"(?!^[0-9]*$)(?!^[a-zA-Z]*$)^([a-zA-Z0-9]{8,28})$",
        ErrorMessage = "Password must contain one uppercase, one lowercase, one number, and be atleast 8 characters long")]
        public string? Password { get; set; }
        [BindProperty]
        [StringLength(28), MinLength(8, ErrorMessage = "Your password is too short"), MaxLength(28, ErrorMessage = "Your password is too long"), Required, Compare(nameof(Password), ErrorMessage = "The passwords does not match")]
        public string? ConfirmPassword { get; set; }
        [BindProperty]
        [Required(ErrorMessage = "Phone Number is required."), StringLength(8), MinLength(8, ErrorMessage = "Your phonenumber should be 8 digits"), MaxLength(8, ErrorMessage = "Your phonenumber should be 8 digits"), Phone]
        public string? PhoneNumber { get; set; }
        [BindProperty]
        [Required(ErrorMessage = "Email is required."), MinLength(6), MaxLength(30), RegularExpression(@"^[a-zA-Z0-9_.+-]+@(edu.easj.dk)+$", ErrorMessage = "Invalid email format, use school mails")]
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
        public bool IsChecked {get; set;}
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            if (IsChecked == true && LicensePlate != "on" && Model != null && Year != null && Seats != null) 
            {
                Car thiscar = new Car();
                thiscar.LicensePlate = LicensePlate;
                thiscar.Model = Model;
                thiscar.Year = Year;
                thiscar.Seats = Convert.ToInt32(Seats);
                DB.Cars.Add(thiscar);
                DB.SaveChanges();
            }
            currentUser = new User();
            currentUser.FirstName = FirstName;
            currentUser.LastName = LastName;
            currentUser.PhoneNumber = PhoneNumber;
            currentUser.Email = Email;
            if (IsChecked == true) 
            {
                currentUser.LicensePlate = LicensePlate;
            }
            currentUser.Password = passwordHasher.HashPassword(null, Password);

            DB.Users.Add(currentUser);
            DB.SaveChanges();

            return RedirectToPage("/Index");

        }
    }
}
