using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagementSystem.DB
{
    public class AMSChangeSet
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public double Value { get; set; }

        [Required]
        public long TimeStamp { get; set; }

        //foreign key
        //strani kljucevi za internalDevice
        public int? AMSDeviceId { get; set; }

        public AMSChangeSet()
        {

        }

        public AMSChangeSet(double value, long timeStamp)
        {
            Value = value;
            TimeStamp = timeStamp;
        }
    }
}
