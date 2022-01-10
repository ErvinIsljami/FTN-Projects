using Contracts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    class Bank : IBank
    {
        UserTableHelper tableHelper;
        string user;
        double cost;

        public Bank()
        {
            tableHelper = new UserTableHelper();
            User u = new User("Ervin", 30000);
            User u1 = new User("Boske", 20000);
            User u2 = new User("Cebo", 30);
            tableHelper.AddElement(u);
            tableHelper.AddElement(u1);
            tableHelper.AddElement(u2);

        }
        public void Commit()
        {
            tableHelper.Update(user, cost);
        }

        public void EnlistMoneyTransfer(string userID, double amount)
        {
            user = userID;
            cost = amount;
        }

        public void List()
        {
            List<User> lista = tableHelper.RetrieveAllElements().ToList();

            foreach(User u in lista)
            {
                Trace.TraceInformation("User: " + u.Name);
            }
        }

        public bool Prepare()
        {
            if (tableHelper.GetMoney(user) > cost)
                return true;
            else
            {
                Trace.TraceInformation("Nema para");
                return false;
            }
        }

        public void Rollback()
        {
            Trace.TraceInformation("Rollback..");
            //ne mora nista
        }
    }
}
