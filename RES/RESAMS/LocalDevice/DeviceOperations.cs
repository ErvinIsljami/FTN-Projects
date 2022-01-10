using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts;
using System.ServiceModel;
using System.Collections.Concurrent;
using System.Configuration;

namespace LocalDevice
{
    public static class DeviceOperations
    {
        public static ConcurrentBag<ChangeSet> ChangeSets { get; set; }
        private static Task sendingDataTask;

        public static Device Device { get; set; }

        static DeviceOperations()
        {
            ChangeSets = new ConcurrentBag<ChangeSet>();
            sendingDataTask = new Task(SendDataPeriodicly);
            sendingDataTask.Start();
        }

        public static Device DeviceTypeMenu()
        {
            while (true)
            {

                Console.WriteLine("Izaberite tip: ");
                Console.WriteLine("1. Analog.");
                Console.WriteLine("2. Digital.");

                int i;

                if (Int32.TryParse(Console.ReadLine(), out i))
                {
                    if (i == 1)
                    {
                        return new Device(EDeviceType.Analog);
                    }
                    else if (i == 2)
                    {
                        return new Device(EDeviceType.Digital);
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("Greska...");
                    }
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Greska...");
                }
            }
        }

        public static IDeviceChangeSetService Proxy { get; set; }

        public static void ConnectToService()
        {
            string serviceName;
            Console.WriteLine("Unesite ime lokalnog kontrolera na koji zelite da se povezete, ili ams ako zelite direktno na sistem.");
            serviceName = Console.ReadLine();

            EndpointAddress address = new EndpointAddress("net.tcp://localhost:41000/"+serviceName);
            NetTcpBinding netTcpBinding = new NetTcpBinding();

            ChannelFactory<IDeviceChangeSetService> channelFactory = new ChannelFactory<IDeviceChangeSetService>(netTcpBinding, address);
            Proxy = channelFactory.CreateChannel();
        }

        public static int UserMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine(Device);
                Console.WriteLine("Izaberite opciju: ");
                Console.WriteLine("1. Promeni vrednost: ");
                Console.WriteLine("2. Izlaz. ");

                int i;
                if (Int32.TryParse(Console.ReadLine(), out i))
                {
                    if(i == 1)
                    {
                        UpdateDeviceValue();
                    }
                    else if(i == 2)
                    {
                        Console.WriteLine("Gasenje aplikacije...");
                        Console.ReadLine();
                        Environment.Exit(0);
                    }
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Greska... Molimo vas da unesete ponovo.");
                }
            }
        }

        public static bool UpdateDeviceValue()
        {
            while (true)
            {
                Console.WriteLine("Unesite novu vrednost: ");

                double newValue;
                if (Double.TryParse(Console.ReadLine(), out newValue))
                {
                    if(Device.UpdateValue(newValue))
                    {
                        ChangeSets.Add(new ChangeSet(Device.Value, Device.TimeStamp));
                        
                        return true;
                    }
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Greska... Molimo vas da unesete ponovo.");
                }
            }
        }

        private async static void SendDataPeriodicly()
        {
            while (true)
            {
                int interval;
                try
                {
                    interval = Int32.Parse(ConfigurationManager.AppSettings["sendingInterval"]);
                }
                catch(Exception e) 
                {
                    interval = 300;
                }

                await Task.Delay(interval * 1000);

                try
                {
                    Proxy.SendNewMeasurement(Device.Id, ChangeSets.ToList());
                    ChangeSets = new ConcurrentBag<ChangeSet>();
                }
                catch (Exception e)
                {
                    Console.Clear();
                    Console.WriteLine("Greska prilikom slanja podataka.");
                    Console.WriteLine("Service nije dostupan");
                    Console.ReadLine();
                    continue;
                }
            }
        }
    }
}
