using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Xml.Serialization;

namespace Common.Model
{
	[DataContract]
	public class Generator
	{
		string id;
		UnitTypeEnum unitType;
		double currentActivePower;
		double minPower;
		double maxPower;
		ControlTypeEnum controlType;
		double workPrice;
		string group;
		List<Measurement> measurementHistory;

		public Generator()
		{
		}

		public Generator(string id, UnitTypeEnum unitType, double currentActivePower, double minPower, double maxPower, ControlTypeEnum controlType, double workPrice, string group)
		{
			this.id = id;
			this.unitType = unitType;
			this.currentActivePower = currentActivePower;
			this.minPower = minPower;
			this.maxPower = maxPower;
			this.controlType = controlType;
			this.workPrice = workPrice;
			this.group = group;
			this.measurementHistory = new List<Measurement>();
		}

		[DataMember]
		public string Id { get => id; set => id = value; }
		[DataMember]
		public UnitTypeEnum UnitType { get => unitType; set => unitType = value; }
		[DataMember]
		public double CurrentActivePower { get => currentActivePower; set => currentActivePower = value; }
		[DataMember]
		public double MinPower { get => minPower; set => minPower = value; }
		[DataMember]
		public double MaxPower { get => maxPower; set => maxPower = value; }
		[DataMember]
		public ControlTypeEnum ControlType { get => controlType; set => controlType = value; }
		[DataMember]
		public double WorkPrice { get => workPrice; set => workPrice = value; }
		[DataMember]
		public string Group { get => group; set => group = value; }
		[DataMember]
		[XmlArray("MeasurementHistory")]
		public List<Measurement> MeasurementHistory { get => measurementHistory; set => measurementHistory = value; }

		public bool AzurirajAktivnuSnagu()
		{
			double novaVrednost = 0;
			Random r = new Random();
			if (controlType == ControlTypeEnum.LOCAL)
			{
				do
				{
					novaVrednost = r.Next((int)minPower, (int)maxPower);

				} while (novaVrednost < currentActivePower * 0.9 || novaVrednost > currentActivePower * 1.1);
				currentActivePower = novaVrednost;
			}
			else
			{
				//uzmi vrednost od modula 2
			}

			AddMeasurementToHistory();

			return true;
		}

		public void ManuallySetActivePower(double newActivePower)
		{
			CurrentActivePower = newActivePower;
			AddMeasurementToHistory();
		}

		private void AddMeasurementToHistory()
		{
			Measurement newMeasurement = new Measurement(Id, DateTime.Now, currentActivePower);
			MeasurementHistory.Add(newMeasurement);
		}
	}
}
