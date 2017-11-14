namespace Cosmetic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDisplays : DbMigration
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
                        Specification = c.String(),
                        Content = c.String(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.ID);
            
            DropColumn("dbo.Products", "Image");
            DropColumn("dbo.Products", "Tag");
            DropColumn("dbo.Products", "Subhead");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "Subhead", c => c.String());
            AddColumn("dbo.Products", "Tag", c => c.String());
            AddColumn("dbo.Products", "Image", c => c.String());
            DropTable("dbo.Displays");
        }
    }
}
