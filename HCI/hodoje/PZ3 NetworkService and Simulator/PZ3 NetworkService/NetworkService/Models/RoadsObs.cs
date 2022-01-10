using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace NetworkService.Models
{
    public class RoadsObs : BindableBase
    {
        private RoadsObs()
        {
            Roads = new ObservableCollection<Road>();
        }

        private static RoadsObs RO { get; set; }            // kao flag da je instanca napravljena

        public static RoadsObs Instance                     // RO koristimo implicitno kroz Instance
        {
            get                                             // zabranjuje setovanje
            {
                if (RO == null)
                {
                    RO = new RoadsObs();
                }
                return RO;
            }
        }

        public ObservableCollection<Road> Roads { get; set; }

        public ObservableCollection<Road> DeletedRoads = new ObservableCollection<Road>();
    }
}
