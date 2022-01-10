using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Models;

namespace FileReader.Interfaces
{
    public interface IReader
    {
        List<PowerConsumptionData> Read(string fileName, out string errorMessage);
    }
}
