using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Model
{
	public class Loan : IdentifiedObject
	{
		public Loan()
		{

		}

		public Loan(long id) : base(id)
		{
		}

		public double Amount { get; set; }
		public int Months { get; set; }

		public long UseId { get; set; }
		public User User { get; set; }
	}
}
