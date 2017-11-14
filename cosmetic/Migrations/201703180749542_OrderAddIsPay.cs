namespace Cosmetic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrderAddIsPay : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "IsPay", c => c.Boolean(nullable: false));
            AddColumn("dbo.SupplierProducts", "Count", c => c.Int(nullable: false));
            AddColumn("dbo.SupplierProducts", "Number", c => c.String());
            AddColumn("dbo.SupplierProducts", "CreateTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SupplierProducts", "CreateTime");
            DropColumn("dbo.SupplierProducts", "Number");
            DropColumn("dbo.SupplierProducts", "Count");
            DropColumn("dbo.Orders", "IsPay");
        }
    }
}
