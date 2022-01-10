using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Backend.DataAccess.ModelRepositoryInterfaces;
using DomainEntities;
using DomainEntities.Models;

namespace Backend.DataAccess.ModelRepositories
{
    public class RideRepository : Repository<Ride, int>, IRideRepository
    {
        protected DatabaseContext DatabaseContext
        {
            get { return _context as DatabaseContext; }
        }

        public RideRepository(DbContext context) : base(context) { }

        public IEnumerable<Ride> FilterRidesIncludeAll(Expression<Func<Ride, bool>> predicate)
        {
            List<Ride> filteredRides = new List<Ride>();
            filteredRides = _entities.Where(predicate)
                                     .Include(r => r.StartLocation)
                                     .Include(r => r.DestinationLocation)
                                     .Include(r => r.Comments)
                                     .Include(r => r.Customer)
                                     .Include(r => r.Driver)
                                     .Include(r => r.Dispatcher)
                                     .ToList();
            return filteredRides;
        }

        public IEnumerable<Ride> GetAllRidesIncludeAll()
        {
            List<Ride> allRides = new List<Ride>();
            allRides = _entities.Include(r => r.StartLocation)
                                .Include(r => r.DestinationLocation)
                                .Include(r => r.Comments)
                                .Include(r => r.Customer)
                                .Include(r => r.Driver)
                                .Include(r => r.Dispatcher)
                                .ToList();
            return allRides;
        }
    }
}
