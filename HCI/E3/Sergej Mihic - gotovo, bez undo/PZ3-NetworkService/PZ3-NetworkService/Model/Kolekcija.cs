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
		public static ObservableCollection<Mjerilo> AllObjects { get; set; } = new ObservableCollection<Mjerilo>();

		// Za chartove
		public static ObservableCollection<double> GlobalBarValues { get; set; } = new ObservableCollection<double>();
		public static ObservableCollection<Brush> ChartColor { get; set; } = new ObservableCollection<Brush>();
		public static ObservableCollection<string> GlobDatumi { get; set; } = new ObservableCollection<string>();
	}
}
