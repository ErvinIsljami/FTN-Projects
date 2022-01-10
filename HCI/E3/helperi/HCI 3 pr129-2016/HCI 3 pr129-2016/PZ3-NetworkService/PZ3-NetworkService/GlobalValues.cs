using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PZ3_NetworkService
{
    public static class GlobalValues
    {
        private static ObservableCollection<double> gVals = new ObservableCollection<double>();

        public static ObservableCollection<double> GVals { get => gVals; set => gVals = value; }

        private static ObservableCollection<string> dTime = new ObservableCollection<string>();

        public static ObservableCollection<string> DTime { get => dTime; set => dTime = value; }

    }
}
