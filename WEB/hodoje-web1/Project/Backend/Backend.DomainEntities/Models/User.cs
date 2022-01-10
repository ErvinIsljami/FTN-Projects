using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities.Models
{
    // For simplicity we are using only one class model for our users that has all possible properties listed in assignment text
    public class User
    {
        public User()
        {
            CustomerRides = new HashSet<Ride>();
            DispatcherRides = new HashSet<Ride>();
            DriverRides = new List<Ride>();
            Comments = new HashSet<Comment>();
        }

        [Key]
        public int Id { get; set; }        
        [Required]
        [Index(IsUnique=true)]
        [MinLength(8)]
        [MaxLength(450)]
        public string Username { get; set; }
        [Required]
        [MinLength(8)]
        [MaxLength(30)]
        public string Password { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(30)]
        public string Name { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(30)]
        public string Lastname { get; set; }
        [Required]
        public int Gender { get; set; }
        [MinLength(13)]
        [MaxLength(13)]
        public string NationalIdentificationNumber { get; set; }
        [MinLength(5)]
        [MaxLength(10)]
        public string PhoneNumber { get; set; }
        [Required]
        [MaxLength(254)]
        public string Email { get; set; }
        [Required]
        public bool IsBanned { get; set; }
        [Required]
        public int Role { get; set; }
        
        // Driver specific properties
        // These properties are optional because a user can also be a customer or a dispatcher
        // which don't have values for these properties
        public int? DriverLocationId { get; set; }
        public Location DriverLocation { get; set; }
        public int? CarId { get; set; }
        public Car Car { get; set; }
        ////////////////////////////////////////////

        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Ride> CustomerRides { get; set; }
        public virtual ICollection<Ride> DispatcherRides { get; set; }
        public virtual ICollection<Ride> DriverRides { get; set; }
    }
}
