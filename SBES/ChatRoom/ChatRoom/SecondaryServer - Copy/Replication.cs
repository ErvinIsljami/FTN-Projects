using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecondaryServer
{
    public class Replication : IReplication
    {
        public void PosaljiBazu(List<string> baza)
        {
            foreach (var item in baza)
            {
                Baza.podaci.Add(item);
                Console.WriteLine(item);
            }
        }

        public List<string> PreuzmiBazu()
        {
            List<string> list = new List<string>();

            foreach (var item in Baza.podaci)
            {
                list.Add(item);
            }

            return list;
        }
    }
}
