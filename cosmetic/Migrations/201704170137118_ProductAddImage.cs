namespace Cosmetic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProductAddImage : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "Image", c => c.String());
            AddColumn("dbo.Products", "Tag", c => c.String());
            AddColumn("dbo.Products", "Subhead", c => c.String());
            DropTable("dbo.Displays");
        }
        
        public override void Down()
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
                    })
                .PrimaryKey(t => t.ID);
            
            DropColumn("dbo.Products", "Subhead");
            DropColumn("dbo.Products", "Tag");
            DropColumn("dbo.Products", "Image");
        }
    }
}
