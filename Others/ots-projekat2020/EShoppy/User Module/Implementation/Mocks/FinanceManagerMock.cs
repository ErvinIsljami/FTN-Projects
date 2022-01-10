using EShoppy.Finantial_Module.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShoppy.User_Module.Implementation.Mocks
{
    public class FinanceManagerMock : IFinanceManager
    {
        public void AccountPayment(Guid accountID, double amount)
        {
            throw new NotImplementedException();
        }


        public double CheckBalance(Guid accountID)
        {
            throw new NotImplementedException();
        }

        //Mock koji nam ne radi nista nego samo vraca sumu, prava implementacija ce se posle testirati
        public double Convert(ICurrency currency, double amount)
        {
            return amount;
        }

        public void CreateAccount(string accountNumber, IBank bank, double balance, double creditPayment, bool creditAvailable)
        {
            throw new NotImplementedException();
        }

        public void CreateBank(string name, string address, List<ICredit> credits, string accountPreffix, List<IAccount> accounts, double minCreditAmount, double maxCreditAmount, int maxCreditYear)
        {
            throw new NotImplementedException();
        }

        public void CreateCredit(string name, double interest, ICurrency currency, IBank bank, int minYear)
        {
            throw new NotImplementedException();
        }

        public void CreditPayment(Guid accountID, double amount)
        {
            throw new NotImplementedException();
        }

        public IAccount GetAccountByID(Guid accountID)
        {
            throw new NotImplementedException();
        }

        public void AskCredit(Guid clientID, Guid creditID, int years, double amount)
        {
            throw new NotImplementedException();
        }
    }
}
