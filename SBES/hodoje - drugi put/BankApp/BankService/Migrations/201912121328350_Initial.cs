namespace BankService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BaseCommands",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        CreationTime = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                        State = c.Int(nullable: false),
                        TimedOut = c.Boolean(nullable: false),
                        Amount = c.Double(),
                        Username = c.String(),
                        Username1 = c.String(),
                        Amount1 = c.Double(),
                        Username2 = c.String(),
                        Amount2 = c.Double(),
                        Username3 = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.CommandNotifications",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        CommandState = c.Int(nullable: false),
                        CommandStatus = c.Int(nullable: false),
                        Information = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.CommandNotifications");
            DropTable("dbo.BaseCommands");
        }
    }
}
