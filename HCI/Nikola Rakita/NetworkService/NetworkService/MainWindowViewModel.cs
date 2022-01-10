using NetworkService.Model;
using NetworkService.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkService
{
    public delegate void Changed();
    public class MainWindowViewModel : BindableBase
    {
        public MyICommand TabCommand { get; private set; }
        public static NetworkDataViewModel networkDataViewModel = new NetworkDataViewModel();
        public static GeneratorViewModel homeViewModel = new GeneratorViewModel();
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
        public static void RaisePropertyChanged()
        {
            PropChanged();
        }
        public void x()
        {
            OnPropertyChanged("CurrentViewModel");
        }

        //kada kliknes tab menjaju se tabovi unutar mainview-a
        //menjaju se oni viewovi koje smo pravili
        //zbog linuks korisnika
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
            else
            {
                CurrentViewModel = homeViewModel;
                tabCounter = 1;
            }
        }
    }
}
