using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class Porudzbina:TableEntity
    {
        private string vrsta;
        private string kolicina;
        public Porudzbina()
        {

        }
        public Porudzbina(string broj)
        {
            PartitionKey = "Porudzbina";
            RowKey = broj;
        }

        public string Vrsta
        {
            get
            {
                return vrsta;
            }

            set
            {
                vrsta = value;
            }
        }

        public string Kolicina
        {
            get
            {
                return kolicina;
            }

            set
            {
                kolicina = value;
            }
        }
    }
}
