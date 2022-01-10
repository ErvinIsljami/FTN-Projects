using Common;
using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebRole.Controllers
{
    public class ZadatakController : Controller
    {
        // GET: Zadatak
        QueueHelper queueHelper = new QueueHelper();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ProveraVrednosti(string Tekst, string Kolicina)
        {
            try
            {
                int pomKol = Int32.Parse(Kolicina);

                if(pomKol > 0)
                {
                    CloudQueue queue = queueHelper.GetQueueReference("redporuka");
                    Roba roba = new Roba(Tekst) { Vrsta = Tekst, Kolicina = pomKol };
                    queue.AddMessage(new CloudQueueMessage(roba.ToString()));

                    return View("Index");
                }
                else
                {
                    return View();
                }
            }
            catch
            {
                return View();
            }
            
        }

        public ActionResult Obavesti()
        {
            CloudQueue queue = queueHelper.GetQueueReference("redpovratak");
            List<CloudQueueMessage> lista = new List<CloudQueueMessage>();

            while (queue.PeekMessage() != null)
            {
                CloudQueueMessage message = queue.GetMessage();
                lista.Add(message);
            }

            return View(lista);
        }
    }
}