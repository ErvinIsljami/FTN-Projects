using Entities.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext()
        {
            //this.Configuration.LazyLoadingEnabled = false;
        }

        public virtual DbSet<PowerConsumptionData> PowerConsumptionDataSet { get; set; }
        public virtual DbSet<GeoArea> GeoAreaSet { get; set; }
    }
}
