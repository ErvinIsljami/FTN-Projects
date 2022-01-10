using BankServiceApp.BankContracts;
using Common.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace BankServiceApp
{
    public class ServiceHosts
    {
        private ServiceHost cardHost = null;
        private ServiceHost transationHost = null;
        private string cardAddress = String.Empty;
        private string transationAddress = String.Empty;
        private NetTcpBinding binding = new NetTcpBinding();
        public ServiceHosts()
        {
            InitBinding();
            cardHost = new ServiceHost(typeof(BankMasterCardService));
            cardHost.AddServiceEndpoint(typeof(IBankMasterCardService), binding, cardAddress);
            transationHost = new ServiceHost(typeof(BankTransactionService));
            transationHost.AddServiceEndpoint(typeof(IBankTransactionService), binding, transationAddress);

            CardOpen();
            Console.WriteLine("BankMasterCardService host opened...");
            cardHost.Open();
            Console.WriteLine("BankMasterTransationService host opened...");
            transationHost.Open();

            Console.ReadLine();
            cardHost.Close();
            transationHost.Close();
        }
       private void InitBinding()
        {
            binding.Security.Mode = SecurityMode.Transport;
            binding.Security.Transport.ProtectionLevel =
            System.Net.Security.ProtectionLevel.EncryptAndSign;

            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
        }
        private void CardOpen()
        {
            try
            {
                cardHost.Open();
            }
            catch (Exception ex)
            {

                Console.WriteLine("cardHost failed to open with an error: {0}",ex.Message);
            }
        }

        private void CardClose()
        {
            try
            {
                cardHost.Close();
            }
            catch (Exception ex)
            {

                Console.WriteLine("cardHost failed to close with an error: {0}", ex.Message);
            }
        }

        private void TransationOpen()
        {
            try
            {
                transationHost.Open();
            }
            catch (Exception ex)
            {

                Console.WriteLine("transationHost failed to open with an error: {0}", ex.Message);
            }
        }

        private void TransationClose()
        {
            try
            {
                transationHost.Open();
            }
            catch (Exception ex)
            {

                Console.WriteLine("transationHost failed to close with an error: {0}", ex.Message);
            }
        }
    }
}
