using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class Device
    {
        public Guid Id { get; set; }
        public double Value { get; set; }
        public long TimeStamp { get; set; }
        public EDeviceType DeviceType { get; set; }

        public Device(double value, EDeviceType type)
        {
            Value = value;
            DateTimeConverter converter = new DateTimeConverter();
            TimeStamp = converter.ConvertToUnix(DateTime.Now);
            Id = Guid.NewGuid();
            DeviceType = type;
        }

        public Device(EDeviceType type)
        {
            Value = 0;
            DateTimeConverter converter = new DateTimeConverter();
            TimeStamp = converter.ConvertToUnix(DateTime.Now);
            Id = Guid.NewGuid();
            DeviceType = type;
        }

        public bool UpdateValue(double newValue)
        {
            if(DeviceType == EDeviceType.Digital)
            {
                if(newValue != 0 && newValue != 1)
                {
                    return false;
                }
            }
            Value = newValue;
            return true;
        }

        public override string ToString()
        {
            string ret = $"{DeviceType} device with id{Id} = {Value}";
            return ret;
        }
    }
}
