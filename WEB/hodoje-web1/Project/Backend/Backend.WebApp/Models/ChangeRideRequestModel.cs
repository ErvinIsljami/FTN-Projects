using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Backend.Dtos;
using DomainEntities.Models;

namespace Backend.Models
{
    public class ChangeRideRequestModel
    {
        public LocationDto Location { get; set; }
        public string CarType { get; set; }
    }
}