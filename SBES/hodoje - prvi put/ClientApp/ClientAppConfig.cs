using System.Collections.Generic;
using System.Configuration;

namespace ClientApp
{
    internal static class ClientAppConfig
    {
        public const string ServiceAddressConfigName = "Address";
        public const string InstanceNumberConfigName = "Instances";
        public const string MasterCardServiceConfigName = "MasterCardServiceEndpointName";
        public const string TransactionServiceConfigName = "TransactionServiceEndpointName";
        public const string ServiceCertificateCNName = "ServiceCertificateCN";
        public const string CertificatePathName = "CertificatePath";

        static ClientAppConfig()
        {
            MasterCardServiceName = ConfigurationManager.AppSettings.Get(MasterCardServiceConfigName);
            TransactionServiceName = ConfigurationManager.AppSettings.Get(TransactionServiceConfigName);
            ServiceCertificateCN = ConfigurationManager.AppSettings.Get(ServiceCertificateCNName);
            CertificatePath = ConfigurationManager.AppSettings.Get(CertificatePathName);
            InstanceNo = int.Parse(ConfigurationManager.AppSettings.Get(InstanceNumberConfigName));
            Endpoints = new List<string>(InstanceNo);
            for (var i = 0; i < InstanceNo; i++)
                Endpoints.Add(ConfigurationManager.AppSettings.Get($"{ServiceAddressConfigName}{i}"));
            MasterCardServiceAddress = new List<string>(InstanceNo);
            TransactionServiceAddress = new List<string>(InstanceNo);
            for (var i = 0; i < InstanceNo; i++)
            {
                MasterCardServiceAddress.Add($"{Endpoints[i]}/{MasterCardServiceName}");
                TransactionServiceAddress.Add($"{Endpoints[i]}/{TransactionServiceName}");
            }
        }

        public static List<string> MasterCardServiceAddress { get; }
        public static List<string> TransactionServiceAddress { get; }
        public static string ServiceCertificateCN { get; }
        public static string CertificatePath { get; }

        public static int InstanceNo { get; }
        public static List<string> Endpoints { get; }
        public static string MasterCardServiceName { get; }

        public static string TransactionServiceName { get; }
    }
}