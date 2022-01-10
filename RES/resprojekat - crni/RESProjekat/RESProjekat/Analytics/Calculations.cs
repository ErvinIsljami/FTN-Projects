using Common;
using Common.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace LocalController.Analytics
{
	public class Calculations
	{
		public Window Lc { get; set; }

		public Calculations(Window lc)
		{
			Lc = lc;
		}

		#region These methods are called by Local Controllers
		#region Generator Methods
		public double MeanPowerGenerator(string lcId, string generatorId, DateTime fromDate, DateTime toDate)
		{
			List<Measurement> geneneratorSpecificMeasurementHistory = GetSpecificGeneratorMeasurements(lcId, generatorId, fromDate, toDate);	

			double measurementSum = 0;
			foreach(Measurement m in geneneratorSpecificMeasurementHistory)
			{
				measurementSum += m.Value;
			}

			return measurementSum / geneneratorSpecificMeasurementHistory.Count;
		}

		public double MinPowerGenerator(string lcId, string generatorId, DateTime fromDate, DateTime toDate)
		{
			List<Measurement> measurements = GetSpecificGeneratorMeasurements(lcId, generatorId, fromDate, toDate);
			return measurements.Min(g => g.Value);
		}
		
		public double MaxPowerGenerator(string lcId, string generatorId, DateTime fromDate, DateTime toDate)
		{
			List<Measurement> measurements = GetSpecificGeneratorMeasurements(lcId, generatorId, fromDate, toDate);
			return measurements.Max(g => g.Value);
		}

		private List<Measurement> GetSpecificGeneratorMeasurements(string lcId, string generatorId, DateTime fromDate, DateTime toDate)
		{
			var lc = Lc as dynamic;
			if(Lc != null)
			{
				Generator g = (lc.LocalGenerators as ObservableCollection<Generator>).FirstOrDefault(gen => gen.Id == generatorId);
				if (g != null)
				{
					List<Measurement> specificMeasurements = new List<Measurement>();
					Measurement[] measurementArray = new Measurement[g.MeasurementHistory.Count];

					g.MeasurementHistory.CopyTo(measurementArray);
					var measurementHistory = measurementArray.ToList();

					foreach (Measurement m in measurementHistory)
					{
						if (fromDate == DateTime.MinValue || toDate == DateTime.MinValue)
						{
							if (fromDate == DateTime.MinValue && toDate == DateTime.MinValue)
							{
								specificMeasurements.Add(m);
							}
							else if (fromDate == DateTime.MinValue && toDate != DateTime.MinValue)
							{
								if (m.Date <= toDate)
								{
									specificMeasurements.Add(m);
								}
							}
							else if (fromDate != DateTime.MinValue && toDate == DateTime.MinValue)
							{
								if (m.Date >= fromDate)
								{
									specificMeasurements.Add(m);
								}
							}
						}
						else
						{
							if ((m.Date > fromDate) && (m.Date < toDate))
							{
								specificMeasurements.Add(m);
							}
						}
					}
					return specificMeasurements;
				}
				return new List<Measurement>();
			}
			return new List<Measurement>();
		}
		#endregion

		#region Group Methods
		public double MeanPowerGroup(string lcId, string groupId, DateTime fromDate, DateTime toDate)
		{
			double totalSum = 0;
			Dictionary<string, List<Measurement>> groupGeneratorsAndMeasurements = new Dictionary<string, List<Measurement>>();
			List<double> particularGeneratorPowerOutput = new List<double>();

			var lc = Lc as dynamic;
			if (lc != null)
			{
				foreach (Generator gen in (lc.LocalGenerators as ObservableCollection<Generator>).Where(generator => generator.Group == groupId))
				{
					groupGeneratorsAndMeasurements.Add(gen.Id, new List<Measurement>());
					groupGeneratorsAndMeasurements[gen.Id] = GetSpecificGeneratorMeasurements(lcId, gen.Id, fromDate, toDate);

					double measurementSum = 0;
					foreach (Measurement m in groupGeneratorsAndMeasurements[gen.Id])
					{
						measurementSum += m.Value;
					}
					particularGeneratorPowerOutput.Add(measurementSum);
				}

				foreach (double genTotal in particularGeneratorPowerOutput)
				{
					totalSum += genTotal;
				}

				return totalSum / particularGeneratorPowerOutput.Count;
			}
			return totalSum;
		}

		public double MinPowerGroup(string lcId, string groupId, DateTime fromDate, DateTime toDate)
		{
			Dictionary<string, List<Measurement>> generatorAndMeasurements = GetSpecificGroupMeasurements(lcId, groupId, fromDate, toDate);
			List<Measurement> generatorMinMeasurements = generatorAndMeasurements.Min(pair => pair.Value).ToList();
			return generatorMinMeasurements.Min(m => m.Value);
		}

		public double MaxPowerGroup(string lcId, string groupId, DateTime fromDate, DateTime toDate)
		{
			Dictionary<string, List<Measurement>> generatorAndMeasurements = GetSpecificGroupMeasurements(lcId, groupId, fromDate, toDate);
			List<Measurement> generatorMaxMeasurements = generatorAndMeasurements.Max(pair => pair.Value).ToList();
			return generatorMaxMeasurements.Min(m => m.Value);
		}

		private Dictionary<string, List<Measurement>> GetSpecificGroupMeasurements(string lcId, string groupId, DateTime fromDate, DateTime toDate)
		{
			var lc = Lc as dynamic;
			if(lc != null)
			{
				Dictionary<string, List<Measurement>> generatorsAndMeasurements = new Dictionary<string, List<Measurement>>();
				foreach (Generator gen in (lc.LocalGenerators as ObservableCollection<Generator>).Where(generator => generator.Group == groupId))
				{
					generatorsAndMeasurements.Add(gen.Id, new List<Measurement>());
					generatorsAndMeasurements[gen.Id] = GetSpecificGeneratorMeasurements(lcId, gen.Id, fromDate, toDate);
				}
				return generatorsAndMeasurements;
			}
			else
			{
				return new Dictionary<string, List<Measurement>>();
			}			
		}
		#endregion
		#endregion
	}
}
