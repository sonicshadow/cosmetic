namespace Cosmetic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStock : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Stocks",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ProductID = c.Int(nullable: false),
                        UserID = c.String(),
                        Type = c.Int(nullable: false),
                        Count = c.Int(nullable: false),
                        Remark = c.String(),
                        CreateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Products", t => t.ProductID, cascadeDelete: true)
                .Index(t => t.ProductID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Stocks", "ProductID", "dbo.Products");
            DropIndex("dbo.Stocks", new[] { "ProductID" });
            DropTable("dbo.Stocks");
        }
    }
}
