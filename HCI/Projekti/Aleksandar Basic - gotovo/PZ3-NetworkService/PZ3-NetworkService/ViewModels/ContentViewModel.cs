using PZ3_NetworkService.Commands;
using PZ3_NetworkService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PZ3_NetworkService.ViewModels
{
	public class ContentViewModel : BindableBase
	{
		private Visibility isNavbarToggled;
		private BindableBase currentViewModel;

		public Visibility IsNavbarToggled
		{
			get { return isNavbarToggled; }
			set { SetField(ref isNavbarToggled, value); }
		}

		public BindableBase CurrentViewModel
		{
			get { return currentViewModel; }
			set { SetField(ref currentViewModel, value); }
		}

		public NetworkDataViewModel NetworkDataViewModel { get; set; }
		public NetworkViewViewModel NetworkViewViewModel { get; set; }
		public ChartViewModel ChartViewModel { get; set; }

		public MyICommand NavbarToggleCommand { get; set; }
		public MyICommand<string> NavUiCommand { get; set; }
		public MyICommand UndoCommand { get; set; }
		public NavCommand NavCommand { get; set; }

		public ContentViewModel()
		{
			IsNavbarToggled = Visibility.Hidden;

			NetworkDataViewModel = new NetworkDataViewModel();
			NetworkViewViewModel = new NetworkViewViewModel();
			ChartViewModel = new ChartViewModel();

			CurrentViewModel = NetworkDataViewModel;

			NavUiCommand = new MyICommand<string>(ExecuteNavCommand);
			NavbarToggleCommand = new MyICommand(ToggleNavbar);
			UndoCommand = new MyICommand(OnUndo);
		}

		public void ExecuteNavCommand(string parameter)
		{
			string previousTab = CurrentViewModel.GetType().Name.Replace("ViewModel", "");
			NavCommand = new NavCommand(this, previousTab, parameter);
			Container.CommandInvoker.AddAndExecute(NavCommand);
		}

		private void OnUndo()
		{
			Container.CommandInvoker.Undo();
		}

		/// <summary>
		/// Called by NavbarCommand.
		/// </summary>
		/// <param name="navigateTo"></param>
		public void Navigate(string navigateTo)
		{
			switch (navigateTo)
			{
				case "Home":
					CurrentViewModel = NetworkDataViewModel;
					break;
				case "NetworkData":
					CurrentViewModel = NetworkDataViewModel;
					ToggleNavbar();
					break;
				case "NetworkView":
					CurrentViewModel = NetworkViewViewModel;
					ToggleNavbar();
					break;
				case "Chart":
					CurrentViewModel = ChartViewModel;
					ToggleNavbar();
					break;
			}
		}

		public void ToggleNavbar()
		{
			if(IsNavbarToggled == Visibility.Hidden)
			{
				IsNavbarToggled = Visibility.Visible;
			}
			else
			{
				IsNavbarToggled = Visibility.Hidden;
			}
		}
	}
}
