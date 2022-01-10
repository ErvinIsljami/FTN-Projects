using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Common
{
    [DataContract]
    public class LocalControllerDataModel
    {
        [DataMember]
        public string ControllerId { get; set; }
        [DataMember]
        public long TimeStamp { get; set; }
        [DataMember]
        public List<DeviceChangeSets> DevicesChanges { get; set; }

        public LocalControllerDataModel()
        {
            DevicesChanges = new List<DeviceChangeSets>();
            DateTimeConverter converter = new DateTimeConverter();
            TimeStamp = converter.ConvertToUnix(DateTime.Now);
        }

        public LocalControllerDataModel(string lcid)
        {
            DevicesChanges = new List<DeviceChangeSets>();
            ControllerId = lcid;
            DateTimeConverter converter = new DateTimeConverter();
            TimeStamp = converter.ConvertToUnix(DateTime.Now);
        }
    }
}
