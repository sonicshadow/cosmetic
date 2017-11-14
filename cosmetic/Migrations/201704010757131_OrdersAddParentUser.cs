namespace Cosmetic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrdersAddParentUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "ParentUser", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "ParentUser");
        }
    }
}
