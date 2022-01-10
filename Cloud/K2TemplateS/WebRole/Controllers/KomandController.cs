using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebRole.Models;

namespace WebRole.Controllers
{
    public class KomandController : Controller
    {
        // GET: Komand
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddKomand(KomandModel model)
        {
            if(model.Komanda == "otvori" || model.Komanda == "zatvori")
            {
                QueueHelper helper = new QueueHelper("queue");
                helper.EnqueueMessage(model.Komanda);
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Greska");
            }

        }

        public ActionResult Greska()
        {
            return View();
        }
    }
}