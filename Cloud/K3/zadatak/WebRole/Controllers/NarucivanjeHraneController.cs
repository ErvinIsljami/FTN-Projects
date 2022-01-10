using Common;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebRole.Controllers
{
    public class NarucivanjeHraneController : Controller
    {
        private CloudStorageAccount csa;
        private CloudTable ct;
        // GET: NarucivanjeHrane
        public ActionResult NarucivanjeHrane()
        {
            return View();
        }
        public ActionResult Dodavanjeporudzbine()
        {
            return View();
        }
        public NarucivanjeHraneController()
        {
            csa =CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));
            CloudTableClient tableClient = new CloudTableClient
            (new Uri(csa.TableEndpoint.AbsoluteUri),
            csa.Credentials);
            ct = tableClient.GetTableReference("Porudzbine");
            ct.CreateIfNotExists();
        }
        public void DodajPorudzbinu(Porudzbina nova)
        {
            TableOperation insertOperation = TableOperation.Insert(nova);
            ct.Execute(insertOperation);
        }
        public IQueryable<Porudzbina> SvePorudzbine()
        {
            var results = from g in ct.CreateQuery<Porudzbina>()
                          where g.PartitionKey == "Porudzbina"
                          select g;
            return results;
        }
        public Porudzbina Ucitajsveporudzbine(string index)
        {
            return SvePorudzbine().Where(p => p.RowKey == index).FirstOrDefault();
        }
    }
}