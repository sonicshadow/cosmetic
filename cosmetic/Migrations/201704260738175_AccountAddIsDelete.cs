namespace Cosmetic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AccountAddIsDelete : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Accounts", "IsDelete", c => c.Boolean(nullable: false));
            AddColumn("dbo.Accounts", "AllowDelete", c => c.Boolean(nullable: false));
            AddColumn("dbo.UserProducts", "TwiceMin", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserProducts", "TwiceMin");
            DropColumn("dbo.Accounts", "AllowDelete");
            DropColumn("dbo.Accounts", "IsDelete");
        }
    }
}
