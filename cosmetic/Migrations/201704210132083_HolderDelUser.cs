namespace Cosmetic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HolderDelUser : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Holders", "UserID", "dbo.AspNetUsers");
            DropIndex("dbo.Holders", new[] { "UserID" });
            AlterColumn("dbo.Holders", "UserID", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Holders", "UserID", c => c.String(maxLength: 128));
            CreateIndex("dbo.Holders", "UserID");
            AddForeignKey("dbo.Holders", "UserID", "dbo.AspNetUsers", "Id");
        }
    }
}
