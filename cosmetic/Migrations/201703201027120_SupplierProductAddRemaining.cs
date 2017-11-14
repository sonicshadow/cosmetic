namespace Cosmetic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SupplierProductAddRemaining : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SupplierProducts", "Remaining", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SupplierProducts", "Remaining");
        }
    }
}
