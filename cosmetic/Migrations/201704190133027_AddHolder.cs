namespace Cosmetic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddHolder : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Holders",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserID = c.String(maxLength: 128),
                        Stock = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.UserID)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.Modifies",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        NewData = c.String(),
                        OldData = c.String(),
                        UserID = c.String(maxLength: 128),
                        Time = c.DateTime(nullable: false),
                        ModifyType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.UserID)
                .Index(t => t.UserID);
            
            AddColumn("dbo.Accounts", "Trader", c => c.String());
            AddColumn("dbo.Accounts", "TraderType", c => c.Int(nullable: false));
            AddColumn("dbo.Accounts", "UserType", c => c.Int());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Modifies", "UserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.Holders", "UserID", "dbo.AspNetUsers");
            DropIndex("dbo.Modifies", new[] { "UserID" });
            DropIndex("dbo.Holders", new[] { "UserID" });
            DropColumn("dbo.Accounts", "UserType");
            DropColumn("dbo.Accounts", "TraderType");
            DropColumn("dbo.Accounts", "Trader");
            DropTable("dbo.Modifies");
            DropTable("dbo.Holders");
        }
    }
}
