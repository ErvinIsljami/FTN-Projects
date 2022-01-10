namespace BankService.Migrations.CommandingContext
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class CommandingConfiguration : DbMigrationsConfiguration<BankService.DatabaseManagement.BankCommandingContext>
    {
        public CommandingConfiguration()
        {
            MigrationsDirectory = @"Migrations\CommandingContext";
        }

        protected override void Seed(BankService.DatabaseManagement.BankCommandingContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
        }
    }
}
