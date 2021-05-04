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
    public class YourProfileModel : PageModel
    {
        private readonly ILogger<YourProfileModel> _logger;
        public EmilDbContext DB = new EmilDbContext();
        public User FoundUser { get; set; }
        public User CurrentUser { get; set; }

        public YourProfileModel(ILogger<YourProfileModel> logger, EmilDbContext db)
        {
            _logger = logger;
            DB = db;
        }

        public void OnGet(int id)
        {
            FoundUser = DB.Users.Find(id);
        }
        public void OnPost()
        {
            Redirect ("Index");
        }
    }
}
