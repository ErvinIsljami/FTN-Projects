using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Backend.Models
{
    public class SearchRidesModel
    {
        public SubSearchByDateModel ByDate { get; set; }
        public SubSearchByRatingModel ByRating { get; set; }
        public SubSearchByPriceModel ByPrice { get; set; }
    }
}