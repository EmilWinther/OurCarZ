using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using OurCarZ.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using OurCarZ.Pages.UserPages;

namespace OurCarZ.Pages
{
    public class SendMessageModel : PageModel
    {
        private readonly ILogger<SendMessageModel> _logger;

        public EmilDbContext DB = new EmilDbContext();
        [BindProperty]
        public User CurrentUser { get; set; }
        [BindProperty]
        public User FoundUser { get; set; }
        public List<User> UserList { get; set; }
        public Message message { get; set; }

        [BindProperty]
        public int MessageFrom { get; set; }
        [BindProperty]
        public string MessageText { get; set; }
        [BindProperty]
        public int MessageTo { get; set; }
        [BindProperty]
        public DateTime DateTime { get; set; }

        public SendMessageModel(ILogger<SendMessageModel> logger, EmilDbContext db)
        {
            _logger = logger;
            DB = db;
        }
        public void OnGet(int id, int id2)
        {
            CurrentUser = DB.Users.Find(id);
            FoundUser = DB.Users.Find(id2);

            if (FoundUser.UserId.Equals(CurrentUser.UserId))
            {
                FoundUser = DB.Users.Find(LogInPageModel.LoggedInUser.UserId);
            }
            else
            {
                FoundUser = DB.Users.Find(id2);
            }

            UserList = DB.Users.ToList();
        }

        public IActionResult OnPost(int id, int id2)
        {
            CurrentUser = DB.Users.Find(id);
            FoundUser = DB.Users.Find(id2);
            if (FoundUser.UserId.Equals(CurrentUser.UserId))
            {
                FoundUser = DB.Users.Find(LogInPageModel.LoggedInUser.UserId);
            }
            else
            {
                FoundUser = DB.Users.Find(id2);
            }

            Message msg = new Message();
            msg.MessageFrom = CurrentUser.UserId;

            if (MessageText == null)
            {
                msg.MessageText = "This is an empty message";
            }
            else
            {
                msg.MessageText = MessageText;
            }

            msg.DateTime = DateTime.Now;
            msg.MessageTo = FoundUser.UserId;

            DB.Messages.Add(msg);
            DB.SaveChanges();

            return RedirectToPage("/Profile", new { id = CurrentUser.UserId });
        }
    }
}
