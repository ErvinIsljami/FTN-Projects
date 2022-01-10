using Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalController.DB
{
    public class XmlDbHelper
    {
        private object padlock = new object();
        private XmlReaderWriter readerWriter;
        private string path;
        private AMSProxy proxy;
        private Task task;

        public XmlDbHelper(string path)
        {
            readerWriter = new XmlReaderWriter();
            this.path = path;
            proxy = new AMSProxy();
            task = new Task(() => SendToAMS());
            task.Start();
        }

        public LocalControllerDataModel GetAllChangeSets()
        {
            lock(padlock)
            {
                LocalControllerDataModel dbMeasurements = readerWriter.DeserializeObject<LocalControllerDataModel>(path);
                if (dbMeasurements == null)
                {
                    long ticks = DateTime.Now.Ticks;
                    dbMeasurements = new LocalControllerDataModel(ControllerOperations.LcName);
                }

                return dbMeasurements;
            }
        }

        public void SetAllMeasurements(LocalControllerDataModel newData)
        {
            lock (padlock)
            { 
                readerWriter.SerializeObject<LocalControllerDataModel>(newData, path);
            }
        }

        public void DeleteXmlFile()
        {
            if (File.Exists(path))
            {
                while (true)
                {
                    lock (padlock)
                    {
                        try
                        {
                            File.Delete(path);
                            break;
                        }
                        catch
                        {

                        }
                    }
                }
            }
        }

        public void AddNewLDMeasurements(Guid id, List<ChangeSet> measurements)
        {
            LocalControllerDataModel dbMeasurements = GetAllChangeSets();

            if(!dbMeasurements.DevicesChanges.Any(x => x.DeviceId == id.ToString()))
            {
                dbMeasurements.DevicesChanges.Add(new DeviceChangeSets(id.ToString()));
            }

            dbMeasurements.DevicesChanges.First(x => x.DeviceId == id.ToString()).ChageSetList.AddRange(measurements);

            SetAllMeasurements(dbMeasurements);
        }

        private async void SendToAMS()
        {
            while(true)
            {
                int waitPeriod = Int32.Parse(ConfigurationManager.AppSettings["sendingInterval"]);
                await Task.Delay(waitPeriod * 1000);

                LocalControllerDataModel dbMeasurements = GetAllChangeSets();
                proxy.Proxy.SendMeasurements(dbMeasurements);
                DeleteXmlFile();
            }
        }
    }
}
