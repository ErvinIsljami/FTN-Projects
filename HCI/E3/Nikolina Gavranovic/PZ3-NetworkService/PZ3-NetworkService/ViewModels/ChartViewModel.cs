using PZ3_NetworkService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PZ3_NetworkService.ViewModels
{
	public class ChartViewModel : BindableBase
	{
		private ChartData chartData1;
		private ChartData chartData2;
		private ChartData chartData3;
		private ChartData chartData4;
		private ChartData chartData5;
		private RoadType selectedRoadType;
		private int graphMin;   // Pixels of minimum value on the graph
		private int graphMax;   // Pixels of maximum value on the graph
		private int possibleValueMin;
		private int possibleValueMax;

		public ChartData ChartData1
		{
			get { return chartData1; }
			set
			{
				SetField(ref chartData1, value);
			}
		}
		public ChartData ChartData2
		{
			get { return chartData2; }
			set
			{
				SetField(ref chartData2, value);
			}
		}
		public ChartData ChartData3
		{
			get { return chartData3; }
			set
			{
				SetField(ref chartData3, value);
			}
		}
		public ChartData ChartData4
		{
			get { return chartData4; }
			set
			{
				SetField(ref chartData4, value);
			}
		}
		public ChartData ChartData5
		{
			get { return chartData5; }
			set
			{
				SetField(ref chartData5, value);
			}
		}
		public List<ChartData> ChartDatas;
		public RoadType SelectedRoadType
		{
			get { return selectedRoadType; }
			set
			{
				SetField(ref selectedRoadType, value);
			}
		}

		public MyICommand ShowHistoryChartCommand { get; set; }

		public ChartViewModel()
		{
			ShowHistoryChartCommand = new MyICommand(ShowHistoryChart);
			ChartDatas = new List<ChartData>(5);
			ChartData1 = new ChartData(150, DateTime.MinValue);
			ChartData2 = new ChartData(150, DateTime.MinValue);
			ChartData3 = new ChartData(150, DateTime.MinValue);
			ChartData4 = new ChartData(150, DateTime.MinValue);
			ChartData5 = new ChartData(150, DateTime.MinValue);
			ChartDatas.Add(ChartData1);
			ChartDatas.Add(ChartData2);
			ChartDatas.Add(ChartData3);
			ChartDatas.Add(ChartData4);
			ChartDatas.Add(ChartData5);
			graphMin = 150;
			graphMax = 470;
			possibleValueMin = 3000;
			possibleValueMax = 19000;
		}

		public void ShowHistoryChart()
		{
			ResetGraph();

			// string part is the name of valve
			List<ChartData> lastFiveMeasurements = GetLastFiveMeasurements();

			// So we can easily set the correct values
			for (int i = 0; i < lastFiveMeasurements.Count; i++)
			{
				ChartDatas[i].Value = lastFiveMeasurements[i].Value;
				ChartDatas[i].CreationDate = lastFiveMeasurements[i].CreationDate;
			}
		}
		private List<ChartData> GetLastFiveMeasurements()
		{
			string[] logLines = Logger.LoadLog();
			List<string> reversedMeasurements = logLines.Reverse().ToList();
			List<ChartData> lastFive = new List<ChartData>();

			int i = 0;
			foreach (string line in reversedMeasurements)
			{
				if (i == 5)
				{
					break;
				}

				string[] properties = line.Split(',');
				string roadTypeString = properties[1].Split(':')[1];
				RoadType roadType = (RoadType)Enum.Parse(typeof(RoadType), roadTypeString);

				if(roadType == SelectedRoadType)
				{
					string dateString = properties[0];
					double value = Double.Parse(properties[2].Split(':')[1]);
					DateTime creationDate = Convert.ToDateTime(dateString);

					double adjustedValue = (value - possibleValueMin) * (graphMax - graphMin) / (possibleValueMax - possibleValueMin) + graphMin;
					ChartData cd = new ChartData(adjustedValue, creationDate);

					lastFive.Add(cd);
					i++;
				}
			}

			return lastFive;
		}
		private void ResetGraph()
		{
			foreach (var cd in ChartDatas)
			{
				cd.Value = 150;
				cd.CreationDate = DateTime.MinValue;
			}
		}
	}
}
