using PZ3_NetworkService.Model;
using PZ3_NetworkService.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PZ3_NetworkService
{
    public delegate void Changed();
    public class MainWindowViewModel : BindableBase
    {
        //public MyICommand<string> NavCommand { get; private set; }
        public MyICommand TabCommand { get; private set; }

        public static NetworkDataViewModel networkDataViewModel = new NetworkDataViewModel();
        public static MeriloViewModel homeViewModel = new MeriloViewModel();
        public static ReportViewModel reportViewModel = new ReportViewModel();
        public static DataChartViewModel dataChartViewModel = new DataChartViewModel();
        public static BindableBase currentViewModel;
        public static event Changed PropChanged;


        public int tabCounter;



        public MainWindowViewModel()
        {
          
            TabCommand = new MyICommand(OnTab);
            tabCounter = 1;
            CurrentViewModel = homeViewModel;
            PropChanged += x;
            

        }

        public BindableBase CurrentViewModel
        {
            get { return currentViewModel; }
            set
            {
                SetProperty(ref currentViewModel, value);
            }
        }



        public static void RisePropChanged()
        {
            PropChanged();
        }
        public void x()
        {
            OnPropertyChanged("CurrentViewModel");
        }

        private void OnTab()
        {
            if (tabCounter == 1)
            {
                CurrentViewModel = networkDataViewModel;
                tabCounter = 2;
            }
            else if (tabCounter == 2)
            {
                CurrentViewModel = dataChartViewModel;
                tabCounter = 3;
            }
            else if (tabCounter == 3)
            {
                CurrentViewModel = reportViewModel;
                tabCounter = 4;
            }
            else
            {
                CurrentViewModel = homeViewModel;
                tabCounter = 1;
            }

        }
    }
    
}
