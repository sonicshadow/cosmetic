namespace Cosmetic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNotices : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Displays",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Subtitle = c.String(),
                        Tag = c.String(),
                        Images = c.String(),
                        Content = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Notices",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CreateTime = c.DateTime(nullable: false),
                        Title = c.String(),
                        Content = c.String(),
                        Sort = c.Int(nullable: false),
                        Publisher = c.String(),
                        Subtitle = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.Accounts", "Fee", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Accounts", "Fee");
            DropTable("dbo.Notices");
            DropTable("dbo.Displays");
        }
    }
}
