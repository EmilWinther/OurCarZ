using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OurCarZ.Model;

namespace OurCarZ.Pages
{
    public class MyMessagesModel : PageModel
    {
        private readonly EmilDbContext DB = new EmilDbContext();
        private readonly IConfiguration Configuration;

        public MyMessagesModel(EmilDbContext db, IConfiguration configuration)
        {
            DB = db;
            Configuration = configuration;
        }

        [BindProperty]
        public User currentUser { get; set; }
        public User FoundUser { get; set; }
        public Message msg { get; set; }
        public List<User> UserList { get; set; }
        public string CurrentSort { get; set; }

        public PaginatedList<Message> Messages { get; set; }

        public async Task OnGetAsync(string sortOrder, int? pageIndex, int id)
        {
            currentUser = DB.Users.Find(id);
            UserList = DB.Users.ToList();

            CurrentSort = sortOrder;

            IQueryable<Message> messageList = from s in DB.Messages
                                             select s;

            IQueryable<User> userList = from u in DB.Users
                select u;

            switch (sortOrder)
            {
                default:
                    messageList = messageList.Where(s => s.MessageFrom.Equals(currentUser.UserId)
                                                           || s.MessageTo.Equals(currentUser.UserId));
                    messageList = messageList.OrderByDescending(s => s.MessageId);
                    break;
            }

            var pageSize = Configuration.GetValue("PageSize", 10);
            Messages = await PaginatedList<Message>.CreateAsync(
                messageList.AsNoTracking(), pageIndex ?? 1, pageSize);
        }

        public IActionResult OnPost(int id, int id2, int DeleteID)
        {
            currentUser = DB.Users.Find(id);
            FoundUser = DB.Users.Find(id2);


            msg = DB.Messages.Find(DeleteID);

            DB.Messages.Remove(msg);
            DB.SaveChanges();

            return RedirectToPage("/MyMessages", new { id = currentUser.UserId });
        }
    }
}