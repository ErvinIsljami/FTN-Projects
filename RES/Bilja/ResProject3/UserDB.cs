using QueueService.User;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueService
{
    public partial class UserDB : DbContext
    {
        public virtual DbSet<Client> Clients { get; set; }
        public UserDB() : base("UsersDataBase")
        {
            
        }


    }
}
