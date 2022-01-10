using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Models;

namespace FileReader.Interfaces
{
    public interface IWriter
    {
        void Write(List<PowerConsumptionData> data, out string errorMessage);
    }
}
