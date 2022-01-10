using PZ1.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PZ1.Models
{
    public class User : ValidationBase
    {
        private string _username { get; set; }
        private string _password { get; set; }

        public User() { }

        public User(string username, string password)
        {
            Username = username;
            Password = password;
        }

        public string Username
        {
            get { return _username; }
            set
            {
                if(_username != value)
                {
                    _username = value;
                    OnPropertyChanged(nameof(Username));
                }
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                if (_password != value)
                {
                    _password = value;
                    OnPropertyChanged(nameof(Password));
                }
            }
        }

        protected override void ValidateSelf()
        {
            if (string.IsNullOrWhiteSpace(Username))
            {
                ValidationErrors[nameof(Username)] = "Username cannot be empty.";
            }
            else
            {
                char firstChar = Username[0];
                if (Char.IsDigit(firstChar))
                {
                    ValidationErrors[nameof(Username)] = "Username cannot start with a number.";
                }

                if (Username.Length > 30)
                {
                    ValidationErrors[nameof(Username)] = "Username can be a maximum of 30 characters long.";
                }
            }

            if (Password.Length < 7)
            {
                ValidationErrors[nameof(Password)] = "Password needs to be at least 7 characters long.";
            }

            if(Password.Length > 30)
            {
                ValidationErrors[nameof(Password)] = "Password can be a maximum of 30 characters long.";
            }
        }
    }
}
