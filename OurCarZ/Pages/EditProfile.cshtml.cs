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
        [BindProperty(SupportsGet = true)]
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
            DB.Users.Update(currentUser);
            DB.SaveChanges();
            return RedirectToPage("/Profile?id=" + currentUser.UserId);
        }
    }
}
