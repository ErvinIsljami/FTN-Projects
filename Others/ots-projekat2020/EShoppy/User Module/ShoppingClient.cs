using EShoppy.User_Module.Implementation;
using EShoppy.User_Module.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShoppy.User_Module
{
    public static class ShoppingClient
    {
        public static List<IClient> Clients { get; private set; }
        static ShoppingClient()
        {
            Clients = new List<IClient>();
        }
    }
}
