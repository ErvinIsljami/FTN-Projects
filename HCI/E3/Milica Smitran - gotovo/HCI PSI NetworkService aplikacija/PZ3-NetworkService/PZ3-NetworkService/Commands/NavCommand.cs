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
		private string previousTab;
		private string nextTab;

		public NavCommand(ContentViewModel contentViewModel, string previousTab, string nextTab)
		{
			this.contentViewModel = contentViewModel;
			this.previousTab = previousTab;
			this.nextTab = nextTab;
		}

		public void Execute()
		{
			contentViewModel.Navigate(nextTab);
		}

		public void Undo()
		{
			contentViewModel.Navigate(previousTab);
		}
	}
}
