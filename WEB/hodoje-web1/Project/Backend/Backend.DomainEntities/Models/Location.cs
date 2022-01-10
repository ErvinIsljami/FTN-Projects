using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities.Models
{
    public class Location
    {
        public Location()
        {
            Drivers = new HashSet<User>();
            RideStarts = new HashSet<Ride>();
            RideDestinations = new HashSet<Ride>();
            Address = new Address();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        public Address Address { get; set; }
        [Required]
        [Range(-180, 180)]
        public double Longitude { get; set; }
        [Required]
        [Range(-90, 90)]
        public double Latitude { get; set; }
        public virtual ICollection<User> Drivers { get; set; }
        public virtual ICollection<Ride> RideStarts { get; set; }
        public virtual ICollection<Ride> RideDestinations { get; set; }
    }
}
