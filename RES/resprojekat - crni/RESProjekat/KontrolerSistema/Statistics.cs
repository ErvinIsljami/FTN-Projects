using Common;
using Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemController
{
	public class Statistics
	{
		private List<Measurement> GetScSpecificGeneratorMeasurements(LC lc, string generatorId, DateTime fromDate, DateTime toDate)
		{
			if (lc != null)
			{
				Generator g = lc.Generators.FirstOrDefault(gen => gen.Id == generatorId);
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
							if (m.Date >= fromDate && m.Date <= toDate)
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

		private double CalculateAllScGeneratorsMeanPower(string lcId, string generatorId, DateTime fromDate, DateTime toDate)
		{
			double measurementSum = 0;

			var scLc = DataAccessHelper.Instance.LCs.FirstOrDefault(l => l.Guid == lcId);
			if (scLc != null)
			{
				List<Measurement> geneneratorSpecificMeasurementHistory = GetScSpecificGeneratorMeasurements(scLc, generatorId, fromDate, toDate);
				foreach (Measurement m in geneneratorSpecificMeasurementHistory)
				{
					measurementSum += m.Value;
				}

				return measurementSum / geneneratorSpecificMeasurementHistory.Count;
			}
			return measurementSum = Math.Round(measurementSum, 2);
		}

		private IEnumerable<DateTime> EachMinute(DateTime fromDate, DateTime toDate)
		{
			for (var iteratorDate = fromDate; iteratorDate <= toDate; iteratorDate = iteratorDate.AddMinutes(1))
			{
				yield return iteratorDate;
			}
		}

		public Dictionary<Generator, Dictionary<DateTime, double>> AllGeneratorsLoadForecast()
		{
			Dictionary<Generator, Dictionary<DateTime, double>> allGeneratorsLoadForecastResult = new Dictionary<Generator, Dictionary<DateTime, double>>();
			foreach (LC lc in DataAccessHelper.Instance.LCs)
			{
				foreach (Generator g in lc.Generators)
				{
					allGeneratorsLoadForecastResult.Add(g, new Dictionary<DateTime, double>(180));
					double generatorMeanPower = CalculateAllScGeneratorsMeanPower(lc.Guid, g.Id, DateTime.MinValue, DateTime.MinValue);
					Random percentageGenerator = new Random();
					double nextPercentage = 0;
					double calculatedPercentage = 0;

					foreach (DateTime dateTime in EachMinute(DateTime.Now, DateTime.Now.AddHours(3)))
					{
						nextPercentage = percentageGenerator.Next(-5, 5);
						calculatedPercentage = nextPercentage / 100;
						allGeneratorsLoadForecastResult[g].Add(dateTime, generatorMeanPower + (generatorMeanPower * calculatedPercentage));
					}
				}
			}
			return allGeneratorsLoadForecastResult;
		}

		public Dictionary<Generator, List<Tuple<DateTime, double>>> LoadFollowing(Dictionary<Generator, Dictionary<DateTime, double>> allGeneratorsLoadForecast)
		{
			Dictionary<Generator, List<Tuple<DateTime, double>>> loadFollowingResult = new Dictionary<Generator, List<Tuple<DateTime, double>>>();

			// Iterate over all forecasts for each generator
			foreach (var generatorAndForecastPair in allGeneratorsLoadForecast)
			{
				loadFollowingResult.Add(generatorAndForecastPair.Key, new List<Tuple<DateTime, double>>());

				Random percentageGenerator = new Random();
				double nextPercentage = 0;
				double calculatedPercentage = 0;

				// Iterate over each concrete forecast value (each minute)
				foreach (var datetimeAndConcreteValuePair in generatorAndForecastPair.Value)
				{
					DateTime followingStartDate = datetimeAndConcreteValuePair.Key;
					DateTime followingEndDate = followingStartDate.AddSeconds(10 * 10);
					for (DateTime iteratorDate = followingStartDate; iteratorDate <= followingEndDate; iteratorDate = iteratorDate.AddSeconds(10))
					{
						nextPercentage = percentageGenerator.Next(-1, 1);
						calculatedPercentage = nextPercentage / 100;
                        double newPredictedValue = datetimeAndConcreteValuePair.Value + (datetimeAndConcreteValuePair.Value * calculatedPercentage);
                        newPredictedValue = Math.Round(newPredictedValue, 2);
                        loadFollowingResult[generatorAndForecastPair.Key].Add(new Tuple<DateTime, double>(iteratorDate, newPredictedValue));
					}
				}
			}
			return loadFollowingResult;
		}
	}
}
