namespace Cosmetic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserAddType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Type", c => c.Int(nullable: false));
            AlterColumn("dbo.UserProducts", "Parent", c => c.String());
            AlterColumn("dbo.UserProducts", "Recommend", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.UserProducts", "Recommend", c => c.String(nullable: false));
            AlterColumn("dbo.UserProducts", "Parent", c => c.String(nullable: false));
            DropColumn("dbo.AspNetUsers", "Type");
        }
    }
}
