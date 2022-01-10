using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Contracts;
using Microsoft.WindowsAzure.Storage.Queue;

namespace WebRole1.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Items()
        {
            return View();
        }

        public ActionResult Ispitaj(string id)
        {
            CloudQueue queue = QueueHelper.GetQueueReference("vezba");

            if (id == "123")
            {
                queue.AddMessage(new CloudQueueMessage(id));
                Trace.TraceInformation("Dodata poruka u queue: " + id);
            }
            else if (id == "456")
            {
                queue.AddMessage(new CloudQueueMessage(id));
                Trace.TraceInformation("Dodata poruka u queue: " + id);
            }
            else if (id == "789")
            {
                queue.AddMessage(new CloudQueueMessage(id));
                Trace.TraceInformation("Dodata poruka u queue: " + id);
            }
            else
                return View("Greska");


            return View("Index");
        }
    }
}