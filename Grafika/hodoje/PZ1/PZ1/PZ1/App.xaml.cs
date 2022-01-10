using PZ1.ViewModels;
using PZ1.Xml;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using Unity;
using Unity.Injection;
using Unity.Lifetime;

namespace PZ1
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            IUnityContainer container = new UnityContainer();

            var customXmlRW = new CustomXmlRW();

            // Register ViewModels
            var loginViewModel = new LoginViewModel(customXmlRW);
            container.RegisterInstance<LoginViewModel>(loginViewModel);
            var showImagesViewModel = new ImageCollectionViewModel(customXmlRW);
            container.RegisterInstance<ImageCollectionViewModel>(showImagesViewModel);
            var addImageViewModel = new AddImageViewModel(customXmlRW);
            container.RegisterInstance<AddImageViewModel>(addImageViewModel);
            var accountDetailsViewModel = new AccountDetailsViewModel(customXmlRW);
            container.RegisterInstance<AccountDetailsViewModel>(accountDetailsViewModel);
            var contentViewModel = new ContentViewModel(container, customXmlRW);
            container.RegisterInstance<ContentViewModel>(contentViewModel);
            var mainWindowViewModel = new MainWindowViewModel(container);
            container.RegisterInstance<MainWindowViewModel>(mainWindowViewModel);

            var mainWindow = container.Resolve<MainWindow>();
            mainWindow.DataContext = container.Resolve<MainWindowViewModel>();
            Application.Current.MainWindow = mainWindow;
            Application.Current.MainWindow.Show();
        }
    }
}
