namespace Cosmetic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserAddDateTime : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "RegisterDateTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.AspNetUsers", "LastLoginDateTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "LastLoginDateTime");
            DropColumn("dbo.AspNetUsers", "RegisterDateTime");
        }
    }
}
