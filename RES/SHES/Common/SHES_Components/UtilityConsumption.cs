using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class UtilityConsumption
    {
        public DateTime TimeStamp { get; set; }
        [Key]
        public int Id { get; set; }
        public double Value { get; set; }
        public string ComponentId { get; set; }
        
        public UtilityConsumption()
        {

        }

        public UtilityConsumption(double value, string componentId)
        {
            ComponentId = componentId;
            Value = value;
        }
    }
}
