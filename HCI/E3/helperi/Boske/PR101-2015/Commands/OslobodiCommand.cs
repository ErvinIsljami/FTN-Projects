using PR101_2015.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PR101_2015.Commands
{
    class OslobodiCommand : ICommand
    {


        private MainViewModel addDj;


        public OslobodiCommand(MainViewModel addDj)
        {
            this.addDj = addDj;
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            this.addDj.Oslobodi();
        }
    }
}
