using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OurCarZ.Model;
using OurCarZ.Services;
using System.ComponentModel.DataAnnotations;

namespace OurCarZ.Pages.UserPages
{
    public class RegisterPageModel : PageModel
    {
        private IUserPersistence _userDb;

        public RegisterPageModel(IUserPersistence userDb)
        {
            _userDb = userDb;
        }

        [BindProperty]
        public User NewUser { get; set; }
        #nullable enable
        [BindProperty]
        public string? ConfirmPass { get; set; }
        
        public bool comparePasswords(string? pass, string? confirm) 
        {
            bool comparefavorably;
            if (pass == confirm)
            {
                comparefavorably = true;
            }
            else 
            {
                comparefavorably = false;
            }
            return comparefavorably;
        }
        #nullable disable
        public IActionResult OnPost()
        {
            if(ModelState.IsValid)
            {
                if (comparePasswords(NewUser.Password, ConfirmPass)) 
                {
                    _userDb.Create(NewUser);
                    return RedirectToPage("/Index");
                }
                
            }
            return Page();
        }
    }
}
