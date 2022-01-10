using System;

namespace BankServiceApp.Arbitration
{
    public class ArbitrationException : Exception
    {
        public ArbitrationException(string message) : base(message)
        {
        }

        public ArbitrationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}