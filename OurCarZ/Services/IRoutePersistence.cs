using OurCarZ.Model;
using System.Collections.Generic;

namespace OurCarZ.Services
{
    public interface IRoutePersistence
    {
        List<Route> GetAll();
        public Route GetOne(int id);
        public void Create(Route route);
        public void Delete(int id);
        public void Update(int id, Route updatedRoute);
    }
}
