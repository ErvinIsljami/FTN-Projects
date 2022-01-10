using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace QueueService.User
{
    [Serializable]
    public class DataModel
    {

        private List<Item> items;
        private List<Position> positions;
        private string id;
        private List<Client> clients;

        public DataModel()
        {
            this.Items = new List<Item>();
            this.positions = new List<Position>();
            this.clients = new List<Client>();
            id = "";
        }

        public List<Position> Positions { get => positions; set => positions = value; }

        public List<Item> Items { get => items; set => items = value; }
        [Key]
        public string Id { get => id; set => id = value; }

        [NotMapped]
        public List<Client> Clients { get => clients; set => clients = value; }
    }
}
