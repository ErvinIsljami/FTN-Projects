using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetworkService.Models;

namespace NetworkService.ViewModel
{
    public class ChartDataViewModel : BindableBase, INotify
    {
        private DataIO serializer = new DataIO();

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

        public Road SelectedRoad
        {
            get { return _selectedRoad; }
            set
            {
                _selectedRoad = value;
            }
        }

        private Road _selectedRoad;

        public ChartDataViewModel()
        {
            NotifiedVms.Instance.Register(this);
        }

        public void Notify(Road changedRoad)
        {
            //throw new NotImplementedException();
        }
    }
}
