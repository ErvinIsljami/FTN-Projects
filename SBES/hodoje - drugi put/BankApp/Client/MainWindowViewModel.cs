using Client.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace Client
{
	public class MainWindowViewModel : BindableBase
	{
		#region Fields
		private IUnityContainer _container;
		private ContentViewModel _contentViewModel;
		private BindableBase _currentViewModel;		
		#endregion

		#region Properties
		public BindableBase CurrentViewModel
		{
			get { return _currentViewModel; }
			set { SetField(ref _currentViewModel, value); }
		}
		#endregion

		#region Constructors
		public MainWindowViewModel(IUnityContainer container)
		{
			_container = container;

			_contentViewModel = _container.Resolve<ContentViewModel>();
			CurrentViewModel = _contentViewModel;
		}
		#endregion
	}
}
