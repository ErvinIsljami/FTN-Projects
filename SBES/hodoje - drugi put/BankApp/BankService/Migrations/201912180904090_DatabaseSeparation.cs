namespace BankService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DatabaseSeparation : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.BankAccounts", "UserId", "dbo.Users");
            DropIndex("dbo.BankAccounts", new[] { "UserId" });
            DropTable("dbo.BankAccounts");
            DropTable("dbo.Users");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Username = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.BankAccounts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AccountNumber = c.String(),
                        Amount = c.Double(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.BankAccounts", "UserId");
            AddForeignKey("dbo.BankAccounts", "UserId", "dbo.Users", "Id", cascadeDelete: true);
        }
    }
}
