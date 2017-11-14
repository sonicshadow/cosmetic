namespace Cosmetic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StaffAddEntryTime : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Accounts", "Pay", c => c.Int(nullable: false));
            AddColumn("dbo.Staffs", "EntryTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Staffs", "EntryTime");
            DropColumn("dbo.Accounts", "Pay");
        }
    }
}
