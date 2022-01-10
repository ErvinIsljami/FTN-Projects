namespace BankService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Notification : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NotificationInformations",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        Username = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.CommandNotifications", "NotificationState", c => c.Int(nullable: false));
            AddColumn("dbo.CommandNotifications", "Username", c => c.String());
            DropColumn("dbo.CommandNotifications", "CommandState");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CommandNotifications", "CommandState", c => c.Int(nullable: false));
            DropColumn("dbo.CommandNotifications", "Username");
            DropColumn("dbo.CommandNotifications", "NotificationState");
            DropTable("dbo.NotificationInformations");
        }
    }
}
