using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities.Models
{
    public class Car
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Range(1900, 2018)]
        public int YearOfManufactoring { get; set; }
        [Required]
        public string RegistrationNumber { get; set; }
        [Required]
        [Range(0, 1000000)]
        public int TaxiNumber { get; set; }
        [Required]
        public int CarType { get; set; }
        [Required]
        public int DriverId { get; set; }
        public User Driver { get; set; }
    }
}
