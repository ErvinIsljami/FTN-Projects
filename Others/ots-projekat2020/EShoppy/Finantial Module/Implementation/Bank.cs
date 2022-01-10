using EShoppy.Finantial_Module.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShoppy.Finantial_Module.Implementation
{
    public class Bank : IBank
    {
        private Guid iD;
        private string name;
        private string address;
        private List<ICredit> listOfCredits;
        private string accountPreffix;
        private List<IAccount> listOfAccounts;
        private double minimumCreditAmount;
        private double maximumCreditAmount;
        private int maximumCreditYear;

        public Bank(string name, string address, List<ICredit> listOfCredits, string accountPreffix, List<IAccount> listOfAccounts, double minimumCreditAmount, double maximumCreditAmount, int maximumCreditYear)
        {
            this.name = name;
            this.address = address;
            this.listOfCredits = listOfCredits;
            this.accountPreffix = accountPreffix;
            this.listOfAccounts = listOfAccounts;
            this.minimumCreditAmount = minimumCreditAmount;
            this.maximumCreditAmount = maximumCreditAmount;
            this.maximumCreditYear = maximumCreditYear;
            ID = Guid.NewGuid();
        }

        public Bank()
        {
            ID = Guid.NewGuid();
        }

        public Guid ID { get => iD; set => iD = value; }
        public string Name { get => name; set => name = value; }
        public string Address { get => address; set => address = value; }
        public List<ICredit> ListOfCredits { get => listOfCredits; set => listOfCredits = value; }
        public string AccountPreffix { get => accountPreffix; set => accountPreffix = value; }
        public List<IAccount> ListOfAccounts { get => listOfAccounts; set => listOfAccounts = value; }
        public double MinimumCreditAmount { get => minimumCreditAmount; set => minimumCreditAmount = value; }
        public double MaximumCreditAmount { get => maximumCreditAmount; set => maximumCreditAmount = value; }
        public int MaximumCreditYear { get => maximumCreditYear; set => maximumCreditYear = value; }


    }
}
