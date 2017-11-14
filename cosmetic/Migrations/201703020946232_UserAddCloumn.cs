namespace Cosmetic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserAddCloumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "RealName", c => c.String());
            AddColumn("dbo.AspNetUsers", "IDCard", c => c.String());
            AddColumn("dbo.AspNetUsers", "Bank", c => c.String());
            AddColumn("dbo.AspNetUsers", "BankCard", c => c.String());
            AddColumn("dbo.AspNetUsers", "Type", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "ParentID", c => c.String());
            AddColumn("dbo.AspNetUsers", "RecommendID", c => c.String());
            AddColumn("dbo.AspNetUsers", "Address", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Address");
            DropColumn("dbo.AspNetUsers", "RecommendID");
            DropColumn("dbo.AspNetUsers", "ParentID");
            DropColumn("dbo.AspNetUsers", "Type");
            DropColumn("dbo.AspNetUsers", "BankCard");
            DropColumn("dbo.AspNetUsers", "Bank");
            DropColumn("dbo.AspNetUsers", "IDCard");
            DropColumn("dbo.AspNetUsers", "RealName");
        }
    }
}
