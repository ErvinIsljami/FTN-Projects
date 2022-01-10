using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebRole.Controllers
{
    public class TelevizorController : Controller
    {
        // GET: Televizor
        public ActionResult Index()
        {
            TableHelper helper = new TableHelper();
            var lista = helper.RetrieveAll();



            return View(lista);
        }
        public ActionResult Create()
        {
            return View();

        }
        public ActionResult Delete(Televizor t)
        {
            TableHelper helper = new TableHelper();
            helper.DeleteData(t);
            return RedirectToAction("Index");
        }

        public ActionResult Detail(Televizor t)
        {
            return View(t);
        }

        public ActionResult Edit(Televizor t)
        {
            return View(t);
        }

        public ActionResult AddNew(Televizor t)
        {
            TableHelper helper = new TableHelper();
            helper.AddOrUpdate(t);
            return RedirectToAction("Index");
        }
    }
}