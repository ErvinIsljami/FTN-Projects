using EShoppy.Logistic_Module.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShoppy.Sale_Module.Implementation
{
    public class Transport : ITransport
    {
        private Guid iD;
        private string description;
        private double coef;

        public Transport(string description, double coef)
        {
            this.description = description;
            this.coef = coef;
            this.iD = Guid.NewGuid();
        }

        public Guid ID { get => iD; set => iD = value; }
        public string Description { get => description; set => description = value; }
        public double Coef { get => coef; set => coef = value; }


    }
}
