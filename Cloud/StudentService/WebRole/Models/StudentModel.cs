using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebRole.Models
{
    public class StudentModel
    {
        public String Name { get; set; }

        public String LastName { get; set; }

        public StudentModel(string name, string lastName)
        {
            Name = name;
            LastName = lastName;
        }

        public StudentModel()
        {
        }
    }
}