using EShoppy.Finantial_Module.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShoppy.Finantial_Module.Implementation
{
    public class Credit : ICredit
    {
        private Guid iD;
        private string name;
        private double interest;
        private ICurrency currency;
        private IBank bank;
        private int minimumYear;

        public Credit(string name, double interest, ICurrency currency, IBank bank, int minimumYear)
        {
            this.name = name;
            this.interest = interest;
            this.currency = currency;
            this.bank = bank;
            this.minimumYear = minimumYear;
            ID = Guid.NewGuid();
        }

        public Guid ID { get => iD; set => iD = value; }
        public string Name { get => name; set => name = value; }
        public double Interest { get => interest; set => interest = value; }
        public ICurrency Currency { get => currency; set => currency = value; }
        public IBank Bank { get => bank; set => bank = value; }
        public int MinimumYear { get => minimumYear; set => minimumYear = value; }


    }
}
