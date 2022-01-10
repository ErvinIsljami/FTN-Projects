using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using DataAccess;
using FileReader;
using FileReader.Loggers;
using FileReader.Readers;
using FileReader.Writers;

namespace UI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            string solutionPath = Path.GetDirectoryName(Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory));
            string pathToWatch = Path.Combine(solutionPath, "Readings");
            string logDirectory = Path.Combine(solutionPath, "Log");
            HttpContext.Current.Application["Watcher"] = new Watcher(
                                                         new XmlReader(), 
                                                         new TxtLogger(),
                                                         new DatabaseWriter(
                                                             new UnitOfWork(
                                                                 new DatabaseContext())),
                                                             pathToWatch, 
                                                             logDirectory);
            ((Watcher)HttpContext.Current.Application["Watcher"]).Watch();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
