namespace Cosmetic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserAddRoleGroupID : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "RoleGroupID", c => c.Int(nullable: false));
            AlterColumn("dbo.AspNetUsers", "RealName", c => c.String());
            AlterColumn("dbo.AspNetUsers", "IDCard", c => c.String());
            AlterColumn("dbo.AspNetUsers", "Bank", c => c.String());
            AlterColumn("dbo.AspNetUsers", "BankCard", c => c.String());
            AlterColumn("dbo.AspNetUsers", "Address", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "Address", c => c.String(nullable: false));
            AlterColumn("dbo.AspNetUsers", "BankCard", c => c.String(nullable: false));
            AlterColumn("dbo.AspNetUsers", "Bank", c => c.String(nullable: false));
            AlterColumn("dbo.AspNetUsers", "IDCard", c => c.String(nullable: false));
            AlterColumn("dbo.AspNetUsers", "RealName", c => c.String(nullable: false));
            DropColumn("dbo.AspNetUsers", "RoleGroupID");
        }
    }
}
