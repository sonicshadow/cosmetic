namespace Cosmetic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MissionAddDataID : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Missions", "DataID", c => c.Int(nullable: false));
            AddColumn("dbo.UserProducts", "CreateTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Missions", "UserID", c => c.String(maxLength: 128));
            CreateIndex("dbo.Missions", "UserID");
            AddForeignKey("dbo.Missions", "UserID", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Missions", "UserID", "dbo.AspNetUsers");
            DropIndex("dbo.Missions", new[] { "UserID" });
            AlterColumn("dbo.Missions", "UserID", c => c.String());
            DropColumn("dbo.UserProducts", "CreateTime");
            DropColumn("dbo.Missions", "DataID");
        }
    }
}
