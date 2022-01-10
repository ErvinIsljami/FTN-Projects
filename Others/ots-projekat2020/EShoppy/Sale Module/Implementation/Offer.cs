using EShoppy.Logistic_Module.Interfaces;
using EShoppy.User_Module.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShoppy.Sale_Module.Implementation
{
    public class Offer : IOffer
    {
        private Guid iD;
        private IClient client;
        private List<IProduct> listOfProducts;
        private List<ITransport> listOfTransports;
        private double price;
        private DateTime submitionDate;

        public Offer(IClient client, List<IProduct> listOfProducts, List<ITransport> listOfTransports, double price, DateTime submitionDate)
        {
            this.client = client;
            this.listOfProducts = listOfProducts;
            this.listOfTransports = listOfTransports;
            this.price = price;
            this.submitionDate = submitionDate;
            this.ID = Guid.NewGuid();
        }

        public Guid ID { get => iD; set => iD = value; }
        public IClient Client { get => client; set => client = value; }
        public List<IProduct> ListOfProducts { get => listOfProducts; set => listOfProducts = value; }
        public double Price { get => price; set => price = value; }
        public DateTime SubmitionDate { get => submitionDate; set => submitionDate = value; }
        public List<ITransport> ListOfTransports { get => listOfTransports; set => listOfTransports = value; }
    }
}
