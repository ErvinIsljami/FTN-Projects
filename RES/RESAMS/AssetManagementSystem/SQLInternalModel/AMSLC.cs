using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagementSystem.DB
{
    public class AMSLC
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LocalControllerId { get; set; }
        
        [Required]
        public string LocalControllerName { get; set; }

        public List<AMSDevice> Devices { get; set; }

        public AMSLC()
        {
            Devices = new List<AMSDevice>();
        }

        public AMSLC(string name)
        {
            LocalControllerName = name;
            Devices = new List<AMSDevice>();
        }

    }
}
