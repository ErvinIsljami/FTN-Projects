namespace BankService.Migrations.DomainContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Loan : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.BankAccounts", "User_ID", "dbo.Users");
            DropIndex("dbo.BankAccounts", new[] { "User_ID" });
            DropColumn("dbo.BankAccounts", "UserId");
            RenameColumn(table: "dbo.BankAccounts", name: "User_ID", newName: "UserId");
            CreateTable(
                "dbo.Loans",
                c => new
                    {
                        ID = c.Long(nullable: false),
                        Amount = c.Double(nullable: false),
                        Months = c.Int(nullable: false),
                        UseId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Users", t => t.ID)
                .Index(t => t.ID);
            
            AddColumn("dbo.Users", "LoanId", c => c.Long(nullable: false));
            AlterColumn("dbo.BankAccounts", "UserId", c => c.Long(nullable: false));
            AlterColumn("dbo.BankAccounts", "UserId", c => c.Long(nullable: false));
            CreateIndex("dbo.BankAccounts", "UserId");
            AddForeignKey("dbo.BankAccounts", "UserId", "dbo.Users", "ID", cascadeDelete: true);
            DropColumn("dbo.BankAccounts", "AccountNumber");
        }
        
        public override void Down()
        {
            AddColumn("dbo.BankAccounts", "AccountNumber", c => c.String());
            DropForeignKey("dbo.BankAccounts", "UserId", "dbo.Users");
            DropForeignKey("dbo.Loans", "ID", "dbo.Users");
            DropIndex("dbo.Loans", new[] { "ID" });
            DropIndex("dbo.BankAccounts", new[] { "UserId" });
            AlterColumn("dbo.BankAccounts", "UserId", c => c.Long());
            AlterColumn("dbo.BankAccounts", "UserId", c => c.Int(nullable: false));
            DropColumn("dbo.Users", "LoanId");
            DropTable("dbo.Loans");
            RenameColumn(table: "dbo.BankAccounts", name: "UserId", newName: "User_ID");
            AddColumn("dbo.BankAccounts", "UserId", c => c.Int(nullable: false));
            CreateIndex("dbo.BankAccounts", "User_ID");
            AddForeignKey("dbo.BankAccounts", "User_ID", "dbo.Users", "ID");
        }
    }
}
