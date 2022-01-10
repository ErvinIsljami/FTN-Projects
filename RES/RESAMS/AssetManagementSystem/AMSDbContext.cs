using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagementSystem.DB
{
    public class AMSDbContext : DbContext
    {
        public AMSDbContext() : base("RESAMS")
        {

        }

        public DbSet<AMSChangeSet> Measurements { get; set; }
        public DbSet<AMSDevice> Devices { get; set; }
        public DbSet<AMSLC> LocalControllers { get; set; }

        public override int SaveChanges()
        {
            int result = base.SaveChanges();
            foreach(AMSDevice device in Devices)
            {
                if (!MainWindow.DevicesCollection.Any(x => x.Id == device.Id))
                {
                    try
                    {

                        int alarmHours = Int32.Parse(ConfigurationManager.AppSettings["alarmValue"]);

                        AMSDbContext dbContext = new AMSDbContext();
                        var listOfMeasurements = dbContext.Measurements.Where(x => x.AMSDeviceId == device.Id);
                        var sortedList = listOfMeasurements.ToList();
                        sortedList.OrderBy(x => x.TimeStamp);

                        DateTime dateTimeLowest = new DateTime(1970, 1, 1, 0, 0, 0, 0);
                        dateTimeLowest = dateTimeLowest.AddSeconds(sortedList[0].TimeStamp);

                        DateTime dateTimeHighest = new DateTime(1970, 1, 1, 0, 0, 0, 0);
                        dateTimeHighest = dateTimeHighest.AddSeconds(sortedList[sortedList.Count - 1].TimeStamp);

                        if ((dateTimeHighest - dateTimeLowest).TotalHours > alarmHours)
                        {
                            device.IsAlarm = true;
                        }
                        else
                        {
                            device.IsAlarm = false;
                        }

                        MainWindow.DevicesCollection.Add(device);
                    }
                    catch { }

                }
            }

            return result;
        }

    }
}
