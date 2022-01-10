using Data;
using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebRole.Controllers
{
    public class ClientController : Controller
    {
        // GET: Client
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Porucivanje(string hrana, string kolicina)
        {
            int kol = 0;
            if (!Int32.TryParse(kolicina, out kol))
            {
                return View("Error");
            }

            string poruka = hrana + "#" + kolicina;
            QueueHelper queueHelper = new QueueHelper();
            CloudQueue queue = queueHelper.GetQueueReference("zahtevi");
            queue.AddMessage(new CloudQueueMessage(poruka));

            return View("Index");
        }

        public ActionResult Obavestenja()
        {
            QueueHelper queueHelper = new QueueHelper();
            CloudQueue red = queueHelper.GetQueueReference("drugired");
            List<CloudQueueMessage> lista = new List<CloudQueueMessage>();
            while(red.PeekMessage()!=null)
            {
                CloudQueueMessage msg = red.GetMessage();
                lista.Add(msg);
                red.DeleteMessage(msg);
            }
            
            return View(lista);
        }
    }
}