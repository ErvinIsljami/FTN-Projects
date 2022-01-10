using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Backend.Models
{
    public class SubSearchByDateModel
    {
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }
}