using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Dll
{
    public class Worker : IWorker
    {
        private string _executingContainerId;
        private string _ipAddress;
        private string _myAssemblyName;
        private string _currentDirectory;
        private Assembly _roleEnvironmentDll;
        private Assembly _serviceDll;
        private Dictionary<string, string> _brotherInstances;

        public string ExecutingContainerId { get => _executingContainerId; set => _executingContainerId = value; }
        public string IpAddress { get => _ipAddress; set => _ipAddress = value; }
        public string MyAssemblyName { get => _myAssemblyName; set => _myAssemblyName = value; }
        public Assembly RoleEnvironmentDll { get => _roleEnvironmentDll; set => _roleEnvironmentDll = value; }
        public string CurrentDirectory { get => _currentDirectory; set => _currentDirectory = value; }
        public Assembly ServiceDll { get => _serviceDll; set => _serviceDll = value; }
        public Dictionary<string, string> BrotherInstances { get => _brotherInstances; set => _brotherInstances = value; }

        public void Start(string containerId)
        {
            ExecutingContainerId = containerId;
            Console.WriteLine($"Test{ExecutingContainerId}");

            MyAssemblyName = GetAssemblyFullName("Dll.dll");
            CurrentDirectory = Path.GetDirectoryName(MyAssemblyName);

            RoleEnvironmentDll = Assembly.LoadFile(GetAssemblyFullName("RoleEnvironmentDll.dll"));
            ServiceDll = LoadService();

            IpAddress = ReturnAddress(MyAssemblyName, ExecutingContainerId);
            Console.WriteLine(IpAddress);

            string[] brotherPorts = ReturnBrotherInstancesAddresses(MyAssemblyName, IpAddress);
            foreach (var brotherPort in brotherPorts)
            {
                BrotherInstances.Add(brotherPort.Split(',')[0], brotherPort.Split(',')[1]);
                Console.WriteLine(brotherPort);
            }

            StartServiceServer();
        }

        public void Stop()
        {
            Console.WriteLine("Stop");
        }

        public string GetAssemblyFullName(string assemblyName)
        {
            string executingExe = Assembly.GetCallingAssembly().Location;
            string debugDir = Path.GetDirectoryName(executingExe);
            string consoleAppPath = Path.GetDirectoryName(debugDir);
            string myAssemblyDirectory = Path.GetFullPath(consoleAppPath + $@"\Folder{ExecutingContainerId}");
            string fullAssemblyName = Directory.GetFiles(myAssemblyDirectory).FirstOrDefault(x => x.Contains(assemblyName));
            return fullAssemblyName;
        }

        private string ReturnAddress(string myAssemblyName, string containerId)
        {
            Task<string> t = new Task<string>(() =>
            {
                string result = "";
                try
                {
                    if (RoleEnvironmentDll != null)
                    {
                        Type workerClass = RoleEnvironmentDll.ExportedTypes.ToList().FirstOrDefault(x => x.Name == "RoleEnvironment");
                        if (workerClass != null)
                        {
                            string typeName = workerClass.FullName;
                            if (!String.IsNullOrWhiteSpace(typeName))
                            {
                                object obj = RoleEnvironmentDll.CreateInstance(typeName);
                                if (obj != null)
                                {
                                    System.Reflection.MethodInfo mi = obj.GetType().GetMethod("ReturnAddress");

                                    result = (string)(mi.Invoke(obj, new object[2] { $"{myAssemblyName}", containerId }));
                                }
                            }
                        }
                        else
                        {
                            result = null;
                        }
                    }
                }
                catch (TargetInvocationException ex)
                {
                    Console.WriteLine(ex.Message);
                    result = null;
                }
                return result;
            });
            t.Start();
            t.Wait();
            return t.Result;
        }

        private string[] ReturnBrotherInstancesAddresses(string myAssemblyName, string myAddress)
        {
            Task<string[]> t = new Task<string[]>(() =>
            {
                string[] result = { };
                try
                {
                    if (RoleEnvironmentDll != null)
                    {
                        Type workerClass = RoleEnvironmentDll.ExportedTypes.ToList().FirstOrDefault(x => x.Name == "RoleEnvironment");
                        if (workerClass != null)
                        {
                            string typeName = workerClass.FullName;
                            if (!String.IsNullOrWhiteSpace(typeName))
                            {
                                object obj = RoleEnvironmentDll.CreateInstance(typeName);
                                if (obj != null)
                                {
                                    System.Reflection.MethodInfo mi = obj.GetType().GetMethod("ReturnBrotherInstancesAddresses");

                                    result = (string[])(mi.Invoke(obj, new object[2] { $"{myAssemblyName}", myAddress }));
                                }
                            }
                        }
                        else
                        {
                            result = null;
                        }
                    }
                }
                catch (TargetInvocationException ex)
                {
                    Console.WriteLine(ex.Message);
                    result = null;
                }
                return result;
            });
            t.Start();
            t.Wait();
            return t.Result;
        }

        private Assembly LoadService()
        {
            DirectoryInfo directory = new DirectoryInfo(CurrentDirectory);

            string filter = "*.dll";
            FileInfo[] fileNames = directory.GetFiles(filter).ToArray();
            string serviceName = "";
            foreach (var dll in fileNames)
            {
                if (dll.Name != Path.GetFileName(MyAssemblyName) && dll.Name != Path.GetFileName(RoleEnvironmentDll.Location))
                {
                    serviceName = dll.FullName;
                    break;
                }
            }
            return Assembly.LoadFile(serviceName);
        }

        private void StartServiceServer()
        {
            Task t = Task.Run(() =>
            {
                try
                {
                    if (ServiceDll != null)
                    {
                        if (ServiceDll.ExportedTypes.ToList().FirstOrDefault(x => x.Name == "BankServer") != null)
                        {
                            Type bankServerClass = ServiceDll.ExportedTypes.ToList().FirstOrDefault(x => x.Name == "BankServer");
                            if (bankServerClass != null)
                            {
                                string typeName = bankServerClass.FullName;
                                if (!String.IsNullOrWhiteSpace(typeName))
                                {
                                    object obj = ServiceDll.CreateInstance(typeName);
                                    if (obj != null)
                                    {
                                        System.Reflection.MethodInfo mi = obj.GetType().GetMethod("Start");

                                        mi.Invoke(obj, new object[1] { $"{IpAddress}" });
                                        Console.WriteLine("Bank server started.");
                                    }
                                }
                            }
                        }
                        else if (ServiceDll.ExportedTypes.ToList().FirstOrDefault(x => x.Name == "BookstoreServer") != null)
                        {
                            Type BookstoreServerClass = ServiceDll.ExportedTypes.ToList().FirstOrDefault(x => x.Name == "BookstoreServer");
                            if (BookstoreServerClass != null)
                            {
                                string typeName = BookstoreServerClass.FullName;
                                if (!String.IsNullOrWhiteSpace(typeName))
                                {
                                    object obj = ServiceDll.CreateInstance(typeName);
                                    if (obj != null)
                                    {
                                        System.Reflection.MethodInfo mi = obj.GetType().GetMethod("Start");

                                        mi.Invoke(obj, new object[1] { $"{IpAddress}" });
                                        Console.WriteLine("Bookstore server started.");
                                    }
                                }
                            }
                        }
                        else if (ServiceDll.ExportedTypes.ToList().FirstOrDefault(x => x.Name == "TransactionCoordinatorServer") != null)
                        {
                            Type TransactionCoordinatorServerClass = ServiceDll.ExportedTypes.ToList().FirstOrDefault(x => x.Name == "TransactionCoordinatorServer");
                            if (TransactionCoordinatorServerClass != null)
                            {
                                string typeName = TransactionCoordinatorServerClass.FullName;
                                if (!String.IsNullOrWhiteSpace(typeName))
                                {
                                    object obj = ServiceDll.CreateInstance(typeName);
                                    if (obj != null)
                                    {
                                        System.Reflection.MethodInfo mi = obj.GetType().GetMethod("Start");

                                        mi.Invoke(obj, new object[1] { $"{IpAddress}" });
                                        Console.WriteLine("Transaction coordinator server started.");
                                    }
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("Dll doesn't contain neither Bank, Bookstore or TransactionCoordinator classes.");
                        }
                    }
                }
                catch (TargetInvocationException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            });
        }
    }
}
