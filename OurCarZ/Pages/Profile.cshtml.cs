using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using OurCarZ.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OurCarZ.Pages
{
    public class ProfileModel : PageModel
    {
        private readonly ILogger<ProfileModel> _logger;
        public EmilDbContext DB = new EmilDbContext();
        public User currentUser { get; set; }

        public ProfileModel(ILogger<ProfileModel> logger, EmilDbContext db)
        {
            _logger = logger;
            DB = db;
        }

        public void OnGet(int id)
        {
            currentUser = DB.Users.Find(id);
        }
    }
}
