using PZ3_NetworkService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PZ3_NetworkService.Commands
{
	public class NavCommand : ICommand
	{
		private ContentViewModel contentViewModel;
		private string PreviousTab;
		private string NextTab;

		public NavCommand(ContentViewModel contentViewModel, string previousTab, string nextTab)
		{
			this.contentViewModel = contentViewModel;
			PreviousTab = previousTab;
			NextTab = nextTab;
		}

		public void Execute()
		{
			contentViewModel.Navigate(NextTab);
		}

		public void Undo()
		{
			contentViewModel.Navigate(PreviousTab);
			// Execute Navigate will toggle the navbar so when we do the undo, 
			// toggle will be called again so we're toggling it again so it's not shown
			contentViewModel.ToggleNavbar();
		}
	}
}
