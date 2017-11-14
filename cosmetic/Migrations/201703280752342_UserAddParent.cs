namespace Cosmetic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserAddParent : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "AllParent", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "AllParent");
        }
    }
}
