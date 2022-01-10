using Contracts;
using Common;
using LocalController.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalController.Services
{
    public class LDToLCMeasurements : IDeviceChangeSetService
    {
        public void SendNewMeasurement(Guid id, List<ChangeSet> measurement)
        {
            XmlDbHelper dbAccess = new XmlDbHelper("baza.xml");
            dbAccess.AddNewLDMeasurements(id, measurement);
        }
    }
}
