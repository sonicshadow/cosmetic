namespace Cosmetic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserProductAddCount : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserProducts", "Sent", c => c.Int(nullable: false));
            AddColumn("dbo.UserProducts", "Sum", c => c.Int(nullable: false));
            AddColumn("dbo.UserProducts", "Count", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserProducts", "Count");
            DropColumn("dbo.UserProducts", "Sum");
            DropColumn("dbo.UserProducts", "Sent");
        }
    }
}
