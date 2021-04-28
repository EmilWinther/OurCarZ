using OurCarZ.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OurCarZ.Services
{
    public class UserPersistence : IUserPersistence
    {
        private EmilDbContext edb = new EmilDbContext();


        public void Create(User user)
        {
            edb.Users.Add(user);
            edb.SaveChanges();
        }

        public void Delete(int id)
        {
            User user = GetOne(id);
            edb.Users.Remove(user);
            edb.SaveChanges();
        }

        public List<User> GetAll()
        {
            return edb.Users.ToList();
        }

        public User GetOne(int id)
        {
            return edb.Users.Find(id);
        }

        public void Update(int id, User updatedUser)
        {
            User user = GetOne(id);
            user.UserId = updatedUser.UserId;
            user.FirstName = updatedUser.FirstName;
            user.LastName = updatedUser.LastName;
            user.PhoneNumber = updatedUser.PhoneNumber;
        }
    }
}
