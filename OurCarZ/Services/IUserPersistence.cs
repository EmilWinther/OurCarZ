using OurCarZ.Model;
using System.Collections.Generic;

namespace OurCarZ.Services
{
    public interface IUserPersistence
    {
        List<User> GetAll();
        public User GetOne(int id);
        public void Create(User user);
        public void Delete(int id);
        public void Update(int id, User updatedUser);
    }
}
