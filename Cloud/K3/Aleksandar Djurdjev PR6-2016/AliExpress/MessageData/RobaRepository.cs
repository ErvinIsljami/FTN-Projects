using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageData
{
    public class RobaRepository
    {
        private CloudStorageAccount storageAccount;
        private CloudTable table;
        public RobaRepository()
        {
            storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));
            CloudTableClient tableClient = new CloudTableClient
            (new Uri(storageAccount.TableEndpoint.AbsoluteUri),
            storageAccount.Credentials);
            table = tableClient.GetTableReference("RobaTable");
            table.CreateIfNotExists();
            DodajRobu(new Roba("Sto", 10));
            DodajRobu(new Roba("Stolica", 5));
            DodajRobu(new Roba("Tepih", 15));
        }
        public IQueryable<Roba> PreuzmiRobu()
        {
            var results = from g in table.CreateQuery<Roba>()
                          where g.PartitionKey == "Roba"
                          select g;
            return results;
        }
        public void DodajRobu(Roba novaRoba)
        {
            try
            {
                TableOperation insertOperation = TableOperation.Insert(novaRoba);
                table.Execute(insertOperation);
            }
            catch { }
        }
        public Roba PreuzmiKonkretnuRobu(String vrsta)
        {
            return PreuzmiRobu().Where(p => p.RowKey == vrsta).FirstOrDefault();
        }
        public void AzurirajRobu(Roba roba)
        {
            TableOperation updateOperation = TableOperation.Replace(roba);
            table.Execute(updateOperation);
        }
    }
}
