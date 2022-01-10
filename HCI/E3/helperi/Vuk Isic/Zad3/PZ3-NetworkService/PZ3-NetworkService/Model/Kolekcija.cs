using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace PZ3_NetworkService.Model
{
    public static class Kolekcija
    {
        private static ObservableCollection<Merilo> allObjects = new ObservableCollection<Merilo>();

        private static ObservableCollection<double> globalBarValues = new ObservableCollection<double>();

        private static ObservableCollection<Brush> chartColor = new ObservableCollection<Brush>();
        private static ObservableCollection<string> globDatumi = new ObservableCollection<string>();

        public static Dictionary<string, Border> NVBorder { get; set; } = new Dictionary<string, Border>();
        public static Dictionary<string, Merilo> DDbase { get; set; } = new Dictionary<string, Merilo>();

        public static ObservableCollection<Merilo> AllObjects { get => allObjects; set => allObjects = value; }
        public static string CurrentWindow { get => currentWindow; set => currentWindow = value; }
        public static ObservableCollection<double> GlobalBarValues { get => globalBarValues; set => globalBarValues = value; }
        public static ObservableCollection<Brush> ChartColor { get => chartColor; set => chartColor = value; }
        public static ObservableCollection<string> GlobDatumi { get => globDatumi; set => globDatumi = value; }

        private static string currentWindow;
    }
}
