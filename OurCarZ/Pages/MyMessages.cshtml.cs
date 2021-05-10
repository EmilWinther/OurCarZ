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
    public class MessageModel : PageModel
    {
        private readonly ILogger<ProfileModel> _logger;

        public EmilDbContext DB = new EmilDbContext();
        [BindProperty]
        public User currentUser { get; set; }

        public User FoundUser { get; set; }

        public List<User> UserList { get; set; }
        public Message msg { get; set; }

        public MessageModel(ILogger<ProfileModel> logger, EmilDbContext db)
        {
            _logger = logger;
            DB = db;
        }


        public void OnGet(int id)
        {
            currentUser = DB.Users.Find(id);
            UserList = DB.Users.ToList();
        }

        public IActionResult OnPost(int id, int id2, int DeleteID)
        {
            currentUser = DB.Users.Find(id);
            FoundUser = DB.Users.Find(id2);


            msg = DB.Messages.Find(DeleteID);

            DB.Messages.Remove(msg);
            DB.SaveChanges();

            return RedirectToPage("/Profile", new { id = currentUser.UserId });
        }

    }
}
