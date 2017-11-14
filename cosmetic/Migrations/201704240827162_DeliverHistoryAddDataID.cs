namespace Cosmetic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeliverHistoryAddDataID : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DeliverHistories", "DataID", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DeliverHistories", "DataID");
        }
    }
}
