using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShoppy.Finantial_Module.Interfaces
{
    public interface IFinanceManager
    {
        void CreateAccount(string accountNumber, IBank bank, double balance, double creditPayment, bool creditAvailable);

        void CreateBank(string name, string address, List<ICredit> credits, string accountPreffix, List<IAccount> accounts, double minCreditAmount, double maxCreditAmount, int maxCreditYear);

        void CreateCredit(string name, double interest, ICurrency currency, IBank bank, int minYear);

        IAccount GetAccountByID(Guid accountID);

        void AskCredit(Guid clientID, Guid creditID, int years, double amount);

        void AccountPayment(Guid accountID, double amount);

        void CreditPayment(Guid accountID, double amount);

        double Convert(ICurrency currency, double amount);

        double CheckBalance(Guid accountID);
    }
}
