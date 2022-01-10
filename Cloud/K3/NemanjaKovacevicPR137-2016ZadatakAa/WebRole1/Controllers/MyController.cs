using HelpersAndTableStorage;
using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebRole1.Controllers
{
    public class MyController : Controller
    {
        // GET: My
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Posalji(string Vrsta, string Kolicina)
        {
            if (!Validacija(Kolicina))
            {
                return RedirectToAction("Greska");
            }
            else
            {
                var queue = QueueHelper.InitQueue("DataConnectionString6", "hranaqueue");
                queue.AddMessage(new Microsoft.WindowsAzure.Storage.Queue.CloudQueueMessage(Vrsta + " " + Kolicina));
                return RedirectToAction("Index");
            }
        }


        public ActionResult Greska()
        {
            return View();
        }

        public ActionResult Obavestenja()
        {
            var queue = QueueHelper.InitQueue("DataConnectionString6", "hranaqueuenazad");

            List<string> poruke = new List<string>();

            CloudQueueMessage msg = null;

            while ((msg = queue.GetMessage()) != null)
            {
                poruke.Add(msg.AsString);
                queue.DeleteMessage(msg);
            }

            ViewBag.Poruke = poruke;

            return View();
        }

        private bool Validacija(string text)
        {
            int br = 0;
            return Int32.TryParse(text, out br);
        }
    }
}