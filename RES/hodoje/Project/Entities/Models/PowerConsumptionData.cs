using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class PowerConsumptionData
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Column(TypeName = "datetime2")]
        public virtual DateTime Timestamp { get; set; }
        [Required]
        public virtual double Consumption { get; set; }
        [Required]
        public virtual string GeoAreaId { get; set; }
        [ForeignKey("GeoAreaId")]
        public virtual GeoArea GeoArea { get; set; }
    }
}
