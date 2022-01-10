using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkService.Models
{
    // Singleton class that will contain a list of INotify object that represent ViewModels
    // Also, it will contain a list of actions that will be invoked when we notify all ViewModels
    public class NotifiedVms : BindableBase
    {
        private NotifiedVms()
        {
            NotifiedVmsList = new List<INotify>();
            ListOfActions = new List<Action>();
        }

        // List of INotify object that are actually our ViewModels
        public List<INotify> NotifiedVmsList { get; set; }

        // List of actions that will be invoked when we notify all objects from NotifiedVmsList
        public List<Action> ListOfActions { get; set; }

        // Our singleton object
        private static NotifiedVms NV { get; set; }

        public static NotifiedVms Instance
        {
            get
            {
                if (NV == null)
                {
                    NV = new NotifiedVms();
                }
                return NV;
            }
        }

        private Road _currentRoad;

        // This property will be updated each time a new Road data arrives
        public Road CurrentRoad
        {
            get { return _currentRoad; }
            set
            {
                if (_currentRoad != value)
                {
                    _currentRoad = value;
                    OnPropertyChanged("CurrentRoad");
                }
            }
        }

        // This method registers a new INotify object (read as ViewModel)
        // It will be called in each ViewModel's constructor (as a last statement)
        public void Register(INotify obj)
        {
            if (!NotifiedVmsList.Contains(obj))
            {
                NotifiedVmsList.Add(obj);
            }
        }

        // This method will register a new action that will be invoked each time a new Road data arrives
        public void RegisterAction(Action act)
        {
            if (!ListOfActions.Contains(act))
            {
                ListOfActions.Add(act);
            }
        }

        // This method will be called each time a new Road data arrives and as a parameter it accepts the new Road
        public void NotifyAll(Road changedRoad)
        {
            // Here we notify each INotify object (read as ViewModel's)
            foreach (var vm in NotifiedVmsList)
            {
                vm.Notify(changedRoad);
            }

            // Here we invoke each action that is registered
            foreach (var act in ListOfActions)
            {
                act.Invoke();
            }
            // Update the current Road
            CurrentRoad = changedRoad;
        }
    }
}
