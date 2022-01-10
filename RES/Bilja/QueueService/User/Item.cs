using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace QueueService.User
{

    public class Item
    {
       
        private string name;
        private double quantity;
        private bool isActive;
        private double destructivePower;

        public Item(string name, double quantity, bool isActive, double destructivePower)
        {
            this.name = name;
            this.quantity = quantity;
            this.isActive = isActive;
            this.destructivePower = destructivePower;
        }

        public Item(string name)
        {
            this.name = name;
        }

        public Item()
        {
            name = "";
        }
        [Key]
        public string Name { get => name; set => name = value; }

        public double Quantity { get => quantity; set => quantity = value; }

        public bool IsActive { get => isActive; set => isActive = value; }

        public double DestructivePower { get => destructivePower; set => destructivePower = value; }
    }
}
