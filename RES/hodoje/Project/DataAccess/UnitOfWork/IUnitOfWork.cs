using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    // Unit of work is speficif to our application
    public interface IUnitOfWork : IDisposable
    {
        IPowerConsumptionDataRepository PowerConsumptionDataRepository { get; }
        IGeoAreaRepository GeoAreaRepository { get; }
        int Complete();
    }
}
