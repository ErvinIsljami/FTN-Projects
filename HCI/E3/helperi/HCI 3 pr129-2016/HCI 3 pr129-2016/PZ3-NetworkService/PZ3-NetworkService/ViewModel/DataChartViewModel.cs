using PZ3_NetworkService.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace PZ3_NetworkService.ViewModel
{
    public class DataChartViewModel : BindableBase
    {
        private ObservableCollection<double> objects;

        private ObservableCollection<string> times;


        public DataChartViewModel()
        {  
            Objects = new ObservableCollection<double>();
            Objects = GlobalValues.GVals;

            Times = new ObservableCollection<string>();
            Times = GlobalValues.DTime;
        }

        public ObservableCollection<double> Objects
        {
            get
            {
                return objects;
            }
            set
            {
                if (objects != value)
                {
                    objects = value;
                    OnPropertyChanged("Objects");
                }
            }
        }

        public ObservableCollection<string> Times
        {
            get
            {
                return times;
            }
            set
            {
                if (times != value)
                {
                    times = value;
                    OnPropertyChanged("Times");
                }
            }
        }
    }
}
