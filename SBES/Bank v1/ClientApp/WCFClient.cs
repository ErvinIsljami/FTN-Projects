using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp
{
    public class WCFClient : ChannelFactory<IUserService>, IUserService, IDisposable
    {
        IUserService factory;
        public WCFClient(NetTcpBinding binding, EndpointAddress address) : base(binding, address)
        {
            factory = this.CreateChannel();
        }
        public void ApplyForCredit(string userName, double amount)
        {
            try
            {
                factory.ApplyForCredit(userName, amount);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void ApproveAccount(string userName)
        {
            try
            {
                factory.ApproveAccount(userName);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void ApproveCredit(string userName)
        {
            try
            {
                factory.ApproveCredit(userName);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void DenyAccount(string userName)
        {
            try
            {
                factory.DenyAccount(userName);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void DenyCredit(string userName)
        {
            try
            {
                factory.DenyCredit(userName);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void Dispose()
        {
            if (factory != null)
            {
                factory = null;
            }

            this.Close();
        }

        public void ExecuteTransaction(string userName, double amount)
        {
            try
            {
                factory.ExecuteTransaction(userName, amount);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public List<Account> GetAllAccountRequests()
        {
            try
            {
                return factory.GetAllAccountRequests();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public List<Credit> GetAllCreditRequests()
        {
            try
            {
                return factory.GetAllCreditRequests();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public void MakeNewAccount(string userName)
        {
            try
            {
                factory.MakeNewAccount(userName);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}