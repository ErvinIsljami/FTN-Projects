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
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        [Required(AllowEmptyStrings = true)]
        [StringLength(1000)]
        public string Description { get; set; }
        [Required]
        public DateTime Timestamp { get; set; }
        [Required]
        public int UserId { get; set; }
        public User User { get; set; }
        [Required]
        public int RideId { get; set; }
        public Ride Ride { get; set; }
        [Range(0, 5)]
        public int Rating { get; set; }
    }
}
