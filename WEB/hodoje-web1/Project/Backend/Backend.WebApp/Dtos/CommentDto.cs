using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using DomainEntities.Models;

namespace Backend.Dtos
{
    public class CommentDto
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime Timestamp { get; set; }
        public int RideId { get; set; }
        public int Rating { get; set; }
        public UserDto User { get; set; }
        public int UserId { get; set; }
    }
}