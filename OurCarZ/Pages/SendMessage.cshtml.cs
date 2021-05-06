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
    public class SendMessageModel : PageModel
    {
        private readonly ILogger<SendMessageModel> _logger;

        public EmilDbContext DB = new EmilDbContext();
        [BindProperty]
        public User CurrentUser { get; set; }
        public List<User> UserList { get; set; }
        public Message  message { get; set; }

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
        public void OnGet(int id)
        {
            CurrentUser = DB.Users.Find(id);
            UserList = DB.Users.ToList();
        }

        public IActionResult OnPost(int id)
        {
            
            CurrentUser = DB.Users.Find(id);
            MessageFrom = CurrentUser.UserId;
            DateTime = DateTime.Now;

            DB.Messages.Add(new Message(MessageFrom, DateTime, MessageTo));
            DB.SaveChanges();

            
            return RedirectToPage("/MyMessages");
        }
    }
}
