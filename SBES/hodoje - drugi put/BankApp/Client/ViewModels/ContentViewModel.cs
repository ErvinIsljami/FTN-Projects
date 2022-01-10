using Client.UICommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace Client.ViewModels
{
	public class ContentViewModel : BindableBase
	{
		#region Fields
		private IUnityContainer _container;
		private BindableBase _currentContentViewModel;
		private UserViewModel _userViewModel;
		#endregion

		#region Properties
		public BindableBase CurrentContentViewModel
		{
			get { return _currentContentViewModel; }
			set { SetField(ref _currentContentViewModel, value); }
		}
		#endregion

		#region Constructors
		public ContentViewModel(IUnityContainer container)
		{
			_container = container;

			_userViewModel = _container.Resolve<UserViewModel>();
			CurrentContentViewModel = _userViewModel;
		}
		#endregion

		#region Methods
		public void DisplayCustomerView()
		{
			CurrentContentViewModel = _userViewModel;
		}
		#endregion
	}
}
