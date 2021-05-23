using OurCarZ.Model;
using System.Collections.Generic;
using System.Linq;

namespace OurCarZ.Services
{
    public class RoutePersistence : IRoutePersistence
    {
        private EmilDbContext _edb;
        public RoutePersistence(EmilDbContext edb) 
        {
            _edb = edb;
        }
        public void Create(Route route)
        {
            _edb.Routes.Add(route);
            _edb.SaveChanges();
        }

        public void Delete(int id)
        {
            Route route = GetOne(id);
            _edb.Routes.Remove(route);
            _edb.SaveChanges();
        }

        public List<Route> GetAll()
        {
            return _edb.Routes.ToList();
        }

        public Route GetOne(int id)
        {
            return _edb.Routes.Find(id);
        }

        public void Update(int id, Route updatedRoute)
        {
            Route route = GetOne(id);
            route.StartPoint = updatedRoute.StartPoint;
            route.FinishPoint = updatedRoute.FinishPoint;
            _edb.SaveChanges();
        }
    }
}
