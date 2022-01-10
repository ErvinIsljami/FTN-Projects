namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateInitialModels : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PowerConsumptionDatas", "GeoAreaId", "dbo.GeoAreas");
            DropPrimaryKey("dbo.GeoAreas");
            AddColumn("dbo.GeoAreas", "Id", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.GeoAreas", "Id");
            AddForeignKey("dbo.PowerConsumptionDatas", "GeoAreaId", "dbo.GeoAreas", "Id", cascadeDelete: true);
            DropColumn("dbo.GeoAreas", "GeoAreaId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.GeoAreas", "GeoAreaId", c => c.String(nullable: false, maxLength: 128));
            DropForeignKey("dbo.PowerConsumptionDatas", "GeoAreaId", "dbo.GeoAreas");
            DropPrimaryKey("dbo.GeoAreas");
            DropColumn("dbo.GeoAreas", "Id");
            AddPrimaryKey("dbo.GeoAreas", "GeoAreaId");
            AddForeignKey("dbo.PowerConsumptionDatas", "GeoAreaId", "dbo.GeoAreas", "GeoAreaId", cascadeDelete: true);
        }
    }
}
