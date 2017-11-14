namespace Cosmetic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeliverHistoriesAddOrderId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DeliverHistories", "OrderID", c => c.Int(nullable: false));
            AddColumn("dbo.Orders", "ProductID", c => c.Int(nullable: false));
            CreateIndex("dbo.DeliverHistories", "OrderID");
            AddForeignKey("dbo.DeliverHistories", "OrderID", "dbo.Orders", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DeliverHistories", "OrderID", "dbo.Orders");
            DropIndex("dbo.DeliverHistories", new[] { "OrderID" });
            DropColumn("dbo.Orders", "ProductID");
            DropColumn("dbo.DeliverHistories", "OrderID");
        }
    }
}
