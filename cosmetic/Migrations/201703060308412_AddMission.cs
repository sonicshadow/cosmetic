namespace Cosmetic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMission : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MissionDetails",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        MissionID = c.Int(nullable: false),
                        StepID = c.Int(nullable: false),
                        UpdateTime = c.DateTime(nullable: false),
                        JData = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Missions", t => t.MissionID, cascadeDelete: true)
                .Index(t => t.MissionID);
            
            CreateTable(
                "dbo.Missions",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserID = c.String(),
                        State = c.Int(nullable: false),
                        Type = c.Int(nullable: false),
                        CreateDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MissionDetails", "MissionID", "dbo.Missions");
            DropIndex("dbo.MissionDetails", new[] { "MissionID" });
            DropTable("dbo.Missions");
            DropTable("dbo.MissionDetails");
        }
    }
}
