using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class Replikator : IReplikator
    {
        public void UpisiSve(List<Telefon> lista)
        {
            //ServerDatabase.Telefoni.Clear();
            foreach(Telefon t in lista)
            {
                ServerDatabase.Telefoni[t.Id] = t;
            }
        }

        public List<Telefon> UzmiSve()
        {
            return ServerDatabase.Telefoni.Values.ToList();
        }
    }
}
