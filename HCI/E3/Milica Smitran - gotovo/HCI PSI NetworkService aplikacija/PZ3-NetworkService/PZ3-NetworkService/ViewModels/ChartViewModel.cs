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

		public ChartViewModel()
		{
			ChartDatas = new List<ChartData>(5);
			ChartData1 = new ChartData();
			ChartData2 = new ChartData();
			ChartData3 = new ChartData();
			ChartData4 = new ChartData();
			ChartData5 = new ChartData();
			ChartDatas.Add(ChartData1);
			ChartDatas.Add(ChartData2);
			ChartDatas.Add(ChartData3);
			ChartDatas.Add(ChartData4);
			ChartDatas.Add(ChartData5);
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
				//					 CreationDate               Name     Value
				// Example log line: 22.12.2019. 18:47:35,Valve:asdf,Value:1
				if(i == 5)
				{
					break;
				}

				string[] properties = line.Split(',');
				string dateString = properties[0];
				string name = properties[1].Split(':')[1];
				double value = Double.Parse(properties[2].Split(':')[1]);
				DateTime creationDate = Convert.ToDateTime(dateString);

				ChartData cd = new ChartData(value, creationDate);

				lastFive.Add(cd);
				i++;
			}

			return lastFive;
		}
		private void ResetGraph()
		{
			foreach (var cd in ChartDatas)
			{
				cd.Value = 0;
				cd.CreationDate = DateTime.MinValue;
			}
		}
	}
}
