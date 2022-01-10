namespace BankService.Migrations.DomainContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BankAccounts",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        AccountNumber = c.String(),
                        Amount = c.Double(nullable: false),
                        UserId = c.Int(nullable: false),
                        User_ID = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Users", t => t.User_ID)
                .Index(t => t.User_ID);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        Username = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BankAccounts", "User_ID", "dbo.Users");
            DropIndex("dbo.BankAccounts", new[] { "User_ID" });
            DropTable("dbo.Users");
            DropTable("dbo.BankAccounts");
        }
    }
}
