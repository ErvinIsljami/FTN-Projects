using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProxy
{
    public interface IPowerConsumptionCachedData
    {
        IEnumerable<PowerConsumptionData> Get(InputDate inputDate);
    }
}
