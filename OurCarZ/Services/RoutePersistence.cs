using OurCarZ.Model;
using System.Collections.Generic;
using System.Linq;

namespace OurCarZ.Services
{
    public class RoutePersistence : IRoutePersistence
    {
        private EmilDbContext edb = new EmilDbContext();

        public void Create(Route route)
        {
            edb.Routes.Add(route);
            edb.SaveChanges();
        }

        public void Delete(int id)
        {
            Route route = GetOne(id);
            edb.Routes.Remove(route);
            edb.SaveChanges();
        }

        public List<Route> GetAll()
        {
            return edb.Routes.ToList();
        }

        public Route GetOne(int id)
        {
            return edb.Routes.Find(id);
        }

        public void Update(int id, Route updatedRoute)
        {
            Route route = GetOne(id);
            route.StartPoint = updatedRoute.StartPoint;
            route.FinishPoint = updatedRoute.FinishPoint;
            edb.SaveChanges();
        }
    }
}
