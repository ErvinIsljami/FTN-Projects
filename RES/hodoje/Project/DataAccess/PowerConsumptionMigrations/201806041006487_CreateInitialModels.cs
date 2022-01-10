namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateInitialModels : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GeoAreas",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PowerConsumptionDatas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Timestamp = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        Consumption = c.Double(nullable: false),
                        GeoAreaId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GeoAreas", t => t.GeoAreaId, cascadeDelete: true)
                .Index(t => t.GeoAreaId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PowerConsumptionDatas", "GeoAreaId", "dbo.GeoAreas");
            DropIndex("dbo.PowerConsumptionDatas", new[] { "GeoAreaId" });
            DropTable("dbo.PowerConsumptionDatas");
            DropTable("dbo.GeoAreas");
        }
    }
}
