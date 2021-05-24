
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OurCarZ.Model;
using OurCarZ.Services;

namespace OurCarZ.Pages.UserPages
{
    public class LogInPageModel : PageModel
    {
        public static User LoggedInUser { get; set; }
        private readonly IUserPersistence _userPersistence;
        [BindProperty] public string Email { get; set; }

        [BindProperty]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [BindProperty] public int UserId { get; set; }
        public string Message { get; set; }

        public LogInPageModel(IUserPersistence userP)
        {
            _userPersistence = userP;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            var users = _userPersistence.GetAll();
            foreach (var user in users)
                if (Email == user.Email)
                {
                    var passwordHasher = new PasswordHasher<string>();
                    if (passwordHasher.VerifyHashedPassword(null, user.Password, Password) ==
                        PasswordVerificationResult.Success)
                    {
                        LoggedInUser = user;

                        var claims = new List<Claim> {new(ClaimTypes.Email, Email)};
                        if (LoggedInUser.UserId == 88) claims.Add(new Claim(ClaimTypes.Role, "admin"));

                        var claimsIdentity = new ClaimsIdentity(claims,
                            CookieAuthenticationDefaults.AuthenticationScheme);
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal(claimsIdentity));
                        return RedirectToPage("/index");
                    }
                }

            Message = "Invalid attempt";
            return Page();
        }
    }
}