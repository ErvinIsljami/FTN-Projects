using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainEntities;
using DomainEntities.Models;

namespace Backend.DataAccess.ModelRepositoryInterfaces
{
    public interface IUserRepository : IRepository<User, int>
    {
        User GetByIdIncludeAll(int id);
        IEnumerable<User> GetAllIncludeAll();
        User GetUserByUsername(string username, string role);
        User GetUserByUsernameIncludeAll(string username, string role);
        IEnumerable<User> GetAllCustomers();
        IEnumerable<User> GetAllDrivers();
        IEnumerable<User> GetAllDispatchers();
        IEnumerable<User> GetAllDriversIncludeLocationAndCar();
    }
}
