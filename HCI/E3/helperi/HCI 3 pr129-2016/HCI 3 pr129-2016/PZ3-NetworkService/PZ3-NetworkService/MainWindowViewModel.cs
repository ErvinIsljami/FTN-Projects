using PZ3_NetworkService.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace PZ3_NetworkService
{
    public class MainWindowViewModel : BindableBase
    {
        public MyICommand<string> NavCommand { get; private set; }
        public MyICommand ExitCommand { get; set; }

        private bool nvIsActive;
        private bool ndIsActive;
        private bool dcIsActive;


        public bool NvIsActive
        {
            get { return nvIsActive; }
            set
            {
                nvIsActive = value;
                OnPropertyChanged("NvIsActive");
            }
        }

        public bool NdIsActive
        {
            get { return ndIsActive; }
            set
            {
                ndIsActive = value;
                OnPropertyChanged("NdIsActive");
            }
        }

        public bool DcIsActive
        {
            get { return dcIsActive; }
            set
            {
                dcIsActive = value;
                OnPropertyChanged("DcIsActive");
            }
        }

        private NetworkViewViewModel networkViewViewModel = new NetworkViewViewModel();
        private StartingScreenViewModel homeViewModel = new StartingScreenViewModel();
        private NetworkDataViewModel networkDataViewModel = new NetworkDataViewModel();
        private DataChartViewModel dataChartViewModel = new DataChartViewModel();

        private BindableBase currentViewModel;

        public MainWindowViewModel()
        {
            NavCommand = new MyICommand<string>(OnNav);
            CurrentViewModel = homeViewModel;

            ExitCommand = new MyICommand(OnExit);
        }

        public BindableBase CurrentViewModel
        {
            get { return currentViewModel; }
            set
            {
                SetProperty(ref currentViewModel, value);
            }
        }

        private void OnExit()
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to exit?", "Confirm closing application.",
                                          MessageBoxButton.YesNo,
                                          MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                Application.Current.MainWindow.Close();
            }
        }

        private void OnNav(string destination)
        {
            switch (destination)
            {
                case "nview":
                    CurrentViewModel = networkViewViewModel;
                    NvIsActive = true;
                    NdIsActive = false;
                    DcIsActive = false;
                    break;
                case "ndata":
                    CurrentViewModel = networkDataViewModel;
                    NvIsActive = false;
                    NdIsActive = true;
                    DcIsActive = false;
                    break;
                case "dchart":
                    CurrentViewModel = dataChartViewModel;
                    NvIsActive = false;
                    NdIsActive = false;
                    DcIsActive = true;
                    break;
            }
        }
    }
}
