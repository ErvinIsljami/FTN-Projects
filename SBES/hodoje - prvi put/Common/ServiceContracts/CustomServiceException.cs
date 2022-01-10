using System;

namespace Common.ServiceContracts
{
    public class CustomServiceException : Exception
    {
        public CustomServiceException() : base("An unknown exception occurred")
        {
        }

        public CustomServiceException(string Message) : base(Message)
        {
        }
    }
}