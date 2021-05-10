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
        public RatingDatabase Rating { get; set; }
        public RatingDatabase UserRated { get; set; }
        public double? avg { get; set; }

        public YourProfileModel(ILogger<YourProfileModel> logger, EmilDbContext db)
        {
            _logger = logger;
            DB = db;
        }

        public void OnGet(int id)
        {
            CurrentUser = DB.Users.Find(7);
            FoundUser = DB.Users.Find(id);
            
            var reviews = (from x in DB.RatingDatabases where x.UserRatedId.Equals(id) select x).ToList();
            avg = (from x in reviews select x.Rating).Average();
        }
    }
}
