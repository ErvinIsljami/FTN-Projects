using EShoppy.Finantial_Module.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShoppy.Finantial_Module.Implementation
{
    public class Account : IAccount
    {
        private Guid iD;
        private string accountNumber;
        private IBank bank;
        private double balance;
        private double creditPayment;
        private bool creditAvailable;

        public Account(string accountNumber, IBank bank, double balance, double creditPayment, bool creditAvailable)
        {
            this.accountNumber = accountNumber;
            this.bank = bank;
            this.balance = balance;
            this.creditPayment = creditPayment;
            this.creditAvailable = creditAvailable;
            this.ID = Guid.NewGuid();
        }

        public Guid ID { get => iD; set => iD = value; }
        public string AccountNumber { get => accountNumber; set => accountNumber = value; }
        public IBank Bank { get => bank; set => bank = value; }
        public double Balance { get => balance; set => balance = value; }
        public double CreditPayment { get => creditPayment; set => creditPayment = value; }
        public bool CreditAvailable { get => creditAvailable; set => creditAvailable = value; }


    }
}
