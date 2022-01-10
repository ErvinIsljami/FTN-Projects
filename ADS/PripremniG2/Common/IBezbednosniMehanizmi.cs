﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [ServiceContract]
    public interface IBezbednosniMehanizmi
    {
        [OperationContract]
        [FaultContract(typeof(SecException))]
        string Autentifikacija(string korisnik, string lozinka);
    }
}
