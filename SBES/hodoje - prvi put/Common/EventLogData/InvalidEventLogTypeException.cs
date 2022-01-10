using System;

namespace Common.EventLogData
{
    [Serializable]
    public class InvalidEventLogTypeException : Exception
    {
        public InvalidEventLogTypeException() : base("Invalid EventLogType specified.")
        {
        }

        public InvalidEventLogTypeException(string message) : base(message)
        {
        }
    }
}