using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.DataAccess.ModelRepositoryInterfaces;
using DomainEntities;
using DomainEntities.Models;

namespace Backend.DataAccess.ModelRepositories
{
    public class UserRepository : Repository<User, int>, IUserRepository
    {
        protected DatabaseContext DatabaseContext
        {
            get { return _context as DatabaseContext; }
        }

        public UserRepository(DbContext context) : base(context) { }

        public User GetByIdIncludeAll(int id)
        {
            return _entities.Where(u => u.Id == id).Include(u => u.Car).Include(u => u.DriverLocation).FirstOrDefault();
        }

        public IEnumerable<User> GetAllIncludeAll()
        {
            return _entities.Include(u => u.Car).Include(u => u.DriverLocation).ToList();
        }

        public User GetUserByUsername(string username, string role)
        {            
           return _entities.Where(u => u.Username == username && ((Role)u.Role).ToString() == role).FirstOrDefault();
        }

        // Used only for Drivers
        public User GetUserByUsernameIncludeAll(string username, string role)
        {
            return _entities.Where(u => u.Username == username && ((Role) u.Role).ToString() == role)
                            .Include(u => u.Car)
                            .Include(u => u.DriverLocation).FirstOrDefault();
        }

        public IEnumerable<User> GetAllCustomers()
        {
            return _entities.Where(u => u.Role == (int) Role.CUSTOMER).ToList();
        }

        public IEnumerable<User> GetAllDrivers()
        {
            return _entities.Where(u => u.Role == (int)Role.DRIVER).ToList();
        }

        public IEnumerable<User> GetAllDispatchers()
        {
            return _entities.Where(u => u.Role == (int) Role.DISPATCHER).ToList();
        }

        public IEnumerable<User> GetAllDriversIncludeLocationAndCar()
        {
            return _entities.Where(u => u.Role == (int) Role.DRIVER).Include(u => u.Car).Include(u => u.DriverLocation).ToList();
        }
    }
}
