using PZ1.Commands;
using PZ1.EventArguments;
using PZ1.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace PZ1
{
    public class MainWindowViewModel : BindableBase
    {
        private IUnityContainer _container;
        private LoginViewModel _loginViewModel;
        private ContentViewModel _contentViewModel;
        private BindableBase _currentViewModel;

        public delegate void RegisterEventHandler(object source, RegisterEventArgs args);
        public event RegisterEventHandler Registered;

        public delegate void LoggingOutEventHandler(object source, EventArgs args);
        public event LoggingOutEventHandler LoggedOut;

        public MainWindowViewModel() { }

        public MainWindowViewModel(IUnityContainer container)
        {
            _container = container;

            _loginViewModel = _container.Resolve<LoginViewModel>();
            _loginViewModel.LoggingIn += OnLoggingIn;
            _loginViewModel.Registering += OnRegistering;

            _contentViewModel = _container.Resolve<ContentViewModel>();
            _contentViewModel.LoggedOut += OnLoggedOut;

            Registered += _contentViewModel.OnRegistered;
            LoggedOut += _loginViewModel.OnLoggedOut;

            CurrentViewModel = _loginViewModel;
        }

        public BindableBase CurrentViewModel
        {
            get { return _currentViewModel; }
            set { SetField(ref _currentViewModel, value); }
        }

        // Subscription to Logging in event from LoginViewModel
        private void OnLoggingIn(object source, LoginEventArgs e)
        {
            CurrentViewModel = _contentViewModel;
            _contentViewModel.UpdateOtherViewModelsUsernameAndPassword(e.Username, e.Password);
        }

        // Subscription to LoggingOut event from ContentViewModel
        private void OnLoggedOut(object source, EventArgs e)
        {
            LoggedOut?.Invoke(this, EventArgs.Empty);
            CurrentViewModel = _loginViewModel;
            _loginViewModel.LoginRegistrationMessage = "";
        }

        // Subscription to Registering event from LoginViewModel
        private void OnRegistering(object source, RegisterEventArgs e)
        {
             CurrentViewModel = _contentViewModel;
             OnRegistered(e);
        }

        // Fire an event that ContentViewModel listens for
        // so it can change the CurrentContentViewModel to AddImageViewModel
        private void OnRegistered(RegisterEventArgs e)
        {
            Registered?.Invoke(this, e);
        }
    }
}
