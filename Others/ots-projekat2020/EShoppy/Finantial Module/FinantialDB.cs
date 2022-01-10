using EShoppy.Finantial_Module.Implementation;
using EShoppy.Finantial_Module.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace EShoppy.Finantial_Module
{
    public static class FinantialDB
    {
        public static Dictionary<Guid, IAccount> Accounts { get; private set; }
        public static Dictionary<Guid, IBank> Banks { get; private set; }
        public static Dictionary<Guid, ICredit> Credits { get; private set; }
        public static Dictionary<string, ICurrency> Currency { get; private set; }

        static FinantialDB()
        {
            FinantialDB.Accounts = new Dictionary<Guid, IAccount>();
            FinantialDB.Banks = new Dictionary<Guid, IBank>();
            FinantialDB.Credits = new Dictionary<Guid, ICredit>();
            FinantialDB.Currency = new Dictionary<string, ICurrency>();
            ICurrency c1 = new Currency() { Code = "EUR", Name = "Euro", Value = 117.8 };
            ICurrency c2 = new Currency() { Code = "USD", Name = "Americki dolar", Value = 99.3 };
            ICurrency c3 = new Currency() { Code = "AUD", Name = "Australijski dolar", Value = 71.22 };
            ICurrency c4 = new Currency() { Code = "KWD", Name = "Kuvajtski dinar", Value = 324.83 };

            FinantialDB.Currency.Add(c1.Code, c1);
            FinantialDB.Currency.Add(c2.Code, c2);
            FinantialDB.Currency.Add(c3.Code, c3);
            FinantialDB.Currency.Add(c4.Code, c4);
        }
    }
}
