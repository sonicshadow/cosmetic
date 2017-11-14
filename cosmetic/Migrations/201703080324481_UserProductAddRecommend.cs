namespace Cosmetic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserProductAddRecommend : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserProducts", "Parent", c => c.String(nullable: false));
            AddColumn("dbo.UserProducts", "Recommend", c => c.String(nullable: false));
            DropColumn("dbo.AspNetUsers", "ParentID");
            DropColumn("dbo.AspNetUsers", "RecommendID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "RecommendID", c => c.String(nullable: false));
            AddColumn("dbo.AspNetUsers", "ParentID", c => c.String(nullable: false));
            DropColumn("dbo.UserProducts", "Recommend");
            DropColumn("dbo.UserProducts", "Parent");
        }
    }
}
