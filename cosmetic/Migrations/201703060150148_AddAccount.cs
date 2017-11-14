namespace Cosmetic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAccount : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AccountKinds",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Type = c.Int(nullable: false),
                        Name = c.String(),
                        Code = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Accounts",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        AccountKindID = c.Int(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UpdateDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AccountKinds", t => t.AccountKindID, cascadeDelete: true)
                .Index(t => t.AccountKindID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Accounts", "AccountKindID", "dbo.AccountKinds");
            DropIndex("dbo.Accounts", new[] { "AccountKindID" });
            DropTable("dbo.Accounts");
            DropTable("dbo.AccountKinds");
        }
    }
}
