using PZ1.Commands;
using PZ1.Xml;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PZ1.EventArguments;
using PZ1.Models;
using System.Threading;

namespace PZ1.ViewModels
{
    public class AccountDetailsViewModel : BindableBase
    {
        private CustomXmlRW _customXmlRW;
        private string _projectDirectory;
        private string _usersFilename;
        private string _imagesContainerDirectory;
        private string _imagesFileName;

        private string _currentUsername;
        private string _currentPassword;
        private string _newUsername;
        private string _newPassword;
        private string _applyChangesMessage;
        private User _userToChange;

        public string CurrentUsername
        {
            get { return _currentUsername; }
            set { SetField<string>(ref _currentUsername, value); }
        }

        public string CurrentPassword
        {
            get { return _currentPassword; }
            set { SetField<string>(ref _currentPassword, value); }
        }

        public string NewUsername
        {
            get { return _newUsername; }
            set
            {
                SetField<string>(ref _newUsername, value);
                ApplyChangesCommand.RaiseCanExecuteChanged();
            }
        }

        public string NewPassword
        {
            get { return _newPassword; }
            set
            {
                SetField<string>(ref _newPassword, value);
                ApplyChangesCommand.RaiseCanExecuteChanged();
            }
        }

        public User UserToChange
        {
            get { return _userToChange; }
            set { SetField<User>(ref _userToChange, value); }
        }

        public string ApplyChangesMessage
        {
            get { return _applyChangesMessage; }
            set { SetField<string>(ref _applyChangesMessage, value); }
        }

        public MyICommand ApplyChangesCommand { get; private set; }

        public delegate void ApplyChangesEventHandler(object source, LoginEventArgs args);

        public event ApplyChangesEventHandler AccountChangesApplied;

        public AccountDetailsViewModel() { }

        public AccountDetailsViewModel(CustomXmlRW customXmlRW)
        {
            _customXmlRW = customXmlRW;

            string workingDirectory = Environment.CurrentDirectory;
            _projectDirectory = Directory.GetParent(workingDirectory).Parent.FullName;
            _usersFilename = ConfigurationManager.AppSettings["usersFile"];
            _usersFilename = _usersFilename.Replace("{AppDir}", _projectDirectory);
            _imagesContainerDirectory = ConfigurationManager.AppSettings["imagesContainerDirectory"];
            _imagesContainerDirectory = _imagesContainerDirectory.Replace("{AppDir}", _projectDirectory);
            _imagesFileName = ConfigurationManager.AppSettings["imagesFile"];
            _imagesFileName = _imagesFileName.Replace("{AppDir}", _projectDirectory);

            ApplyChangesCommand = new MyICommand(OnApplyChanges, CanApplyChanges);            

            NewUsername = CurrentUsername;
            NewPassword = CurrentPassword;
            ApplyChangesMessage = "";
        }

        public void SetUpInitialUsernameAndPassword(string username, string password)
        {
            CurrentUsername = username;
            CurrentPassword = password;
            NewUsername = username;
            NewPassword = password;
            if(UserToChange == null)
            {
                var users = _customXmlRW.DeSerializeObject<List<User>>(_usersFilename);
                UserToChange = users.FirstOrDefault(u => u.Username == CurrentUsername);
            }
        }

        public void OnApplyChanges()
        {
            ApplyChangesMessage = "";

            UserToChange.Username = NewUsername;
            UserToChange.Password = NewPassword;
            UserToChange.Validate();
            if (UserToChange.IsValid)
            {
                var users = _customXmlRW.DeSerializeObject<List<User>>(_usersFilename);
                if (users.Count > 0)
                {
                    User oldUser = users.FirstOrDefault(u => u.Username == CurrentUsername);
                    if (oldUser != null)
                    {
                        if(oldUser.Username != UserToChange.Username && oldUser.Password != UserToChange.Password || 
                            oldUser.Username != UserToChange.Username && oldUser.Password == UserToChange.Password)
                        {
                            if (users.Any(u => u.Username == UserToChange.Username))
                            {
                                ApplyChangesMessage = "User with this username already exists.";
                                return;
                            }
                            else
                            {
                                // Update all images
                                var allImages = _customXmlRW.DeSerializeObject<List<Image>>(_imagesFileName);
                                foreach (var img in allImages)
                                {
                                    if (img.Owner == oldUser.Username)
                                    {
                                        img.Owner = UserToChange.Username;
                                    }
                                }
                                _customXmlRW.SerializeObject<List<Image>>(allImages, _imagesFileName);

                                oldUser.Username = UserToChange.Username;
                                oldUser.Password = UserToChange.Password;
                                _customXmlRW.SerializeObject<List<User>>(users, _usersFilename);
                                ApplyChangesMessage = "Changes applied successfully.";
                                CurrentUsername = UserToChange.Username;
                                CurrentPassword = UserToChange.Password;
                                AccountChangesApplied?.Invoke(this, new LoginEventArgs(CurrentUsername, CurrentPassword));
                                Task.Run(() =>
                                {
                                    Thread.Sleep(1000);
                                    ApplyChangesMessage = "";
                                });
                            }
                        }
                        else if(oldUser.Username == UserToChange.Username && oldUser.Password != UserToChange.Password)
                        {
                            oldUser.Username = UserToChange.Username;
                            oldUser.Password = UserToChange.Password;
                            _customXmlRW.SerializeObject<List<User>>(users, _usersFilename);
                            ApplyChangesMessage = "Changes applied successfully.";
                            CurrentUsername = UserToChange.Username;
                            CurrentPassword = UserToChange.Password;
                            AccountChangesApplied?.Invoke(this, new LoginEventArgs(CurrentUsername, CurrentPassword));
                            Task.Run(() =>
                            {
                                Thread.Sleep(1000);
                                ApplyChangesMessage = "";
                            });
                        }
                    }
                }
            }
        }

        public bool CanApplyChanges()
        {
            return !(NewUsername == CurrentUsername && NewPassword == CurrentPassword);
        }
    }
}
