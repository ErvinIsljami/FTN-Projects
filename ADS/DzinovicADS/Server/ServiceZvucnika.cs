using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class ServiceZvucnika : IServiceZvucnika
    {
        public void DodajZvucnik(Zvucnik z)
        {
            if(ServerDatabase.Zvucnici.ContainsKey(z.Id) == false)
            {
                ServerDatabase.Zvucnici.Add(z.Id, z);
                Console.WriteLine("Dodali smo novi zvucnik: " + z);
            }
            else
            {
                DbException e = new DbException("Zvucnik sa datim idijem vec postoji u bazi");
                throw new FaultException<DbException>(e);
            }
        }

        public void ObrisiZvucnik(int id)
        {
            throw new NotImplementedException();
        }

        public List<Zvucnik> VratiSveZvucnike()
        {
            throw new NotImplementedException();
        }
    }
}
