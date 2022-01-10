using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using DomainEntities.Models;

namespace Backend.Dtos
{
    public class CarDto
    {        
        public int? Id { get; set; }
        public int YearOfManufactoring { get; set; }
        public string RegistrationNumber { get; set; }
        public int TaxiNumber { get; set; }
        public string CarType { get; set; }
        public int DriverId { get; set; }
    }
}