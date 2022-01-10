namespace BankService.Migrations.CommandingContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialDatbaseSepration : DbMigration
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
                        Username = c.String(),
                        TimedOut = c.Boolean(nullable: false),
                        Amount = c.Double(),
                        Months = c.Int(),
                        BankAccountId = c.Long(),
                        Amount1 = c.Double(),
                        TransactionType = c.Int(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.NotificationInformations",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        Username = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.CommandNotifications",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        NotificationState = c.Int(nullable: false),
                        CommandStatus = c.Int(nullable: false),
                        Username = c.String(),
                        Information = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.CommandNotifications");
            DropTable("dbo.NotificationInformations");
            DropTable("dbo.BaseCommands");
        }
    }
}
