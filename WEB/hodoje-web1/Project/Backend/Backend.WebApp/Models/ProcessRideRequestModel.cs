using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Backend.Dtos;

namespace Backend.Models
{
    public class ProcessRideRequestModel
    {
        public int DriverId { get; set; }
        public int DispatcherId { get; set; }
        public int RideId { get; set; }
    }
}