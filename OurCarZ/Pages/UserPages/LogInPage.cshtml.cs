using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OurCarZ.Model;
using OurCarZ.Services;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OurCarZ.Pages.UserPages
{
    public class LogInPageModel : PageModel
    {
        public static User LoggedInUser { get; set; } = null;
        private IUserPersistence _userPersistence;
        [BindProperty]
        public string Email { get; set; }
        [BindProperty, DataType(DataType.Password)]
        public string Password { get; set; }
        [BindProperty]
        public int UserId { get; set; }
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

            List<User> users = _userPersistence.GetAll();
            foreach (User user in users)
            {
                if (Email == user.Email && Password == user.Password)
                {
                    LoggedInUser = user;

                    var claims = new List<Claim> { new Claim(ClaimTypes.Email, Email) };
                    if (LoggedInUser.UserId == 35) claims.Add(new Claim(ClaimTypes.Role, "admin"));

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                    return RedirectToPage("/index");
                }

            }
            Message = "Invalid attempt";
            return Page();
        }

    }
}
