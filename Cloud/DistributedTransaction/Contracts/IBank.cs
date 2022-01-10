﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    [ServiceContract]
    public interface IBank : ITransaction
    {
        [OperationContract]
        void List();

        [OperationContract]
        void EnlistMoneyTransfer(string userID, double amount);
    }
}
