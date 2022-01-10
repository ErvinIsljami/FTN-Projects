using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudCompute
{
    public class ContainerData
    {
        private int _id;
        private int _port;        
        private string _containerRootDirectory;
        private string _currentlyExecutingAssemblyName;
        private string _lastExecutedAssemblyName;
        private bool _isOnline;

        public ContainerData(int id, int port, string containerRootDirectory, string currentlyExecutingAssemblyName, string lastExecutedAssemblyName)
        {
            _id = id;
            _port = port;
            _containerRootDirectory = containerRootDirectory;
            _currentlyExecutingAssemblyName = currentlyExecutingAssemblyName;
            _lastExecutedAssemblyName = lastExecutedAssemblyName;
            _isOnline = true;
        }

        public int Id { get => _id; set => _id = value; }
        public int Port { get => _port; set => _port = value; }
        public string ContainerRootDirectory { get => _containerRootDirectory; set => _containerRootDirectory = value; }
        public string LastExecutingAssemblyName { get => _lastExecutedAssemblyName; set => _lastExecutedAssemblyName = value; }
        public string CurrentlyExecutingAssemblyName { get => _currentlyExecutingAssemblyName; set => _currentlyExecutingAssemblyName = value; }
        public bool IsOnline { get => _isOnline; set => _isOnline = value; }
    }
}
