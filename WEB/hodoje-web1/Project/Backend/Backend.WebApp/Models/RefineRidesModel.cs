using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Backend.Models
{
    public class RefineRidesModel
    {
        public string Filter { get; set; }
        public SortRidesModel Sort { get; set; }
        public SearchRidesModel Search { get; set; }
    }
}