namespace BankService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CommandUsername : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.BaseCommands", "Username1");
            DropColumn("dbo.BaseCommands", "Username2");
            DropColumn("dbo.BaseCommands", "Username3");
        }
        
        public override void Down()
        {
            AddColumn("dbo.BaseCommands", "Username3", c => c.String());
            AddColumn("dbo.BaseCommands", "Username2", c => c.String());
            AddColumn("dbo.BaseCommands", "Username1", c => c.String());
        }
    }
}
