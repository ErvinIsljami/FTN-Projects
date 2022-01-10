using QueueService.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueService
{
    public class DbModel
    {
        [Key]
        public string UserID { get; set; }

    }
}
