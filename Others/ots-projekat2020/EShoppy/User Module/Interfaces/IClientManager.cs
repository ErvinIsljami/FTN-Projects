using EShoppy.Finantial_Module.Interfaces;
using EShoppy.Transactional_Module.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShoppy.User_Module.Interfaces
{
    public interface IClientManager
    {
        void RegisterUser(string firstName, string lastName, string email, string password, string place, DateTime dateOfBirth, List<IAccount> accounts);

        void RegisterOrg(string name, string PIB, string place, string email, DateTime dateOfBirth, List<IAccount> accounts);

        void ChangeUserAccount(IUser user, List<IAccount> accounts);

        void ChangeOrgAccount(IOrganization organization, List<IAccount> accounts);

        List<ITransaction> SearchHistory(IClient client, DateTime? date, int? transType, int? rating);

        IClient GetClientByID(Guid clientID);

        void AddFunds(IClient client, Guid accountID, double amount, ICurrency currency);
    }
}
