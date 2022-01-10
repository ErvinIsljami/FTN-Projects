using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PZ1.EventArguments
{
    public class RegisterEventArgs : EventArgs
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public RegisterEventArgs(string username, string password)
        {
            Username = username;
            Password = password;            
        }
    }
}
