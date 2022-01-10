using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkService.Models
{
    // Each ViewModel will inherit this interface and implement (or not, depends) it's method
    public interface INotify
    {
        void Notify(Road changedRoad);
    }
}
