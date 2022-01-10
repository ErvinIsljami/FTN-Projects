using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using Entities.Models;

namespace DataAccess.ModelRepositories
{
    public class PowerConsumptionDataRepository : Repository<PowerConsumptionData, int>, IPowerConsumptionDataRepository
    {
        // In future if we'd use the context, we'd need to cast it all the time, so this property is for that
        public DatabaseContext DatabaseContext
        {
            get { return _context as DatabaseContext; }
        }

        public PowerConsumptionDataRepository(DatabaseContext context) : base(context) { }
    }
}
