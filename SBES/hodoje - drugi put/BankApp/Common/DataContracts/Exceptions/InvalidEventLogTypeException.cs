using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DataContracts.Exceptions
{
	[Serializable]
	public class InvalidEventLogTypeException : Exception
	{
		public InvalidEventLogTypeException() : base("Invalid EventLogType specified.")
		{
		}

		public InvalidEventLogTypeException(string message) : base(message)
		{
		}
	}
}
