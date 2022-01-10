using Common;
using Microsoft.WindowsAzure.ServiceRuntime;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace JobWorker
{
    class JobServer
    {
        ServiceHost serviceHost;
        BlobHelper blobHelper = new BlobHelper();
        //public bool otvoren = false;

        public JobServer()
        {
            string adresa = RoleEnvironment.CurrentRoleInstance.InstanceEndpoints["Internal"].IPEndpoint.ToString();
            string endpoint = $"net.tcp://{adresa}/Internal";

            NetTcpBinding binding = new NetTcpBinding();

            serviceHost = new ServiceHost(typeof(JobServerProvider));
            serviceHost.AddServiceEndpoint(typeof(Int), binding, endpoint);
        }

        public void Open()
        {
            try
            {
                serviceHost.Open();
                //otvoren = true;
                
                Trace.WriteLine("Konekcija otvorena.");
            }
            catch (Exception e)
            {
                Trace.WriteLine($"Greska prilikom otvaranja konekcije. Greska: {e.Message}");
            }
        }
        public void Close()
        {
            try
            {
                serviceHost.Close();
                //otvoren = false;
                blobHelper.UploadToBlob("Zatvoren");
                Trace.WriteLine("Konekcija zatvorena.");
            }
            catch (Exception e)
            {
                Trace.WriteLine($"Greska prilikom zatvaranja konekcije. Greska: {e.Message}");
            }
        }
    }
}
