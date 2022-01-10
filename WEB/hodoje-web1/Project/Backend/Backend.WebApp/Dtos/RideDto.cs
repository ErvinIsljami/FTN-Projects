using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using DomainEntities.Models;

namespace Backend.Dtos
{
    public class RideDto
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public LocationDto StartLocation { get; set; }
        public string CarType { get; set; }
        public UserDto Customer { get; set; }
        public LocationDto DestinationLocation { get; set; }
        public UserDto Dispatcher { get; set; }
        public UserDto Driver { get; set; }
        public double Price { get; set; }
        public List<CommentDto> Comments { get; set; }
        public string RideStatus { get; set; }
    }
}