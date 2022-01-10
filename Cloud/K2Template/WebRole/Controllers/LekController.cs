using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebRole.Models;

namespace WebRole.Controllers
{
    public class LekController : Controller
    {
        // GET: Lek
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult InputNew()
        {
            return View();
        }

        public ActionResult AddNew(Lekic l)
        {
            Lek lek = new Lek(l.Naziv, l.Cena, l.SifraLeka);
            TableHelper helper = new TableHelper();
            helper.AddOrUpdate(lek);

            return RedirectToAction("ListAll");
        }
        public ActionResult ListAll()
        {
            TableHelper tableHelper = new TableHelper();

            tableHelper.AddOrUpdate(new Lek("Brufen", 190, "381"));
            tableHelper.AddOrUpdate(new Lek("Kafetin", 250, "450"));
            tableHelper.AddOrUpdate(new Lek("Paracetamol", 410, "541"));

            var lista = tableHelper.RetrieveAll();

            return View(lista);
        }
    }
}