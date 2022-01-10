﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore
{
    [ServiceContract]
    public interface ITransaction
    {
        [OperationContract]
        bool Prepare();
        [OperationContract]
        void Commit();
        [OperationContract]
        void Rollback();
    }
}
