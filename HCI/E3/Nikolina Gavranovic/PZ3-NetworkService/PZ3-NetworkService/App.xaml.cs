using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace PZ3_NetworkService
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);

			var mainWindow = new MainWindow();
			var mainWindowViewModel = new MainWindowViewModel();
			mainWindow.DataContext = mainWindowViewModel;
			Application.Current.MainWindow = mainWindow;
			Application.Current.MainWindow.Show();
		}
	}
}
