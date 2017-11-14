namespace Cosmetic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserProduct : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserProducts",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserID = c.String(maxLength: 128),
                        ProductID = c.Int(nullable: false),
                        Type = c.Int(nullable: false),
                        Min = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Products", t => t.ProductID, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserID)
                .Index(t => t.UserID)
                .Index(t => t.ProductID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserProducts", "UserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserProducts", "ProductID", "dbo.Products");
            DropIndex("dbo.UserProducts", new[] { "ProductID" });
            DropIndex("dbo.UserProducts", new[] { "UserID" });
            DropTable("dbo.UserProducts");
        }
    }
}
