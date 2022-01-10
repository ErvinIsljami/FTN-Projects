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
		private Valve selectedValve;
		private ChartData chartData1;
		private ChartData chartData2;
		private ChartData chartData3;
		private ChartData chartData4;
		private ChartData chartData5;

		public Valve SelectedValve
		{
			get { return selectedValve; }
			set
			{
				SetField(ref selectedValve, value);
			}
		}
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
		public MyICommand ShowHistoryChartCommand { get; set; }

		public ChartViewModel()
		{
			ShowHistoryChartCommand = new MyICommand(OnShowHistoryChart);
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

		private void OnShowHistoryChart()
		{
			ResetGraph();

			if (SelectedValve != null)
			{
				// string part is the name of valve
				Dictionary<string, List<ChartData>> valvesAndValues = GetValvesAndValues();
				if(valvesAndValues.TryGetValue(SelectedValve.Name, out List<ChartData> data))
				{
					// First elements in data are the latest measurements
					List<ChartData> tempList = new List<ChartData>(5);
					for(int i = 0; i < data.Count; i++)
					{
						if(i == 5)
						{
							break;
						}
						else
						{
							tempList.Add(data[i]);
						}
					}

					// Reverese the measurement list
					tempList.Reverse();

					// So we can easily set the correct values
					for(int i = 0; i < tempList.Count; i++)
					{
						// Multiplying with 20 so ti suits the graph
						ChartDatas[i].Value = data[i].Value * 20;
						ChartDatas[i].CreationDate = data[i].CreationDate;
					}
				}
			}
		}
		private Dictionary<string, List<ChartData>> GetValvesAndValues()
		{
			Dictionary<string, List<ChartData>> valveNameAndValueMap = new Dictionary<string, List<ChartData>>();
			string[] logLines = Logger.LoadLog();

			List<string> reversedMeasurements = logLines.Reverse().ToList();

			foreach(string line in reversedMeasurements)
			{
				//					 CreationDate               Name     Value
				// Example log line: 22.12.2019. 18:47:35,Valve:asdf,Value:1

				string[] properties = line.Split(',');
				string dateString = properties[0];
				string name = properties[1].Split(':')[1];
				double value = Double.Parse(properties[2].Split(':')[1]);
				DateTime creationDate = Convert.ToDateTime(dateString);

				ChartData cd = new ChartData(value, creationDate);

				if(valveNameAndValueMap.TryGetValue(name, out List<ChartData> list))
				{
					list.Add(cd);
				}
				else
				{
					valveNameAndValueMap.Add(name, new List<ChartData>());
					valveNameAndValueMap[name].Add(cd);
				}
			}

			return valveNameAndValueMap;
		}
		private void ResetGraph()
		{
			foreach(var cd in ChartDatas)
			{
				cd.Value = 0;
				cd.CreationDate = DateTime.MinValue;
			}
		}
	}
}
