using AssetManagementSystem.DB;
using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts;

namespace AssetManagementSystem.Services
{
    public class LCToAMSMeasurements : IControllerChangeSetService
    {
        public void SendMeasurements(LocalControllerDataModel dataModel)
        {
            AMSDbContext dbContext = new AMSDbContext();
            AMSLC lc = null;

            AMSDevice internalDevice = null;
            if (!dbContext.LocalControllers.Any(x => x.LocalControllerName == dataModel.ControllerId))
            {
                lc = new AMSLC(dataModel.ControllerId);
                dbContext.LocalControllers.Add(lc);
                dbContext.SaveChanges();
            }

            int lcId = dbContext.LocalControllers.First(k => k.LocalControllerName == dataModel.ControllerId).LocalControllerId;

            dataModel.DevicesChanges.ForEach(x =>
            {
                int deviceId = 0;
                if (!dbContext.Devices.Any(y => x.DeviceId == y.DeviceName))
                {
                    internalDevice = new AMSDevice(x.DeviceId) { AMSLCId = lcId };
                    dbContext.Devices.Add(internalDevice);
                    dbContext.SaveChanges();
                    deviceId = internalDevice.Id;
                }
                deviceId = dbContext.Devices.First(k => k.DeviceName == x.DeviceId).Id;
                x.ChageSetList.ForEach(a => dbContext.Measurements.Add(new AMSChangeSet(a.Value, a.TimeStamp) { AMSDeviceId = deviceId }));
                dbContext.SaveChanges();
            }
            );
            dbContext.SaveChanges();
        }
    }
}
