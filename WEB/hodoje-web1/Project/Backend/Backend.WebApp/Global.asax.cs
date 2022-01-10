using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using AutoMapper;
using Backend.App_Start;
using Backend.App_Start.MappingProfiles;

namespace Backend
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {             
            UnityConfig.RegisterComponents();
            GlobalConfiguration.Configure(WebApiConfig.Register);

            // Disable circular references for json formatter
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings
                .ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            // Remove xml formatter
            GlobalConfiguration.Configuration.Formatters
                .Remove(GlobalConfiguration.Configuration.Formatters.XmlFormatter);
        }
    }
}
