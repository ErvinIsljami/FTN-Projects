using System;
using System.Security.Principal;

namespace BankAuditServiceApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var principal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
            if (!principal.IsInRole("BankServices"))
            {
                Console.WriteLine($"Only user in BankServices role can run {nameof(BankServiceApp)}");
                Console.WriteLine("Press any key to exit...");
                Console.ReadLine();
                return;
            }

            var bankAuditServiceHost = new BankAuditServiceHost();
            bankAuditServiceHost.OpenService();
            Console.WriteLine("BankAuditService is open...");
            Console.ReadLine();
            bankAuditServiceHost.CloseService();
        }
    }
}