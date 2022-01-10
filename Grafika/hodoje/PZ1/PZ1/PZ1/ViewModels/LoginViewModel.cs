using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PZ1.Commands;
using PZ1.EventArguments;
using PZ1.Xml;
using System.Configuration;
using PZ1.Models;
using System.IO;

namespace PZ1.ViewModels
{
    public class LoginViewModel : BindableBase
    {
        private CustomXmlRW _customXmlRW;
        private string _usersFilename;

        public User LoginUser { get; set; }
        private string _username;
        private string _password;
        private string _loginRegisterMessage;

        public string Username
        {
            get { return _username; }
            set
            {
                SetField<string>(ref _username, value);
                LoginCommand.RaiseCanExecuteChanged();
                RegisterCommand.RaiseCanExecuteChanged();
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                SetField<string>(ref _password, value);
                LoginCommand.RaiseCanExecuteChanged();
                RegisterCommand.RaiseCanExecuteChanged();
            }
        }

        public string LoginRegistrationMessage
        {
            get { return _loginRegisterMessage; }
            set { SetField<string>(ref _loginRegisterMessage, value); }
        }

        public MyICommand LoginCommand { get; private set; }        

        public delegate void LoginEventHandler(object source, LoginEventArgs args);

        public event LoginEventHandler LoggingIn;

        public MyICommand RegisterCommand { get; private set; }

        public delegate void RegisterEventHandler(object source, RegisterEventArgs args);

        public event RegisterEventHandler Registering;

        public LoginViewModel() { }

        public LoginViewModel(CustomXmlRW customXmlRW)
        {
            _customXmlRW = customXmlRW;
            string workingDirectory = Environment.CurrentDirectory;
            string projectDirectory = Directory.GetParent(workingDirectory).Parent.FullName;
            _usersFilename = ConfigurationManager.AppSettings["usersFile"];
            _usersFilename = _usersFilename.Replace("{AppDir}", projectDirectory);

            // Handle empty users file
            var users = _customXmlRW.DeSerializeObject<List<User>>(_usersFilename);
            if(users == null)
            {
                users = new List<User>();
                _customXmlRW.SerializeObject<List<User>>(users, _usersFilename);
            }

            LoginCommand = new MyICommand(OnLoggingIn, LogInCanExecute);
            RegisterCommand = new MyICommand(OnRegistering, RegisterCanExecute);

            LoginUser = new User();
        }

        protected virtual void OnLoggingIn()
        {
            LoginRegistrationMessage = "";
            LoginUser.Username = Username;
            LoginUser.Password = Password;
            LoginUser.Validate();
            if (LoginUser.IsValid)
            {
                if (CheckIfExists(LoginUser))
                {
                    LoggingIn?.Invoke(this, new LoginEventArgs(Username, Password));
                }
                else
                {
                    LoginRegistrationMessage = "User with these credentials does not exist.";
                    return;
                }
            }
        }

        private bool LogInCanExecute()
        {
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                return false;
            }
            return true;
        }

        protected virtual void OnRegistering()
        {
            LoginRegistrationMessage = "";
            LoginUser.Username = Username;
            LoginUser.Password = Password;
            LoginUser.Validate();
            if (LoginUser.IsValid)
            {
                if (!CheckIfExists(LoginUser))
                {
                    var users = _customXmlRW.DeSerializeObject<List<User>>(_usersFilename);
                    users.Add(LoginUser);
                    _customXmlRW.SerializeObject<List<User>>(users, _usersFilename);
                    Registering?.Invoke(this, new RegisterEventArgs(Username, Password));
                }
                else
                {
                    LoginRegistrationMessage = "User with these credentials already exists.";                    
                }
            }
        }

        private bool RegisterCanExecute()
        {
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                return false;
            }
            return true;
        }

        // Subscription to LoggedOut event from MainWindowViewModel
        // which in return subscribed to LoggedOut event from ContentViewModel
        public void OnLoggedOut(object source, EventArgs e)
        {
            Username = "";
            Password = "";
        }

        private bool CheckIfExists(User user)
        {
            bool result = false;
            if(!string.IsNullOrWhiteSpace(user.Username))
            {
                var users = _customXmlRW.DeSerializeObject<List<User>>(_usersFilename);
                if(users != null)
                {
                    if (users.Count > 0)
                    {
                        if (users.Any(u => u.Username == user.Username && u.Password == user.Password))
                        {
                            result = true;
                        }
                    }
                }
            }
            return result;
        }
    }
}
