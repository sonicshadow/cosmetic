namespace Cosmetic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddReturn : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Returns",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        User = c.String(),
                        Time = c.DateTime(nullable: false),
                        OrderID = c.Int(nullable: false),
                        ProductID = c.Int(nullable: false),
                        Count = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CheckState = c.Int(nullable: false),
                        CheckTime = c.DateTime(),
                        CheckUser = c.String(),
                        PayTime = c.DateTime(),
                        ReceiptTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Orders", t => t.OrderID, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.ProductID, cascadeDelete: true)
                .Index(t => t.OrderID)
                .Index(t => t.ProductID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Returns", "ProductID", "dbo.Products");
            DropForeignKey("dbo.Returns", "OrderID", "dbo.Orders");
            DropIndex("dbo.Returns", new[] { "ProductID" });
            DropIndex("dbo.Returns", new[] { "OrderID" });
            DropTable("dbo.Returns");
        }
    }
}
