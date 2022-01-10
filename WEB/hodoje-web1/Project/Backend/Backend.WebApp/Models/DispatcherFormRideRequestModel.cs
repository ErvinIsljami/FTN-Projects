using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Backend.Dtos;

namespace Backend.Models
{
    public class DispatcherFormRideRequestModel
    {
        public LocationDto Location { get; set; }
        public string CarType { get; set; }
        public int DriverId { get; set; }
        public int DispatcherId { get; set; }
    }
}