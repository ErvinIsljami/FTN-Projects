using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities.Models
{
    public class Ride
    {
        public Ride()
        {
            Comments = new HashSet<Comment>();
        }
        [Key]
        public int Id { get; set; }
        [Required]
        [Index(IsUnique = true)]
        public DateTime Timestamp { get; set; }
        [Required]
        public int StartLocationId { get; set; }  
        public Location StartLocation { get; set; }
        public int? DestinationLocationId { get; set; }
        public Location DestinationLocation { get; set; }
        public double Price { get; set; }
        [Required]
        public int RideStatus { get; set; }
        [Required]
        public int CarType { get; set; }        
        public int? CustomerId { get; set; }
        public User Customer { get; set; }
        public int? DispatcherId { get; set; }   
        public User Dispatcher { get; set; }
        public int? DriverId { get; set; }
        public User Driver { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}
