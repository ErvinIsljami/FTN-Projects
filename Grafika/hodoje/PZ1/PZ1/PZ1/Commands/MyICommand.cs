using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PZ1.Commands
{
    public class MyICommand : ICommand
    {
        Action _targetExecuteMethod;
        Func<bool> _targetCanExecuteMethod;

        public MyICommand(Action executeMethod)
        {
            _targetExecuteMethod = executeMethod;
        }

        public MyICommand(Action executeMethod, Func<bool> canExecuteMethod)
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
            if(_targetCanExecuteMethod != null)
            {
                return _targetCanExecuteMethod();
            }

            if(_targetExecuteMethod != null)
            {
                return true;
            }

            return false;
        }

        public void Execute(object parameter)
        {
            if(_targetExecuteMethod != null)
            {
                _targetExecuteMethod();
            }
        }
    }

    public class MyICommand<T> : ICommand
    {
        Action<T> _targetExecuteMethod;
        Func<T, bool> _targetCanExecuteMethod;

        public MyICommand(Action<T> executeMethod)
        {
            _targetExecuteMethod = executeMethod;
        }

        public MyICommand(Action<T> executeMethod, Func<T, bool> canExecuteMethod)
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
            if(_targetCanExecuteMethod != null)
            {
                T tparam = (T)parameter;
                return _targetCanExecuteMethod(tparam);
            }

            if(_targetExecuteMethod != null)
            {
                return true;
            }

            return false;
        }

        public void Execute(object parameter)
        {
            if(_targetExecuteMethod != null)
            {
                _targetExecuteMethod((T)parameter);
            }
        }
    }
}
