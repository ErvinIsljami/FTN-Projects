using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetworkService.ViewModel;
using System.ComponentModel;
using NetworkService.Models;

namespace NetworkService
{
    public class AllViewModelsContainer : BindableBase
    {
        public MyICommand<string> NavCommand { get; private set; }
        private NetworkViewViewModel networkViewViewModel = new NetworkViewViewModel();
        private NetworkDataViewModel networkDataViewModel = new NetworkDataViewModel();
        private ChartDataViewModel chartViewModel = new ChartDataViewModel();
        private ReportViewModel reportViewModel = new ReportViewModel();
        private BindableBase currentViewModel;

        public AllViewModelsContainer()
        { 
            NavCommand = new MyICommand<string>(OnNav);
            CurrentViewModel = networkViewViewModel;
        }

        public BindableBase CurrentViewModel
        {
            get
            {
                return currentViewModel;
            }

            set
            {
                SetProperty(ref currentViewModel, value);
            }
        }

        private void OnNav(string destination)
        {
            switch (destination)
            {
                case "chartData":
                    CurrentViewModel = chartViewModel;
                    break;

                case "networkData":
                    CurrentViewModel = networkDataViewModel;
                    break;

                case "networkView":
                    CurrentViewModel = networkViewViewModel;
                    break;

                case "report":
                    CurrentViewModel = reportViewModel;
                    break;
            }
        }
    }
}
