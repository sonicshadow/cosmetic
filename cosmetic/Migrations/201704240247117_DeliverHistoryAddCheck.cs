namespace Cosmetic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeliverHistoryAddCheck : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DeliverHistories", "CheckTime", c => c.DateTime());
            AddColumn("dbo.DeliverHistories", "CheckState", c => c.Int(nullable: false));
            AddColumn("dbo.DeliverHistories", "CheckUser", c => c.String());
            AddColumn("dbo.Holders", "IDCard", c => c.String());
            AddColumn("dbo.Holders", "BankName", c => c.String());
            AddColumn("dbo.Holders", "BankCard", c => c.String());
            AddColumn("dbo.Holders", "Phone", c => c.String());
            AddColumn("dbo.ProductDetails", "TwiceMin", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProductDetails", "TwiceMin");
            DropColumn("dbo.Holders", "Phone");
            DropColumn("dbo.Holders", "BankCard");
            DropColumn("dbo.Holders", "BankName");
            DropColumn("dbo.Holders", "IDCard");
            DropColumn("dbo.DeliverHistories", "CheckUser");
            DropColumn("dbo.DeliverHistories", "CheckState");
            DropColumn("dbo.DeliverHistories", "CheckTime");
        }
    }
}
