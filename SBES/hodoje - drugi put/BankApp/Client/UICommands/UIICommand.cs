using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Client.UICommands
{
	/// <summary>
	/// Class used for encapsulating actions that are binded to a UI button
	/// </summary>
	public class UIICommand : ICommand
	{
		Action _targetExecuteMethod;
		Func<bool> _targetCanExecuteMethod;

		public UIICommand(Action executeMethod)
		{
			_targetExecuteMethod = executeMethod;
		}

		public UIICommand(Action executeMethod, Func<bool> canExecuteMethod)
		{
			_targetExecuteMethod = executeMethod;
			_targetCanExecuteMethod = canExecuteMethod;
		}

		public event EventHandler CanExecuteChanged = delegate { };

		public void RaiseCanExecuteChanged()
		{
			CanExecuteChanged(this, EventArgs.Empty);
		}

		public bool CanExecute(object parameter)
		{
			if (_targetCanExecuteMethod != null)
			{
				return _targetCanExecuteMethod();
			}

			if (_targetExecuteMethod != null)
			{
				return true;
			}

			return false;
		}

		public void Execute(object parameter)
		{
			if (_targetExecuteMethod != null)
			{
				_targetExecuteMethod();
			}
		}
	}

	public class UIICommand<T> : ICommand
	{
		Action<T> _targetExecuteMethod;
		Func<T, bool> _targetCanExecuteMethod;

		public UIICommand(Action<T> executeMethod)
		{
			_targetExecuteMethod = executeMethod;
		}

		public UIICommand(Action<T> executeMethod, Func<T, bool> canExecuteMethod)
		{
			_targetExecuteMethod = executeMethod;
			_targetCanExecuteMethod = canExecuteMethod;
		}

		public event EventHandler CanExecuteChanged = delegate { };

		public void RaiseCanExecuteChanged()
		{
			CanExecuteChanged(this, EventArgs.Empty);
		}

		public bool CanExecute(object parameter)
		{
			if (_targetCanExecuteMethod != null)
			{
				T tparam = (T)parameter;
				return _targetCanExecuteMethod(tparam);
			}

			if (_targetExecuteMethod != null)
			{
				return true;
			}

			return false;
		}

		public void Execute(object parameter)
		{
			if (_targetExecuteMethod != null)
			{
				_targetExecuteMethod((T)parameter);
			}
		}
	}
}
