using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Command(string poruka)
        {
            if(poruka != "otvori" && poruka != "zatvori")
            {
                return View("Greska");
            }
            QueueHelper helper = new QueueHelper("queue");
            helper.EnqueueMessage(poruka);


            return View("Index");
        }
    }
}