namespace UI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Batteries",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 128),
                        MaxPower = c.Double(nullable: false),
                        Capacity = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Consumers",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 128),
                        Consumption = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Measurements",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        TimeStamp = c.DateTime(nullable: false),
                        Value = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SolarPanels",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 128),
                        MaxPower = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.SolarPanels");
            DropTable("dbo.Measurements");
            DropTable("dbo.Consumers");
            DropTable("dbo.Batteries");
        }
    }
}
