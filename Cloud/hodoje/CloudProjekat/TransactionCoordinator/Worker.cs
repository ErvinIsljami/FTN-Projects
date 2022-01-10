using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DistributedTransaction
{
    public class Worker : IWorker
    {
        private string _executingContainerId;
        private string _ipAddress;
        private string _myAssemblyName;
        private Assembly _roleEnvironment;
        private TransactionCoordinatorServer _transactionCoordinatorServer;

        public string ExecutingContainerId { get => _executingContainerId; set => _executingContainerId = value; }
        public string IpAddress { get => _ipAddress; set => _ipAddress = value; }
        public string MyAssemblyName { get => _myAssemblyName; set => _myAssemblyName = value; }
        public Assembly RoleEnvironment { get => _roleEnvironment; set => _roleEnvironment = value; }
        public TransactionCoordinatorServer TransactionCoordinatorServer { get => _transactionCoordinatorServer; set => _transactionCoordinatorServer = value; }

        public void Start(string containerId)
        {
            ExecutingContainerId = containerId;
            Console.WriteLine($"Test{ExecutingContainerId}");

            MyAssemblyName = GetAssemblyFullName($"{Assembly.GetExecutingAssembly().GetName().Name}.dll");
            RoleEnvironment = Assembly.LoadFile(GetAssemblyFullName("RoleEnvironment.dll"));

            IpAddress = ReturnAddress(MyAssemblyName, ExecutingContainerId);
            Console.WriteLine(IpAddress);

            string bankAddress = GetServiceAddress("Bank");
            string bookstoreAddress = GetServiceAddress("Bookstore");
            TransactionCoordinator tc = new TransactionCoordinator(bankAddress, bookstoreAddress, GetServiceAddress);
            TransactionCoordinatorServer = new TransactionCoordinatorServer(tc);
            TransactionCoordinatorServer.Start(IpAddress);

            string[] brotherPorts = ReturnBrotherInstancesAddresses(MyAssemblyName, IpAddress);
            foreach (var brotherPort in brotherPorts)
            {
                Console.WriteLine(brotherPort);
            }
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
                    if (RoleEnvironment != null)
                    {
                        Type workerClass = RoleEnvironment.ExportedTypes.ToList().FirstOrDefault(x => x.Name == "RoleEnvironment");
                        if (workerClass != null)
                        {
                            string typeName = workerClass.FullName;
                            if (!String.IsNullOrWhiteSpace(typeName))
                            {
                                object obj = RoleEnvironment.CreateInstance(typeName);
                                if (obj != null)
                                {
                                    System.Reflection.MethodInfo mi = obj.GetType().GetMethod("ReturnAddress");

                                    result = (string)(mi.Invoke(obj, new object[2] { $"{Path.GetFileName(myAssemblyName)}", containerId }));
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
                    if (RoleEnvironment != null)
                    {
                        Type workerClass = RoleEnvironment.ExportedTypes.ToList().FirstOrDefault(x => x.Name == "RoleEnvironment");
                        if (workerClass != null)
                        {
                            string typeName = workerClass.FullName;
                            if (!String.IsNullOrWhiteSpace(typeName))
                            {
                                object obj = RoleEnvironment.CreateInstance(typeName);
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

        private string GetServiceAddress(string serviceName)
        {
            Task<string> t = new Task<string>(() =>
            {
                string result = "";
                try
                {
                    if (RoleEnvironment != null)
                    {
                        Type workerClass = RoleEnvironment.ExportedTypes.ToList().FirstOrDefault(x => x.Name == "RoleEnvironment");
                        if (workerClass != null)
                        {
                            string typeName = workerClass.FullName;
                            if (!String.IsNullOrWhiteSpace(typeName))
                            {
                                object obj = RoleEnvironment.CreateInstance(typeName);
                                if (obj != null)
                                {
                                    System.Reflection.MethodInfo mi = obj.GetType().GetMethod("GetServiceAddress");

                                    result = (string)(mi.Invoke(obj, new object[1] { $"{serviceName}" }));
                                }
                            }
                        }
                        else
                        {
                            result = "";
                        }
                    }
                }
                catch (TargetInvocationException ex)
                {
                    Console.WriteLine(ex.Message);
                    result = "";
                }
                return result;
            });
            t.Start();
            t.Wait();
            return t.Result;
        }
    }
}
