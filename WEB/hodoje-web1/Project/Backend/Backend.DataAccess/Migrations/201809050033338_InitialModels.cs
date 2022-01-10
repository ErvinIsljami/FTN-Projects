namespace Backend.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialModels : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cars",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        YearOfManufactoring = c.Int(nullable: false),
                        RegistrationNumber = c.String(nullable: false),
                        TaxiNumber = c.Int(nullable: false),
                        CarType = c.Int(nullable: false),
                        DriverId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Username = c.String(nullable: false, maxLength: 450),
                        Password = c.String(nullable: false, maxLength: 30),
                        Name = c.String(nullable: false, maxLength: 30),
                        Lastname = c.String(nullable: false, maxLength: 30),
                        Gender = c.Int(nullable: false),
                        NationalIdentificationNumber = c.String(maxLength: 13),
                        PhoneNumber = c.String(maxLength: 10),
                        Email = c.String(nullable: false, maxLength: 254),
                        IsBanned = c.Boolean(nullable: false),
                        Role = c.Int(nullable: false),
                        DriverLocationId = c.Int(),
                        CarId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Locations", t => t.DriverLocationId)
                .Index(t => t.Username, unique: true)
                .Index(t => t.DriverLocationId);
            
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(nullable: false, maxLength: 1000),
                        Timestamp = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        UserId = c.Int(nullable: false),
                        RideId = c.Int(nullable: false),
                        Rating = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Rides", t => t.RideId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.RideId);
            
            CreateTable(
                "dbo.Rides",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Timestamp = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        StartLocationId = c.Int(nullable: false),
                        DestinationLocationId = c.Int(),
                        Price = c.Double(nullable: false),
                        RideStatus = c.Int(nullable: false),
                        CarType = c.Int(nullable: false),
                        CustomerId = c.Int(),
                        DispatcherId = c.Int(),
                        DriverId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CustomerId)
                .ForeignKey("dbo.Locations", t => t.DestinationLocationId)
                .ForeignKey("dbo.Users", t => t.DispatcherId)
                .ForeignKey("dbo.Users", t => t.DriverId)
                .ForeignKey("dbo.Locations", t => t.StartLocationId)
                .Index(t => t.Timestamp, unique: true)
                .Index(t => t.StartLocationId)
                .Index(t => t.DestinationLocationId)
                .Index(t => t.CustomerId)
                .Index(t => t.DispatcherId)
                .Index(t => t.DriverId);
            
            CreateTable(
                "dbo.Locations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Address_StreetName = c.String(nullable: false),
                        Address_StreetNumber = c.String(nullable: false),
                        Address_City = c.String(nullable: false),
                        Address_PostalCode = c.String(nullable: false),
                        Longitude = c.Double(nullable: false),
                        Latitude = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Users", "DriverLocationId", "dbo.Locations");
            DropForeignKey("dbo.Comments", "UserId", "dbo.Users");
            DropForeignKey("dbo.Rides", "StartLocationId", "dbo.Locations");
            DropForeignKey("dbo.Rides", "DriverId", "dbo.Users");
            DropForeignKey("dbo.Rides", "DispatcherId", "dbo.Users");
            DropForeignKey("dbo.Rides", "DestinationLocationId", "dbo.Locations");
            DropForeignKey("dbo.Rides", "CustomerId", "dbo.Users");
            DropForeignKey("dbo.Comments", "RideId", "dbo.Rides");
            DropForeignKey("dbo.Cars", "Id", "dbo.Users");
            DropIndex("dbo.Rides", new[] { "DriverId" });
            DropIndex("dbo.Rides", new[] { "DispatcherId" });
            DropIndex("dbo.Rides", new[] { "CustomerId" });
            DropIndex("dbo.Rides", new[] { "DestinationLocationId" });
            DropIndex("dbo.Rides", new[] { "StartLocationId" });
            DropIndex("dbo.Rides", new[] { "Timestamp" });
            DropIndex("dbo.Comments", new[] { "RideId" });
            DropIndex("dbo.Comments", new[] { "UserId" });
            DropIndex("dbo.Users", new[] { "DriverLocationId" });
            DropIndex("dbo.Users", new[] { "Username" });
            DropIndex("dbo.Cars", new[] { "Id" });
            DropTable("dbo.Locations");
            DropTable("dbo.Rides");
            DropTable("dbo.Comments");
            DropTable("dbo.Users");
            DropTable("dbo.Cars");
        }
    }
}
