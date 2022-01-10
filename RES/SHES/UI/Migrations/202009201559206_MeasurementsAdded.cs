namespace UI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MeasurementsAdded : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Measurements", newName: "UtilityConsumptions");
            DropPrimaryKey("dbo.UtilityConsumptions");
            AddColumn("dbo.UtilityConsumptions", "ComponentId", c => c.String());
            AlterColumn("dbo.UtilityConsumptions", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.UtilityConsumptions", "Id");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.UtilityConsumptions");
            AlterColumn("dbo.UtilityConsumptions", "Id", c => c.String(nullable: false, maxLength: 128));
            DropColumn("dbo.UtilityConsumptions", "ComponentId");
            AddPrimaryKey("dbo.UtilityConsumptions", "Id");
            RenameTable(name: "dbo.UtilityConsumptions", newName: "Measurements");
        }
    }
}
