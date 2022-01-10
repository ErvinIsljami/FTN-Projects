using EShoppy.Logistic_Module.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShoppy.Sale_Module.Implementation
{
    public class Product : IProduct
    {
        private Guid iD;
        private string name;
        private string description;
        private double price;
        private int state;

        public Product(string name, string description, double price, int state)
        {
            this.name = name;
            this.description = description;
            this.price = price;
            this.state = state;
            this.iD = Guid.NewGuid();
        }

        public Guid ID { get => iD; set => iD = value; }
        public string Name { get => name; set => name = value; }
        public string Description { get => description; set => description = value; }
        public double Price { get => price; set => price = value; }
        public int State { get => state; set => state = value; }
    }
}
