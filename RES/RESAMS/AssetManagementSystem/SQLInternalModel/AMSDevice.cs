using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagementSystem.DB
{
    public class AMSDevice
    {
        [Required]
        public List<AMSChangeSet> Measurements { get; set; }
        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [Required]
        public string DeviceName { get; set; }

        [NotMapped]
        public bool IsAlarm { get; set; }

        //strani kljucevi za localcontroller
        public int? AMSLCId { get; set; }
        

        public AMSDevice()
        {
            Measurements = new List<AMSChangeSet>();
            IsAlarm = true;
        }
        public AMSDevice(string deviceName)
        {
            DeviceName = deviceName;
            Measurements = new List<AMSChangeSet>();
            IsAlarm = true;
        }
    }
}
