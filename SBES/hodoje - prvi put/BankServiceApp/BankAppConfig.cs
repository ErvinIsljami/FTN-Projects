using System.Collections.Generic;
using System.Configuration;

namespace BankServiceApp
{
    internal static class BankAppConfig
    {
        public const string ServiceAddressConfigName = "Address";
        public const string InstanceNumberConfigName = "Instances";
        public const string ReplicatorConfigName = "ReplicationEndpointName";
        public const string MasterCardServiceConfigName = "MasterCardServiceEndpointName";
        public const string TransactionServiceConfigName = "TransactionServiceEndpointName";
        public const string BankAuditServiceAddressConfigName = "BankAuditServiceAddress";
        public const string BankTransactionServiceCertificatePathConfigName = "ServiceCertificatePath";
        public const string BankTransactionServiceCertificateSubjectNameConfigName = "ServiceCertificateSubjectName";
        public const string BankTransactionServiceCertificatePasswordConfigName = "ServiceCertificatePass";
        public const string CACertificatePathConfigName = "CACertificatePath";
        public const string CACertificatePassConfigName = "CACertificatePass";
        public const string BankCachePathConfigName = "BankCachePath";
        public const string BankServiceNameConfigName = "BankServiceName";
        public const string TimeIntervalForAuditCheckConfigName = "TimeIntervalForWithdrawalAuditCheck";
        public const string NumberOfWithdrawalAuditCheckConfigName = "NumberOfWithdrawalForAuditCheck";

        static BankAppConfig()
        {
            ReplicatorName = ConfigurationManager.AppSettings.Get(ReplicatorConfigName);
            MasterCardServiceName = ConfigurationManager.AppSettings.Get(MasterCardServiceConfigName);
            TransactionServiceName = ConfigurationManager.AppSettings.Get(TransactionServiceConfigName);

            InstanceNo = int.Parse(ConfigurationManager.AppSettings.Get(InstanceNumberConfigName));
            Endpoints = new List<string>(InstanceNo);

            for (var i = 0; i < InstanceNo; i++)
                Endpoints.Add(ConfigurationManager.AppSettings.Get($"{ServiceAddressConfigName}{i}"));

            BankAuditServiceEndpoint = ConfigurationManager.AppSettings.Get(BankAuditServiceAddressConfigName);
            BankTransactionServiceCertificatePath =
                ConfigurationManager.AppSettings.Get(BankTransactionServiceCertificatePathConfigName);
            BankTransactionServiceSubjectName =
                ConfigurationManager.AppSettings.Get(BankTransactionServiceCertificateSubjectNameConfigName);
            BankTransactionServiceCertificatePassword =
                ConfigurationManager.AppSettings.Get(BankTransactionServiceCertificatePasswordConfigName);
            CACertificatePath = ConfigurationManager.AppSettings.Get(CACertificatePathConfigName);
            CACertificatePass = ConfigurationManager.AppSettings.Get(CACertificatePassConfigName);
            BankCachePath = ConfigurationManager.AppSettings.Get(BankCachePathConfigName);
            BankName = ConfigurationManager.AppSettings.Get(BankServiceNameConfigName);

            TimeIntervalForAudidChecking =
                int.Parse(ConfigurationManager.AppSettings.Get(TimeIntervalForAuditCheckConfigName));
            WithdrawLimitForAudit =
                int.Parse(ConfigurationManager.AppSettings.Get(NumberOfWithdrawalAuditCheckConfigName));
        }

        public static string ReplicatorName { get; }

        public static string MasterCardServiceName { get; }

        public static string TransactionServiceName { get; }

        public static int InstanceNo { get; }

        public static List<string> Endpoints { get; }

        public static string BankAuditServiceEndpoint { get; }

        public static string BankTransactionServiceCertificatePath { get; }

        public static string BankTransactionServiceSubjectName { get; }

        public static string BankTransactionServiceCertificatePassword { get; }

        public static int TimeIntervalForAudidChecking { get; }

        public static int WithdrawLimitForAudit { get; }

        public static string CACertificatePath { get; }

        public static string CACertificatePass { get; }

        public static string BankCachePath { get; }

        public static string BankName { get; }

        public static string MyEndpoint { get; set; } = null;
    }
}