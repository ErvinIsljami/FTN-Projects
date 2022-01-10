using Common;
using Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebRole1.Controllers
{
    public class AliController : Controller
    {
        // GET: Ali
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Input(string type, string amount)
        {
            int amountNum = 0;
            if (!int.TryParse(amount, out amountNum))
            {
                return View("Error");
            }

            var queue = QueueHelper.GetQueueReference("red");

            queue.AddMessage(new Microsoft.WindowsAzure.Storage.Queue.CloudQueueMessage($"{type}~{amountNum}"));
            Trace.WriteLine($"Added message: {type}~{amount}");

            return RedirectToAction("Index");
        }

        public ActionResult Obavestenja()
        {
            List<string> poruke = new List<string>();

            var queue = QueueHelper.GetQueueReference("red2");
            var message = queue.GetMessage();
            if (message != null)
            {
                poruke.Add(message.AsString);
                queue.DeleteMessage(message);
            }
            do
            {
                message = queue.GetMessage();
                if (message != null)
                {
                    poruke.Add(message.AsString);
                    queue.DeleteMessage(message);
                }
            } while (message != null);

            ViewBag.list = poruke;

            return View("Obavestenja");
        }
    }
}