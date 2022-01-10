using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditingService
{
	class Program
	{
		static void Main(string[] args)
		{
			AuditServiceServiceHost auditHost = new AuditServiceServiceHost();
			auditHost.OpenService();

			Console.WriteLine("Audit service started...");
			Console.ReadLine();
		}
	}
}
