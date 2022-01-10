using EShoppy.Logistic_Module.Interfaces;
using EShoppy.User_Module.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShoppy.Sale_Module.Interfaces
{
    public interface ISalesManager
    {
        void CreateProduct(string name, string description, int state, double price);

        void CreateOffer(IClient client, List<IProduct> products, List<ITransport> transports);

        List<IOffer> GetOffersByTransportID(Guid transportID);

        List<IOffer> GetOffersByProductID(Guid productID);

        IOffer GetLowestOffer(List<IOffer> offers);

        double GetTransportCost(IOffer offer, ITransport transport);

        void UpdateOffer(IOffer offer, double? price, List<IProduct> products, List<ITransport> transports, IClient client);
    }
}
