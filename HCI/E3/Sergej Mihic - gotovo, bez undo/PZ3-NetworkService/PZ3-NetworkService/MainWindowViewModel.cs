using PZ3_NetworkService.ViewModel;

namespace PZ3_NetworkService
{
	public delegate void Changed();
    public class MainWindowViewModel : BindableBase
    {
        //public MyICommand<string> NavCommand { get; private set; }
        public MyICommand TabCommand { get; private set; }

        public static NetworkDataViewModel networkDataViewModel = new NetworkDataViewModel();
        public static MjeriloViewModel homeViewModel = new MjeriloViewModel();
        public static DataChartViewModel dataChartViewModel = new DataChartViewModel();
        public static BindableBase currentViewModel;
        public static event Changed PropChanged;
        public int tabCounter;

        public MainWindowViewModel()
        {
            TabCommand = new MyICommand(OnTab);
            tabCounter = 1;
            CurrentViewModel = homeViewModel;
            PropChanged += ChangeCurrentViewModel;
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

        public void ChangeCurrentViewModel()
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
            else
            {
                CurrentViewModel = homeViewModel;
                tabCounter = 1;
            }

        }
    }
    
}
