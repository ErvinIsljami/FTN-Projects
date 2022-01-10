using EShoppy.Logistic_Module.Interfaces;
using EShoppy.Sale_Module.Interfaces;
using EShoppy.User_Module.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace EShoppy.Sale_Module.Implementation
{
    public class SalesManager : ISalesManager
    {
        public void CreateOffer(IClient client, List<IProduct> products, List<ITransport> transports)
        {
            if(client == null)
            {
                throw new ArgumentNullException();
            }

            if (products == null)
            {
                throw new ArgumentNullException();
            }

            if (transports == null)
            {
                throw new ArgumentNullException();
            }

            double price = products.Sum(x => x.Price);  //suma cene proizvoda
            IOffer offer = new Offer(client, products, transports, price, DateTime.Now);

            ShoppingOffers.Offers.Add(offer.ID, offer);
        }

        public void CreateProduct(string name, string description, int state, double price)
        {
            if(string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException();
            }

            if(string.IsNullOrEmpty(description))
            {
                throw new ArgumentNullException();
            }

            IProduct product = new Product(name, description, price, state);

            ShoppingOffers.Products.Add(product.ID, product);
        }

        public IOffer GetLowestOffer(List<IOffer> offers)
        {
            if(offers == null)
            {
                throw new ArgumentNullException();
            }

            double min = offers.Min(x => x.Price);

            return offers.FirstOrDefault(x => x.Price == min);
        }

        public List<IOffer> GetOffersByProductID(Guid productID)
        {
            if (productID == null)
            {
                throw new ArgumentNullException();
            }

            return ShoppingOffers.Offers.Values.Where(x => x.ListOfProducts.FirstOrDefault(y => y.ID == productID) != null).ToList();
        }

        public List<IOffer> GetOffersByTransportID(Guid transportID)
        {
            if(transportID == null)
            {
                throw new ArgumentNullException();
            }

            return ShoppingOffers.Offers.Values.Where(x => x.ListOfTransports.FirstOrDefault(y => y.ID == transportID) != null).ToList();
        }

        public double GetTransportCost(IOffer offer, ITransport transport)
        {
            if(offer == null)
            {
                throw new ArgumentNullException();
            }

            if (transport == null)
            {
                throw new ArgumentNullException();
            }

            if(!offer.ListOfTransports.Contains(transport))
            {
                throw new KeyNotFoundException();
            }

            return offer.Price * transport.Coef;
        }

        public void UpdateOffer(IOffer offer, double? price, List<IProduct> products, List<ITransport> transports, IClient client)
        {
            if(offer == null)
            {
                throw new ArgumentNullException();
            }

            //offer.Price = price;
            offer.ListOfProducts = products;
            offer.ListOfTransports = transports;
            offer.Client = client;
        }
    }
}
