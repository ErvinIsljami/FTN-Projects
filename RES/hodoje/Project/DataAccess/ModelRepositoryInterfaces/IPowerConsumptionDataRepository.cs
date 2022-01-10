using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using Entities.Models;

namespace DataAccess
{
    public interface IPowerConsumptionDataRepository : IRepository<PowerConsumptionData, int>
    {
        // Currentyle there is nothing here, but in future we can extend this repository
    }
}
