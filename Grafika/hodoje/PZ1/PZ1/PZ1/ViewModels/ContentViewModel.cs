using PZ1.Commands;
using PZ1.EventArguments;
using PZ1.Xml;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace PZ1.ViewModels
{
    public class ContentViewModel : BindableBase
    {
        private IUnityContainer _container;
        private CustomXmlRW _customXmlRW;
        private string _usersFilename;

        private string _currentUsername;
        private string _currentPassword;

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

        private ImageCollectionViewModel _imageCollectionViewModel;
        private AddImageViewModel _addImageViewModel;
        private AccountDetailsViewModel _accountDetailsViewModel;
        private BindableBase _currentContentViewModel;

        public MyICommand<string> NavCommand { get; private set; }
        public MyICommand LogoutCommand { get; private set; }

        public delegate void LogoutEventHandler(object sender, EventArgs args);
        public event LogoutEventHandler LoggedOut;

        public ContentViewModel() { }

        public ContentViewModel(IUnityContainer container, CustomXmlRW customXmlRW)
        {
            _container = container;

            _customXmlRW = customXmlRW;

            string workingDirectory = Environment.CurrentDirectory;
            string projectDirectory = Directory.GetParent(workingDirectory).Parent.FullName;
            _usersFilename = ConfigurationManager.AppSettings["usersFile"];
            _usersFilename = _usersFilename.Replace("{AppDir}", projectDirectory);

            _imageCollectionViewModel = _container.Resolve<ImageCollectionViewModel>();
            _addImageViewModel = _container.Resolve<AddImageViewModel>();
            _accountDetailsViewModel = _container.Resolve<AccountDetailsViewModel>();
            _addImageViewModel.ImageAdded += OnImageAdded;
            _accountDetailsViewModel.AccountChangesApplied += OnAccountChangesApplied;

            UpdateOtherViewModelsUsernameAndPassword(CurrentUsername, CurrentPassword);

            CurrentContentViewModel = _imageCollectionViewModel;

            NavCommand = new MyICommand<string>(OnNav);
            LogoutCommand = new MyICommand(OnLoggingOut);
        }

        public BindableBase CurrentContentViewModel
        {
            get { return _currentContentViewModel; }
            set { SetField(ref _currentContentViewModel, value); }
        }

        private void OnNav(string destination)
        {
            switch (destination)
            {
                case "showImages":
                    CurrentContentViewModel = _imageCollectionViewModel;
                    _imageCollectionViewModel.GetUserImages();
                    break;
                case "addImage":
                    CurrentContentViewModel = _addImageViewModel;
                    break;
                case "accountDetails":
                    CurrentContentViewModel = _accountDetailsViewModel;
                    break;
            }
        }

        private void OnLoggingOut()
        {
            UpdateOtherViewModelsUsernameAndPassword("", "");
            LoggedOut?.Invoke(this, EventArgs.Empty);
        }

        public void UpdateOtherViewModelsUsernameAndPassword(string username, string password)
        {
            _imageCollectionViewModel.SetUpInitialUsernameAndPassword(username, password);
            _addImageViewModel.SetUpInitialUsernameAndPassword(username, password);
            _accountDetailsViewModel.SetUpInitialUsernameAndPassword(username, password);
        }

        // Subscription to Registered event on MainWindowViewModel
        public void OnRegistered(object source, RegisterEventArgs e)
        {
            CurrentContentViewModel = _addImageViewModel;
            CurrentUsername = e.Username;
            CurrentPassword = e.Password;
            UpdateOtherViewModelsUsernameAndPassword(CurrentUsername, CurrentPassword);
        }

        // Subscription to AccountChangesApplied event on AccountDetailsViewModel
        public void OnAccountChangesApplied(object source, LoginEventArgs e)
        {
            UpdateOtherViewModelsUsernameAndPassword(e.Username, e.Password);
        }

        // Subscription to ImageAdded event on ImageCollectionViewModel
        public void OnImageAdded(object source, EventArgs e)
        {
            CurrentContentViewModel = _imageCollectionViewModel;
            _imageCollectionViewModel.GetUserImages();
        }
    }
}
