using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Xml;
using System.IO;
using Contracts;
using System.Diagnostics;
using System.Configuration;
using System.Threading;

namespace ComputeService
{
    class Program
    {

        private static int numberOfContainers;
        private static IContainer[] proxy;
        private static string path;
        
        

        static void Main(string[] args)
        {
            RoleEnvironmentService res = new RoleEnvironmentService();

            
            StartContainters();
            LoadPath();

            Task checkDll = new Task(() => CheckDLLPeriodicly());
            Task checkIfAlive = new Task(() => CheckIfAlive());
            checkDll.Start();
            checkIfAlive.Start();


            Console.ReadLine();


        }


        static void StartContainters()
        {
            for (int i = 1; i <= 4; i++)
            {
                Process.Start(@"C:\Users\ervin\Documents\Cloud\Projekat\Container\bin\Debug\Container.exe", (10000 + i * 10).ToString());
                RoleEnviroment.inUse[RoleEnviroment.ports[i-1]] = false;
            }
        }
        static void LoadPath()
        {
            path = ConfigurationSettings.AppSettings["putanja"];

        }
        static bool LoadPackage(string path)
        {
            using (XmlReader reader = XmlReader.Create(path))
            {
                reader.MoveToContent();
                while (reader.Read())
                {
                    try
                    {
                        numberOfContainers = Int32.Parse(reader.Value);
                        if (numberOfContainers > 4 || numberOfContainers < 0)
                        {
                            Console.WriteLine("Broj kontejnera nije ispravan. Broj kontejnera mora biti ceo broj od 0 do 4");
                            reader.Close();
                            File.Delete(path);
                            return false;

                        }
                        Console.WriteLine("Pokretanje " + numberOfContainers + " kontejnerskih aplikacija...");
                        reader.Close();
                        return true;
                    }
                    catch
                    {
                        Console.WriteLine("XML fajl nije pravilno konfigurisan");
                        break;
                    }
                }
                return false;
            }
        }
        static void CopyDLL(string path)
        {
            for (int i = 1; i <= numberOfContainers; i++)
            {
                if (File.Exists(string.Format(@"C:\Users\ervin\Documents\Cloud\Container{0}\Assembly.dll", i)))
                    File.Delete(string.Format(@"C:\Users\ervin\Documents\Cloud\Container{0}\Assembly.dll", i));
                
                File.Copy(path, string.Format(@"C:\Users\ervin\Documents\Cloud\Container{0}\Assembly.dll", i));
            }
        }
        static void LoadContainers()
        {
            proxy = new IContainer[4];
            for (int i = 1; i <= 4; i++)
            {
                NetTcpBinding binding = new NetTcpBinding();
                EndpointAddress adress = new EndpointAddress(string.Format(@"net.tcp://localhost:{0}/Container", i * 10 + 10000));
                ChannelFactory<IContainer> factory = new ChannelFactory<IContainer>(binding, adress);
                proxy[i - 1] = factory.CreateChannel();
                if(i-1 < numberOfContainers)
                    RoleEnviroment.inUse[RoleEnviroment.ports[i-1]] = true;
            }
            for (int i = 0; i < numberOfContainers; i++)
            {
                try
                {
                    RoleEnviroment.inUse[RoleEnviroment.ports[i]] = true;
                    Console.WriteLine(proxy[i].Load(string.Format(@"C:\Users\ervin\Documents\Cloud\Container{0}\Assembly.dll", i + 1)));
                }
                catch
                {
                    Console.WriteLine("Container {0} nije uspesno load-ovao", i + 1);

                }
            }
        }
        static async void CheckDLLPeriodicly()
        {
            while (true)
            {
                string[] files = Directory.GetFiles(path);
                if (files.Count() > 2)
                {
                    Console.WriteLine("Package error");
                    foreach (string file in files)
                        File.Delete(file);
                }
                else
                {
                    if (files[0].Contains(".xml"))
                    {
                        if (LoadPackage(files[0]))
                        {
                            CopyDLL(files[1]);
                            LoadContainers();
                        }
                    }
                    else if (files[1].Contains(".xml"))
                    {
                        if (LoadPackage(files[1]))
                        {
                            CopyDLL(files[0]);
                            LoadContainers();
                        }
                    }
                }
                await Task.Delay(1000 * 60 * 3);
            }
        }
        static async void CheckIfAlive()
        {
            while (true)
            {
                    await Task.Delay(10000);
                    for (int i = 0; i < 4; i++)
                    {

                        NetTcpBinding binding = new NetTcpBinding();
                        EndpointAddress adress = new EndpointAddress(string.Format(@"net.tcp://localhost:{0}/Container", RoleEnviroment.ports[i]));
                        ChannelFactory<IContainer> factory = new ChannelFactory<IContainer>(binding, adress);
                        proxy[i] = factory.CreateChannel();
                    }
                for (int i = 0; i < 4; i++)
                {
                    try
                    {
                        Console.WriteLine(proxy[i].CheckState());
                    }
                    catch
                    {
                        Console.WriteLine("Container {0} is not responding.", i + 1);
                        if (RoleEnviroment.inUse[RoleEnviroment.ports[i]])
                        {
                            RoleEnviroment.inUse[RoleEnviroment.ports[i]] = false;
                            if (numberOfContainers != 4)
                            {
                                for (int j = 0; j < 4; j++)
                                {
                                    if (!RoleEnviroment.inUse[RoleEnviroment.ports[j]] && j != i)
                                    {
                                        RoleEnviroment.inUse[RoleEnviroment.ports[j]] = true;
                                        proxy[j].Load(string.Format(@"C:\Users\ervin\Documents\Cloud\Container{0}\Assembly.dll", j + 1));
                                        break;
                                    }
                                }
                                Process.Start(@"C:\Users\ervin\Documents\Cloud\Projekat\Container\bin\Debug\Container.exe", RoleEnviroment.ports[i].ToString());
                            }
                            else    //broj zauzetih kontejnera je max i moramo njega opet dici i pokrenuti ga
                            {
                                Process.Start(@"C:\Users\ervin\Documents\Cloud\Projekat\Container\bin\Debug\Container.exe", RoleEnviroment.ports[i].ToString());

                                NetTcpBinding binding = new NetTcpBinding();
                                EndpointAddress adress = new EndpointAddress(string.Format(@"net.tcp://localhost:{0}/Container", RoleEnviroment.ports[i]));
                                ChannelFactory<IContainer> factory = new ChannelFactory<IContainer>(binding, adress);
                                proxy[i] = factory.CreateChannel();


                                proxy[i].Load(string.Format(@"C:\Users\ervin\Documents\Cloud\Container{0}\Assembly.dll", i + 1));
                            }
                        }
                        else    //nije u upotrebi container koji je pao, samo ga opet dignemo
                        {
                            Process.Start(@"C:\Users\ervin\Documents\Cloud\Projekat\Container\bin\Debug\Container.exe", RoleEnviroment.ports[i].ToString());
                        }
                    }
                }  
            }
        }
    }
}