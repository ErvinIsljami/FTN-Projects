using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class GeoArea
    {
        [Key]
        public string Id { get; set; }
        [Required]
        public virtual string Name { get; set; }
        public virtual ICollection<PowerConsumptionData> PowerConsumptionDatas { get; set; }
    }
}
