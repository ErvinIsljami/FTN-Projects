namespace BankService.Migrations.DomainContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserIsNullable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.BankAccounts", "UserId", "dbo.Users");
            DropIndex("dbo.BankAccounts", new[] { "UserId" });
            AlterColumn("dbo.BankAccounts", "UserId", c => c.Long());
            CreateIndex("dbo.BankAccounts", "UserId");
            AddForeignKey("dbo.BankAccounts", "UserId", "dbo.Users", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BankAccounts", "UserId", "dbo.Users");
            DropIndex("dbo.BankAccounts", new[] { "UserId" });
            AlterColumn("dbo.BankAccounts", "UserId", c => c.Long(nullable: false));
            CreateIndex("dbo.BankAccounts", "UserId");
            AddForeignKey("dbo.BankAccounts", "UserId", "dbo.Users", "ID", cascadeDelete: true);
        }
    }
}
