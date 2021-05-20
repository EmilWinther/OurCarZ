using OurCarZ.Model;
using System.Collections.Generic;
using System.Linq;

namespace OurCarZ.Services
{
    public class UserPersistence : IUserPersistence
    {
        private EmilDbContext _edb;
        public UserPersistence(EmilDbContext edb)
        {
            _edb = edb;
        }

        public void Create(User user)
        {
            _edb.Users.Add(user);
            _edb.SaveChanges();
        }

        public void Delete(int id)
        {
            User user = GetOne(id);
            _edb.Users.Remove(user);
            _edb.SaveChanges();
        }

        public List<User> GetAll()
        {
            return _edb.Users.ToList();
        }

        public User GetOne(int id)
        {
            return _edb.Users.Find(id);
        }

        public void Update(int id, User updatedUser)
        {
            User user = GetOne(id);
            user.UserId = updatedUser.UserId;
            user.FirstName = updatedUser.FirstName;
            user.LastName = updatedUser.LastName;
            user.PhoneNumber = updatedUser.PhoneNumber;
            _edb.SaveChanges();
            user.Email = updatedUser.Email;
            user.Password = updatedUser.Password;
        }
    }
}
