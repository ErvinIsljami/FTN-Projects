using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebRole.Models;
using Common;

namespace WebRole.Controllers
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

        public ActionResult ExecuteCommand(CommandModel cm)
        {
            if(cm.Command == "otvori" || cm.Command == "zatvori")
            {
                QueueHelper queueHelper = new QueueHelper("queue");
                queueHelper.EnqueueMessage(cm.Command);
            }
            else //vrati gresku
            {
                return RedirectToAction("Error");
            }
            return RedirectToAction("Index");
        }

        public ActionResult Error()
        {
            return View();
        }

        public ActionResult InputCommand()
        {
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}