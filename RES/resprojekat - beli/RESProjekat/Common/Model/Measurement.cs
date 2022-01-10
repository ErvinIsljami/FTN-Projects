using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Model
{
	public class Measurement
	{
		private string generatorId;
		private DateTime date;
		private double value;

		public Measurement()
		{

		}

		public Measurement(string generatorId, DateTime date, double value)
		{
			this.generatorId = generatorId;
			this.date = date;
			this.value = value;
		}

		public string GeneratorId { get => generatorId; set => generatorId = value; }
		public DateTime Date { get => date; set => date = value; }
		public double Value { get => value; set => this.value = value; }
	}
}
