using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueService.User
{
    public class DataModel
    {
        private List<Item> items;
        private List<Position> positions;
        private List<Client> clients;

        public DataModel()
        {
            this.Items = new List<Item>();
            this.positions = new List<Position>();
            this.clients = new List<Client>();
        }

        public List<Position> Positions { get => positions; set => positions = value; }
        public List<Item> Items { get => items; set => items = value; }
        public List<Client> Clients { get => clients; set => clients = value; }
    }
}
