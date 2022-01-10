using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.FileReader.ReadModels
{
    public class DispatcherReadModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }        
        public bool IsBanned { get; set; }
        public int Role { get; set; }
    }
}
