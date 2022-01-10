using PZ3_NetworkService.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PZ3_NetworkService.ViewModels
{
	public class ChartViewModel : BindableBase
	{
		private ObservableCollection<double> graphValues;
		private ObservableCollection<DateTime> graphDates;
		private int graphMin;	// Pixels of minimum value on the graph
		private int graphMax;   // Pixels of maximum value on the graph
		private int possibleValueMin;
		private int possibleValueMax;

		public ObservableCollection<double> GraphValues
		{
			get { return graphValues; }
			set
			{
				SetField(ref graphValues, value);
			}
		}

		public ObservableCollection<DateTime> GraphDates
		{
			get { return graphDates; }
			set
			{
				SetField(ref graphDates, value);
			}
		}

		public MyICommand ShowHistoryChartCommand { get; set; }

		public ChartViewModel()
		{
			ShowHistoryChartCommand = new MyICommand(ShowHistoryChart);
			GraphValues = new ObservableCollection<double>();
			GraphValues.Add(0);
			GraphValues.Add(0);
			GraphValues.Add(0);
			GraphValues.Add(0);
			GraphValues.Add(0);
			GraphDates = new ObservableCollection<DateTime>();
			GraphDates.Add(DateTime.MinValue);
			GraphDates.Add(DateTime.MinValue);
			GraphDates.Add(DateTime.MinValue);
			GraphDates.Add(DateTime.MinValue);
			GraphDates.Add(DateTime.MinValue);
			graphMin = 150;
			graphMax = 450;
			possibleValueMin = 580;
			possibleValueMax = 850;
		}

		public void ShowHistoryChart()
		{
			for(int i = 0; i < GraphValues.Count; i++)
			{
				GraphValues[i] = 0;
			}

			for(int i = 0; i < GraphDates.Count; i++)
			{
				GraphDates[i] = DateTime.MinValue;
			}			

			string[] logLines = Logger.LoadLog();			

			int j = 0;
			for(int i = logLines.Length - 1; i >= 0; i--)
			{
				if (j == 5)
				{
					break;
				}

				string[] lineParts = logLines[i].Split('-');
				string dateString = lineParts[0];
				double value = Double.Parse(lineParts[2]);
				DateTime date = Convert.ToDateTime(dateString);

				GraphValues[j] = (value - possibleValueMin) * (graphMax - graphMin) / (possibleValueMax - possibleValueMin) + graphMin;
				GraphDates[j] = date;

				j++;
			}
		}
	}
}
