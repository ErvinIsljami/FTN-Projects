using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetworkService.Models;

namespace NetworkService.ViewModel
{
    public class NetworkViewViewModel : BindableBase, INotify
    {
        private DataIO serializer = new DataIO();

        private Road _currentRoad;

        // ObservableCollection represents a dynamic data collection that provides notifications when items get added, removed or when the whole list is refreshed
        // Essentially, it works like a regular collection, except that it implements the interfaces "INotifyCollectionChanged" and "INotifyPropertyChanged"
        // As such, it is very useful when we want to know when the collection has changed, it allows the code outside the collection to be aware of when changes to the collection occur
        // An event is triggered that will tell the user that entries have been added/removed or moved
        public ObservableCollection<Road> Roads
        {
            get
            {
                return RoadsObs.Instance.Roads;
            }

            set
            {
                if (RoadsObs.Instance.Roads != value)
                {
                    RoadsObs.Instance.Roads = value;
                    OnPropertyChanged("Roads");
                }
            }
        }

        public NetworkViewViewModel()
        {
            Roads.Clear();                                                                                          // We clear the list of the old elements
            if (serializer.DeSerializeObject<ObservableCollection<Road>>("roads.xml") != null)
            {
                serializer.DeSerializeObject<ObservableCollection<Road>>("roads.xml").ToList().ForEach(Roads.Add);  // And then we deserialize the file holding a list of Roads
            }

            NotifiedVms.Instance.Register(this);
        }

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

        public void Notify(Road changedRoad)
        {
            CurrentRoad = changedRoad;
        }
    }
}
