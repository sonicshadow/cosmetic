namespace Cosmetic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProductAddRequired : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Products", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Products", "Info", c => c.String(nullable: false));
            AlterColumn("dbo.Products", "Number", c => c.String(nullable: false));
            AlterColumn("dbo.Products", "Unit", c => c.String(nullable: false));
            AlterColumn("dbo.AspNetUsers", "RealName", c => c.String(nullable: false));
            AlterColumn("dbo.AspNetUsers", "IDCard", c => c.String(nullable: false));
            AlterColumn("dbo.AspNetUsers", "Bank", c => c.String(nullable: false));
            AlterColumn("dbo.AspNetUsers", "BankCard", c => c.String(nullable: false));
            AlterColumn("dbo.AspNetUsers", "ParentID", c => c.String(nullable: false));
            AlterColumn("dbo.AspNetUsers", "RecommendID", c => c.String(nullable: false));
            AlterColumn("dbo.AspNetUsers", "Address", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "Address", c => c.String());
            AlterColumn("dbo.AspNetUsers", "RecommendID", c => c.String());
            AlterColumn("dbo.AspNetUsers", "ParentID", c => c.String());
            AlterColumn("dbo.AspNetUsers", "BankCard", c => c.String());
            AlterColumn("dbo.AspNetUsers", "Bank", c => c.String());
            AlterColumn("dbo.AspNetUsers", "IDCard", c => c.String());
            AlterColumn("dbo.AspNetUsers", "RealName", c => c.String());
            AlterColumn("dbo.Products", "Unit", c => c.String());
            AlterColumn("dbo.Products", "Number", c => c.String());
            AlterColumn("dbo.Products", "Info", c => c.String());
            AlterColumn("dbo.Products", "Name", c => c.String());
        }
    }
}
