using EShoppy.Logistic_Module.Interfaces;
using EShoppy.Transactional_Module.Interfaces;
using EShoppy.User_Module.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShoppy.Transactional_Module.Implementation
{
    public class Transaction : ITransaction
    {
        private Guid iD;
        private IClient customer;
        private IOffer offer;
        private ITransactionType transactionType;
        private double price;
        private double discount;
        private DateTime date;
        private int rating;

        public Transaction(IClient customer, IOffer offer, ITransactionType transactionType, double price, double discount, DateTime date, int rating)
        {
            this.customer = customer;
            this.offer = offer;
            this.transactionType = transactionType;
            this.price = price;
            this.discount = discount;
            this.date = date;
            this.rating = rating;
            ID = Guid.NewGuid();
        }

        public Transaction()
        {
            ID = Guid.NewGuid();
        }

        public Guid ID { get => iD; set => iD = value; }
        public IClient Customer { get => customer; set => customer = value; }
        public IOffer Offer { get => offer; set => offer = value; }
        public ITransactionType TransactionType { get => transactionType; set => transactionType = value; }
        public double Price { get => price; set => price = value; }
        public double Discount { get => discount; set => discount = value; }
        public DateTime Date { get => date; set => date = value; }
        public int Rating { get => rating; set => rating = value; }


    }
}
