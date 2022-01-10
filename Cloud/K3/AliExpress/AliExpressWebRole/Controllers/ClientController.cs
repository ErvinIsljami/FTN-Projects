using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage.Queue;
using AlIExpress_Data;

namespace AliExpressWebRole.Controllers
{
    public class ClientController : Controller
    {
        // GET: Client
        public ActionResult Index()
        {
            return View();
        }
    
        public ActionResult Poruci(string numberInput)
        {
            int result = 0;
            if(!Int32.TryParse(numberInput, out result))
            {
                ViewBag.ErrorMessage = "Uneti broj nije ceo broj!";
                return View("Error");
            }
            ViewBag.Message = "Uneli ste robu uspesno.";
            CloudQueue queue = QueueHelper.GetQueueReference("poruci");
            queue.AddMessage(new CloudQueueMessage(numberInput));
            return View("Index");
        }

        public ActionResult GoToTable()
        {
            TableHelper table = new TableHelper();
            List<Sluzba> tableContent = table.RetrieveAll().ToList();

            return View("Table", tableContent.OrderBy(entity => Int32.Parse(entity.RowKey)));
        }


    
    }
}