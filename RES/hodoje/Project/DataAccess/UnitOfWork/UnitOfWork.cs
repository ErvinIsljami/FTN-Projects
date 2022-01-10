using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.ModelRepositories;

namespace DataAccess
{
    // Unit of work is speficif to our application
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DatabaseContext _context;

        public UnitOfWork() { }

        // Note that a user of UnitOfWork will pass a context and it will be used in all of the repositories
        public UnitOfWork(DatabaseContext context)
        {
            _context = context;

            PowerConsumptionDataRepository = new PowerConsumptionDataRepository(_context);
            GeoAreaRepository = new GeoAreaRepository(_context);
        }

        public virtual IPowerConsumptionDataRepository PowerConsumptionDataRepository { get; private set; }
        public virtual IGeoAreaRepository GeoAreaRepository { get; private set; }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
