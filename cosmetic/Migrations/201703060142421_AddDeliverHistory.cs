namespace Cosmetic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDeliverHistory : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DeliverHistories",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Count = c.Int(nullable: false),
                        UserID = c.String(),
                        Address = c.String(),
                        Express = c.String(),
                        Code = c.String(),
                        CreateDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.DeliverHistories");
        }
    }
}
