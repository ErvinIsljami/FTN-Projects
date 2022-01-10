using EShoppy.Finantial_Module.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShoppy.Finantial_Module.Implementation
{
    public class Currency : ICurrency
    {
        private Guid iD;
        private string name;
        private string code;
        private double value;

        public Guid ID { get => iD; set => iD = value; }
        public string Name { get => name; set => name = value; }
        public string Code { get => code; set => code = value; }
        public double Value { get => value; set => this.value = value; }

        public Currency()
        {
            ID = Guid.NewGuid();
        }
    }
}
