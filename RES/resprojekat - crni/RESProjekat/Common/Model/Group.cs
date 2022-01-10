using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Model
{
	public class Group
	{
		string id;
		int numOfUnits;
		double currentProduction;
		double maxProduction;

		public Group()
		{

		}

		public Group(string id)
		{
			this.id = id;
			this.numOfUnits = 0;
			this.currentProduction = 0;
			this.maxProduction = 0;
		}

		public string Id { get => id; set => id = value; }
		public int NumOfUnits { get => numOfUnits; set => numOfUnits = value; }
		public double CurrentProduction { get => currentProduction; set => currentProduction = value; }
		public double MaxProduction { get => maxProduction; set => maxProduction = value; }
	}
}
