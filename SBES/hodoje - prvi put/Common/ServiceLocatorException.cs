using System;
using System.Runtime.Serialization;

namespace Common
{
    [DataContract]
    [Serializable]
    public class ServiceLocatorException : Exception
    {
        public ServiceLocatorException()
        {
        }

        public ServiceLocatorException(string message) : base(message)
        {
        }

        public ServiceLocatorException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}