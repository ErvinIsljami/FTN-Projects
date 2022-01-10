using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PZ3_NetworkService.Models
{
	public class ChartData : BindableBase
	{
		private double value;
		private DateTime creationDate;

		public ChartData() { }

		public ChartData(double value, DateTime creationDate)
		{
			this.value = value;
			this.creationDate = creationDate;
		}

		public double Value
		{
			get { return value; }
			set
			{
				SetField(ref this.value, value);
			}
		}
		public DateTime CreationDate
		{
			get { return creationDate; }
			set
			{
				SetField(ref creationDate, value);
			}
		}
	}
}
