using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class DeviceChangeSets
    {
        public string DeviceId { get; set; }
        
        public List<ChangeSet> ChageSetList { get; set; }

        public DeviceChangeSets()
        {
            ChageSetList = new List<ChangeSet>();
        }
        public DeviceChangeSets(string id)
        {
            DeviceId = id;
            ChageSetList = new List<ChangeSet>();
        }
    }
}
