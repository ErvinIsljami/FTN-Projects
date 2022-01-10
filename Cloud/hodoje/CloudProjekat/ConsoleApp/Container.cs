using System;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading.Tasks;
using Contract;
using System.Linq;
using System.ServiceModel;
using System.Threading;

namespace ConsoleApp
{
    // With ConcurrencyMode.Multiple, threads can call an operation at any time.  
    // It is your responsibility to guard your state with locks. If
    // you always guarantee you leave state consistent when you leave
    // the lock, you can assume it is valid when you enter the lock.
    //[CallbackBehavior(UseSynchronizationContext = false)]
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class Container : IContainer
    {
        private string _containerDirectoryPath;
        private int _id;
        private int _port;

        public string ContainerDirectoryPath
        {
            get { return _containerDirectoryPath; }
            set { _containerDirectoryPath = value; }
        }

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public int Port
        {
            get { return _port; }
            set { _port = value; }
        }

        public Container(string containerDirectoryPath, int id, int port)
        {
            _containerDirectoryPath = containerDirectoryPath;
            _id = id;
            _port = port;
        }

        public Container() { }

        public string Load(string assemblyName)
        {
            Task<string> t = new Task<string>(() =>
            {
                string result = "";
                try
                {
                    Assembly dll = Assembly.LoadFile(assemblyName);
                    if (dll != null)
                    {
                        Type workerClass = dll.ExportedTypes.ToList().Find(x => x.Name == "Worker");
                        Type iWorkerInterface = dll.ExportedTypes.ToList().Find(x => x.Name == "IWorker");
                        if(workerClass.GetInterfaces().Contains(iWorkerInterface))
                        {
                            string typeName = dll.ExportedTypes.ToList().Find(x => x.Name == "Worker").FullName;
                            object obj = dll.CreateInstance(typeName);
                            if (obj != null)
                            {
                                System.Reflection.MethodInfo mi = obj.GetType().GetMethod("Start");

                                mi.Invoke(obj, new object[1] { $"{Id}" });
                                result = "Dll executed successfully.";
                            }
                        }
                        else
                        {
                            result = "Dll has no IWorker interface and a class that implements it.";
                        }
                    }
                    dll = null;
                }
                catch (TargetInvocationException ex)
                {
                    Console.WriteLine(ex.Message);
                    //result = $"Dll not executed properly on container: Container{_id}";
                    result = null;
                }
                return result;
            });
            t.Start();
            t.Wait();
            return t.Result;
        }

        public string CheckState()
        {
            return $"Container[{Id}] state is OK.";
        }
    }
}
