using Contracts;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Web;
using System.Web.Mvc;

namespace WebSimulator.Controllers
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
        
        public ActionResult AddNewCommand(string id)
        {
            if(id == "otvori" || id == "zatvori")
            {
                CloudQueue queue = QueueHelper.GetQueueReference("vezba");
                queue.AddMessage(new CloudQueueMessage(id));
                return View("Index");
            }
            return View("Greska");
        }

        public ActionResult Command()
        {
            return View();
        }

        public ActionResult Brother()
        {
            //webrola je klijent za instancu 1
            ICallBrother proxy;
            string port = RoleEnvironment.Roles["JobWorker"].Instances[1].InstanceEndpoints["InternalEndpoint2"].IPEndpoint.ToString();
            var binding = new NetTcpBinding();
            ChannelFactory<ICallBrother> factory = new ChannelFactory<ICallBrother>(binding, new EndpointAddress("net.tcp://" + port + "/InternalEndpoint2"));
            proxy = factory.CreateChannel();

            string ret = proxy.IsBrotherAlive();
            Trace.TraceInformation("Pozvan brat." + ret);
            TableHelper th = new TableHelper();
            if (ret == "uspeh")
            {
                th.AddElement(new Information("Servis je otvoren", "..."));
            }
            else
            {
                th.AddElement(new Information("neuspesno", ret));
            }
            
            
            return View("Index");
        }
    }
}