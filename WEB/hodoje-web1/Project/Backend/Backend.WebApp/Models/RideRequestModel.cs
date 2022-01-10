using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Backend.Dtos;

namespace Backend.Models
{
    public class RideRequestModel
    {
        public LocationDto Location { get; set; }
        public string CarType { get; set; }
    }
}