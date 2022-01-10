using Common.Communication.AuthorizationPolicy;
using Common.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Policy;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Security;
using Common.CertificateManagement;
using System.Security.Principal;
using Common.Communication;

namespace BankService
{
	class Program
	{
		static void Main(string[] args)
		{
			string srvCertCN = StringFormatter.ParseName(WindowsIdentity.GetCurrent().Name);
			//string srvCertCN = "bankservice";
			string address = $"{BankServiceConfig.BankServiceAddress}/{BankServiceConfig.UserServiceEndpointName}";
			BankingService bankingService = new BankingService();

			Console.WriteLine("Wait for sector connections...");
			bankingService.StartListeningForSectorConnections();
			bankingService.StartListeningForCheckAlivePings();

			ServiceHost userHost = CreateHost(address, typeof(IUserService), bankingService, srvCertCN, "UserService");

			address = $"{BankServiceConfig.BankServiceAddress}/{BankServiceConfig.AdminServiceEndpointName}";
			ServiceHost adminHost = CreateHost(address, typeof(IAdminService), bankingService, srvCertCN, "AdminService");

			Console.ReadLine();
		}

		private static ServiceHost CreateHost(string address, Type contract, BankingService bankingService, string srvCertCN, string serviceName)
		{
			ServiceHost host = new ServiceHost(bankingService);
			host.AddServiceEndpoint(contract, CreateCertificateBinding(), address);

			InitializeHostForCertificateUse(host);
			InitializeHostForCustomAuthorizationPolicy(host);

			///Set appropriate service's certificate on the host. Use CertManager class to obtain the certificate based on the "srvCertCN"
			X509Certificate2 certificate;
			if (CertificateStorageReader.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, srvCertCN, out certificate))
			{
				host.Credentials.ServiceCertificate.Certificate = certificate;
				host.Open();
				Console.WriteLine($"{serviceName} started...");
			}
			else
			{
				Console.WriteLine($"{serviceName} has no valid certificates...");
			}

			return host;
		}

		private static void InitializeHostForCustomAuthorizationPolicy(ServiceHost host)
		{
			host.Authorization.PrincipalPermissionMode = PrincipalPermissionMode.Custom;
			List<IAuthorizationPolicy> policies = new List<IAuthorizationPolicy>();
			policies.Add(new OUAuthorizationPolicy());
			host.Authorization.ExternalAuthorizationPolicies = policies.AsReadOnly();
		}

		private static void InitializeHostForCertificateUse(ServiceHost host)
		{
			host.Credentials.ClientCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.ChainTrust;
			host.Credentials.ClientCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;
		}

		private static NetTcpBinding CreateCertificateBinding()
		{
			NetTcpBinding binding = new NetTcpBinding();
			binding.CloseTimeout = binding.OpenTimeout = binding.ReceiveTimeout = binding.SendTimeout = new TimeSpan(1, 0, 0, 0);
			binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;
			binding.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;
			return binding;
		}
	}
}
