namespace Cosmetic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserIncome : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserIncomes",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserID = c.String(maxLength: 128),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Type = c.Int(nullable: false),
                        CreateDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.UserID)
                .Index(t => t.UserID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserIncomes", "UserID", "dbo.AspNetUsers");
            DropIndex("dbo.UserIncomes", new[] { "UserID" });
            DropTable("dbo.UserIncomes");
        }
    }
}
