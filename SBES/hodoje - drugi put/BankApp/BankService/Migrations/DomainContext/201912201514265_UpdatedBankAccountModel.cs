namespace BankService.Migrations.DomainContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedBankAccountModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BankAccounts", "AccountName", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BankAccounts", "AccountName");
        }
    }
}
