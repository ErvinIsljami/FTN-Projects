using Common;
using SecurityManager;
using System;
using System.Collections.Generic;
using System.IdentityModel.Policy;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace BankingService
{
    class Program
    {
        static void Main(string[] args)
        {
            Account a = new Account("test", 5000);
            List<Account> list = new List<Account>() { a };
            //XmlReaderxmlReaderWriter.SerializeObject<List<Account>>(list, "../../../Resources/dataBase.xml");
            NetTcpBinding binding = new NetTcpBinding();
            string address = "net.tcp://localhost:9999/UserService";

            ServiceHost host = new ServiceHost(typeof(UserServices));
            host.AddServiceEndpoint(typeof(IUserService), binding, address);

            // podesavamo da se koristi MyAuthorizationManager umesto ugradjenog
            //host.Authorization.ServiceAuthorizationManager = new CustomAuthorizationManager();

            // podesavamo custom polisu, odnosno nas objekat principala
            host.Authorization.PrincipalPermissionMode = PrincipalPermissionMode.Custom;
            List<IAuthorizationPolicy> policies = new List<IAuthorizationPolicy>();
            policies.Add(new CustomAuthorizationPolicy());
            host.Authorization.ExternalAuthorizationPolicies = policies.AsReadOnly();

            host.Open();
            Console.WriteLine("WCFService is opened. Press <enter> to finish...");
            Console.ReadLine();

            host.Close();
        }
    }
}
