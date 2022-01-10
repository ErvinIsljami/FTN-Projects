namespace BankService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedUserAndBankAccount : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.BaseCommands", "Amount");
            RenameColumn(table: "dbo.BaseCommands", name: "Amount1", newName: "Amount");
            CreateTable(
                "dbo.BankAccounts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AccountNumber = c.String(),
                        Amount = c.Double(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Username = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.BaseCommands", "Months", c => c.Int());
            AddColumn("dbo.BaseCommands", "TransactionType", c => c.Int());
            DropColumn("dbo.BaseCommands", "Amount2");
        }
        
        public override void Down()
        {
            AddColumn("dbo.BaseCommands", "Amount2", c => c.Double());
            DropForeignKey("dbo.BankAccounts", "UserId", "dbo.Users");
            DropIndex("dbo.BankAccounts", new[] { "UserId" });
            DropColumn("dbo.BaseCommands", "TransactionType");
            DropColumn("dbo.BaseCommands", "Months");
            DropTable("dbo.Users");
            DropTable("dbo.BankAccounts");
            RenameColumn(table: "dbo.BaseCommands", name: "Amount", newName: "Amount1");
            AddColumn("dbo.BaseCommands", "Amount", c => c.Double());
        }
    }
}
