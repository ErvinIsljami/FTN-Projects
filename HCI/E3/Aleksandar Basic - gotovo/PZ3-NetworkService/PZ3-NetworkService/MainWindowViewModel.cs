using PZ3_NetworkService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PZ3_NetworkService
{
	public class MainWindowViewModel : BindableBase
	{
		public BindableBase ContentViewModel { get; set; }

		public MainWindowViewModel()
		{
			ContentViewModel = new ContentViewModel();
		}
	}
}
