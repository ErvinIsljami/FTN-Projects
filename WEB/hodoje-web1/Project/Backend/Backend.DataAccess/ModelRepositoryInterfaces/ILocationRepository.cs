using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainEntities;
using DomainEntities.Models;

namespace Backend.DataAccess.ModelRepositoryInterfaces
{
    public interface ILocationRepository : IRepository<Location, int>
    {
    }
}
