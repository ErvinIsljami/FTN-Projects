using MessageData;
using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebRole.Controllers
{
    public class MainController : Controller
    {
        // GET: Main
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Poruci(String vrsta, String kolicina)
        {
            try
            {
                int kol = int.Parse(kolicina);
                CloudQueue queue = QueueHelper.GetQueueReference("queue1");
                queue.AddMessage(new CloudQueueMessage(String.Format(vrsta + " " + kolicina)));
                return View("Index");
            } catch
            {
                return View("Greska");
            }
        }

        public ActionResult Obavestenja()
        {
            CloudQueue queue = QueueHelper.GetQueueReference("queue2");
            CloudQueueMessage retrievedMessage;
            List<String> lista = new List<String>();

            while ((retrievedMessage = queue.GetMessage()) != null)
            {
                lista.Add(retrievedMessage.AsString);
                queue.DeleteMessage(retrievedMessage);
            }

            ViewBag.lista = lista;
            return View();
        }
    }
}