using Microsoft.WindowsAzure.Storage.Queue;
using Storage;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebRole1.Controllers
{
    public class PrviController : Controller
    {
        // GET: Prvi
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Validacija(string vrstaHrane, string kolicina)
        {
            int temp;
            if (!Int32.TryParse(kolicina, out temp))
            {
                return View("Greska");
            } else
            {
                string pomocna = vrstaHrane + "_" + kolicina;
                QueueHelper queueHelper = new QueueHelper("red1");
                CloudQueueMessage message = new CloudQueueMessage(pomocna);
                    
                queueHelper.queue.AddMessage(message);
                return View("Uspjesno");
                
                
            }
            
        }

        public ActionResult IspisIzDrugogReda()
        {
            return View();
        }
    }
}