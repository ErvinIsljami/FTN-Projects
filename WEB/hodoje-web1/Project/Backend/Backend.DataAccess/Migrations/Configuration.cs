using DomainEntities.Models;

namespace Backend.DataAccess.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Backend.DataAccess.DatabaseContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Backend.DataAccess.DatabaseContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            User admin = new User
            {
                Id = 1,
                Username = "adminadmin",
                Password = "adminadmin",
                Name = "nikolaadmin",
                Lastname = "karaklicadmin",
                Gender = (int) Gender.MALE,
                Email = "admin@admin.com",
                Role = (int)Role.DISPATCHER,
                IsBanned = false
            };
            context.Users.AddOrUpdate(admin);
            context.SaveChanges();
        }
    }
}
