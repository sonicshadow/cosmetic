namespace Cosmetic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOrder : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Code = c.String(),
                        UserID = c.String(maxLength: 128),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Count = c.Int(nullable: false),
                        Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        State = c.Int(nullable: false),
                        Send = c.Int(nullable: false),
                        CreateDateTime = c.DateTime(nullable: false),
                        ReceiptDateTime = c.DateTime(),
                        Bank = c.String(),
                        BankCard = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.UserID)
                .Index(t => t.UserID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Orders", "UserID", "dbo.AspNetUsers");
            DropIndex("dbo.Orders", new[] { "UserID" });
            DropTable("dbo.Orders");
        }
    }
}
