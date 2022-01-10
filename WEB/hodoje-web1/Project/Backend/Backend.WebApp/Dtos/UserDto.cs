using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using DomainEntities.Models;

namespace Backend.Dtos
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        public string Gender { get; set; }
        public string NationalIdentificationNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public bool IsBanned { get; set; }
        public string Role { get; set; }
        public int? DriverLocationId { get; set; }
        public LocationDto DriverLocation { get; set; }
        public int? CarId { get; set; }
        public CarDto Car { get; set; }
    }
}