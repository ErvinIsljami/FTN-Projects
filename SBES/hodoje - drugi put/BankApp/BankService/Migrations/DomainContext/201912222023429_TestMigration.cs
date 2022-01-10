namespace BankService.Migrations.DomainContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TestMigration : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Loans", new[] { "ID" });
            DropPrimaryKey("dbo.Loans");
            AlterColumn("dbo.Loans", "ID", c => c.Long(nullable: false));
            AddPrimaryKey("dbo.Loans", "ID");
            CreateIndex("dbo.Loans", "ID");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Loans", new[] { "ID" });
            DropPrimaryKey("dbo.Loans");
            AlterColumn("dbo.Loans", "ID", c => c.Long(nullable: false, identity: true));
            AddPrimaryKey("dbo.Loans", "ID");
            CreateIndex("dbo.Loans", "ID");
        }
    }
}
