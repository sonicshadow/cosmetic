namespace Cosmetic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProductAndSupplier : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Info = c.String(),
                        Spec = c.String(),
                        Number = c.String(),
                        Unit = c.String(),
                        Release = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.SupplierProducts",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ProductID = c.Int(nullable: false),
                        SupplierID = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Products", t => t.ProductID, cascadeDelete: true)
                .ForeignKey("dbo.Suppliers", t => t.SupplierID, cascadeDelete: true)
                .Index(t => t.ProductID)
                .Index(t => t.SupplierID);
            
            CreateTable(
                "dbo.Suppliers",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        PhoneNumber = c.String(),
                        Bank = c.String(),
                        BankCard = c.String(),
                        Address = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SupplierProducts", "SupplierID", "dbo.Suppliers");
            DropForeignKey("dbo.SupplierProducts", "ProductID", "dbo.Products");
            DropIndex("dbo.SupplierProducts", new[] { "SupplierID" });
            DropIndex("dbo.SupplierProducts", new[] { "ProductID" });
            DropTable("dbo.Suppliers");
            DropTable("dbo.SupplierProducts");
            DropTable("dbo.Products");
        }
    }
}
