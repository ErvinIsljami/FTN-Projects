namespace BankService.Migrations.DomainContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedUsernameToBankAccount : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BankAccounts", "Username", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.BankAccounts", "Username");
        }
    }
}
