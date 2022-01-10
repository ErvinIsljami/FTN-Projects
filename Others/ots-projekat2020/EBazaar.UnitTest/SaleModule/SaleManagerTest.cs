using EShoppy.Logistic_Module.Interfaces;
using EShoppy.Sale_Module;
using EShoppy.Sale_Module.Implementation;
using EShoppy.User_Module.Implementation;
using EShoppy.User_Module.Interfaces;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBazaar.UnitTest.SaleModule
{
    [TestFixture]
    public class SaleManagerTest
    {
        [Test]
        [TestCase("asdfasdf", "retewrtwert", 1, 4324)]
        [TestCase("asdfgsdfg", "hfdghfgh", 0, 87567)]
        [TestCase("asdfasdf", "yrthrth", 1, 6435)]
        [TestCase("asdfasdf", "vcbnvcbn", 1, 5345)]
        [TestCase("hdfghthr", "nvbngn", 1, 6767)]
        public void CreateProduct_ValidTest(string name, string description, int state, double price)
        {
            SalesManager salesManager = new SalesManager();

            int numberOfProducts = ShoppingOffers.Products.Count;

            salesManager.CreateProduct(name, description, state, price);

            Assert.AreEqual(numberOfProducts + 1, ShoppingOffers.Products.Count);
        }

        [Test]
        [TestCase("asdfasdf", "", 1, 4324)]
        [TestCase("asdfgsdfg", "", 0, 87567)]
        [TestCase("", "yrthrth", 1, 6435)]
        [TestCase("", "", 1, 5345)]
        [TestCase("", "nvbngn", 1, 6767)]
        public void CreateProduct_InvalidTest(string name, string description, int state, double price)
        {
            SalesManager salesManager = new SalesManager();

            int numberOfProducts = ShoppingOffers.Products.Count;
            Assert.Throws<ArgumentNullException>(() => salesManager.CreateProduct(name, description, state, price));
        }

        [Test]
        public void CreateOffer_ValidTest()
        {
            SalesManager salesManager = new SalesManager();

            int numberOfOffers = ShoppingOffers.Offers.Count;

            salesManager.CreateOffer(new FizickoLice(), new List<IProduct>(), new List<ITransport>());

            Assert.AreEqual(numberOfOffers + 1, ShoppingOffers.Offers.Count);
        }

        [Test]
        [TestCase(null, null, null)]
        public void CreateOffer_InvalidTest(IClient client, List<IProduct> products, List<ITransport> transports)
        {
            SalesManager salesManager = new SalesManager();

            int numberOfProducts = ShoppingOffers.Offers.Count;

            Assert.Throws<ArgumentNullException>(() => salesManager.CreateOffer(client, products, transports));
        }

        [Test]
        public void GetOffersByTransportID_ValidTest()
        {
            SalesManager salesManager = new SalesManager();

            ITransport transport = new Transport("gojowgijweoirg", 3);
            List<ITransport> transportList = new List<ITransport>();
            transportList.Add(transport);

            salesManager.CreateOffer(new FizickoLice(), new List<IProduct>(), transportList);
            salesManager.CreateOffer(new FizickoLice(), new List<IProduct>(), transportList);
            salesManager.CreateOffer(new FizickoLice(), new List<IProduct>(), transportList);

            var result = salesManager.GetOffersByTransportID(transport.ID);

            Assert.IsNotNull(result);
            Assert.AreEqual(result.Count, 3);
        }

        [Test]
        public void GetOffersByTransportID_InvalidGuidTest()
        {
            SalesManager salesManager = new SalesManager();

            ITransport transport = new Transport("gojowgijweoirg", 3);
            List<ITransport> transportList = new List<ITransport>();
            transportList.Add(transport);

            salesManager.CreateOffer(new FizickoLice(), new List<IProduct>(), transportList);
            salesManager.CreateOffer(new FizickoLice(), new List<IProduct>(), transportList);
            salesManager.CreateOffer(new FizickoLice(), new List<IProduct>(), transportList);

            var result = salesManager.GetOffersByTransportID(Guid.NewGuid());

            Assert.IsNotNull(result);
            Assert.AreEqual(result.Count, 0);
        }

        [Test]
        public void GetOffersByProductID_ValidTest()
        {
            SalesManager salesManager = new SalesManager();

            IProduct product = new Product("dgfsafg", "fgwerf", 4234, 1);
            List<IProduct> productList = new List<IProduct>();
            productList.Add(product);

            salesManager.CreateOffer(new FizickoLice(), productList, new List<ITransport>());
            salesManager.CreateOffer(new FizickoLice(), productList, new List<ITransport>());
            salesManager.CreateOffer(new FizickoLice(), productList, new List<ITransport>());

            var result = salesManager.GetOffersByProductID(product.ID);

            Assert.IsNotNull(result);
            Assert.AreEqual(result.Count, 3);
        }

        [Test]
        public void GetOffersByProductID_InvalidGuidTest()
        {
            SalesManager salesManager = new SalesManager();

            IProduct product = new Product("dgfsafg", "fgwerf", 4234, 1);
            List<IProduct> productList = new List<IProduct>();
            productList.Add(product);

            salesManager.CreateOffer(new FizickoLice(), productList, new List<ITransport>());
            salesManager.CreateOffer(new FizickoLice(), productList, new List<ITransport>());
            salesManager.CreateOffer(new FizickoLice(), productList, new List<ITransport>());

            var result = salesManager.GetOffersByProductID(Guid.NewGuid());

            Assert.IsNotNull(result);
            Assert.AreEqual(result.Count, 0);
        }

        [Test]
        public void GetLowestOffer_ValidTest()
        {
            SalesManager salesManager = new SalesManager();

            IProduct product1 = new Product("dgfsafg", "fgwerf", 4234, 1);
            IProduct product2 = new Product("dgfsafg", "fgwerf", 7568756, 1);
            IProduct product3 = new Product("dgfsafg", "fgwerf", 345345, 1);
            List<IProduct> productList1 = new List<IProduct>();
            List<IProduct> productList2 = new List<IProduct>();
            productList1.Add(product1);
            productList2.Add(product2);
            productList2.Add(product3);
            ShoppingOffers.Offers.Clear();
            salesManager.CreateOffer(new FizickoLice(), productList2, new List<ITransport>());
            salesManager.CreateOffer(new FizickoLice(), productList2, new List<ITransport>());
            salesManager.CreateOffer(new FizickoLice(), productList1, new List<ITransport>());

            IOffer result = salesManager.GetLowestOffer(ShoppingOffers.Offers.Values.ToList());

            Assert.IsNotNull(result);
            Assert.AreEqual(result.Price, 4234);
        }

        [Test]
        public void GetLowestOffer_InvalidTest()
        {
            SalesManager salesManager = new SalesManager();

            IProduct product1 = new Product("dgfsafg", "fgwerf", 4234, 1);
            IProduct product2 = new Product("dgfsafg", "fgwerf", 7568756, 1);
            IProduct product3 = new Product("dgfsafg", "fgwerf", 345345, 1);
            List<IProduct> productList1 = new List<IProduct>();
            List<IProduct> productList2 = new List<IProduct>();
            productList1.Add(product1);
            productList2.Add(product2);
            productList2.Add(product3);

            salesManager.CreateOffer(new FizickoLice(), productList2, new List<ITransport>());
            salesManager.CreateOffer(new FizickoLice(), productList2, new List<ITransport>());
            salesManager.CreateOffer(new FizickoLice(), productList1, new List<ITransport>());

            Assert.Throws<ArgumentNullException>(() => salesManager.GetLowestOffer(null));
        }

        [Test]
        [TestCase(1000, 3)] 
        [TestCase(2000, 5)]
        [TestCase(1600, 6)]
        public void GetTransportCost_ValidTest(double price, double coef)
        {
            SalesManager salesManager = new SalesManager();

            ITransport transport1 = new Transport("afsdfasdf", coef);
            List<ITransport> transportList = new List<ITransport>();
            transportList.Add(transport1);
            IOffer offer = new Offer(new FizickoLice(), new List<IProduct>(), transportList, price, DateTime.Now);

            double price1 = salesManager.GetTransportCost(offer, transport1);

            Assert.AreEqual(price1, price * coef);
        }

        [Test]
        [TestCase(1000, 3)]
        [TestCase(2000, 5)]
        [TestCase(1600, 6)]
        public void GetTransportCost_InvalidTest(double price, double coef)
        {
            SalesManager salesManager = new SalesManager();

            ITransport transport1 = new Transport("afsdfasdf", coef);
            ITransport transport2 = new Transport("afsdfasdf", coef);
            List<ITransport> transportList = new List<ITransport>();
            transportList.Add(transport1);
            IOffer offer = new Offer(new FizickoLice(), new List<IProduct>(), transportList, price, DateTime.Now);

            Assert.Throws<KeyNotFoundException>(() => salesManager.GetTransportCost(offer, transport2));
        }

        [Test]
        [TestCase(1000, 3)]
        [TestCase(2000, 5)]
        [TestCase(1600, 6)]
        public void GetTransportCost_InvalidNullTest(double price, double coef)
        {
            SalesManager salesManager = new SalesManager();

            ITransport transport1 = new Transport("afsdfasdf", coef);
            ITransport transport2 = new Transport("afsdfasdf", coef);
            List<ITransport> transportList = new List<ITransport>();
            transportList.Add(transport1);
            IOffer offer = new Offer(new FizickoLice(), new List<IProduct>(), transportList, price, DateTime.Now);

            Assert.Throws<KeyNotFoundException>(() => salesManager.GetTransportCost(offer, transport2));
        }
    }
}
