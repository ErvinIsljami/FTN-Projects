using EShoppy.Logistic_Module.Interfaces;
using EShoppy.Sale_Module.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShoppy.Sale_Module
{
    public static class ShoppingOffers
    {
        public static Dictionary<Guid, IOffer> Offers { get; private set; }
        public static Dictionary<Guid, IProduct> Products { get; private set; }

        static ShoppingOffers()
        {
            Offers = new Dictionary<Guid, IOffer>();
            Products = new Dictionary<Guid, IProduct>();
        }

    }
}
