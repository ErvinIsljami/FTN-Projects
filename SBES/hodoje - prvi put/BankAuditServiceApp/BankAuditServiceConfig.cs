using System.Configuration;

namespace BankAuditServiceApp
{
    internal static class BankAuditServiceConfig
    {
        public const string LogNameConfigName = "LogName";
        public const string BankAuditServiceAddressConfigName = "BankAuditServiceAddress";
        public const string BankAuditServiceEndpointNameConfigName = "BankAuditServiceEndpointName";

        static BankAuditServiceConfig()
        {
            LogName = ConfigurationManager.AppSettings[LogNameConfigName];
            BankAuditServiceAddress = ConfigurationManager.AppSettings[BankAuditServiceAddressConfigName];
            BankAuditServiceEndpointName = ConfigurationManager.AppSettings[BankAuditServiceEndpointNameConfigName];
        }

        public static string LogName { get; }
        public static string BankAuditServiceAddress { get; }
        public static string BankAuditServiceEndpointName { get; }
    }
}