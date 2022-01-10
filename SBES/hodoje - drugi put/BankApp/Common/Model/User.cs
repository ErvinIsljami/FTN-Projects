using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Model
{
	public class User : IdentifiedObject
	{
		public User() : base() { }
		public User(string username) : base()
		{
			Username = username;
			Accounts = new HashSet<BankAccount>();
		}

		public string Username { get; set; }
		public virtual ICollection<BankAccount> Accounts { get; set; }

		public long LoanId { get; set; }
		public Loan Loan { get; set; }
	}
}
